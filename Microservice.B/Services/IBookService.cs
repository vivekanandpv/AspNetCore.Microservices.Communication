using Microservice.B.Extensions;
using Microservice.B.Models;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice.B.Services
{
    public interface IBookService
    {
        Task<Book> GetBook(Guid id);
    }

    public class BookService : IBookService
    {
        private readonly HttpClient _client;

        public BookService(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("appHttpClient");
        }

        public async Task<Book> GetBook(Guid id)
        {
            var policy1 = Policy.BulkheadAsync(3, async (ctx) =>
            {
                await Console.Out.WriteLineAsync($"Bulkhaed failed");
            });

            int bulkheadAvailableCount = policy1.BulkheadAvailableCount;
            int queueAvailableCount = policy1.QueueAvailableCount;

            List<Task<HttpResponseMessage>> tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < 10; i++)
            {
                var t = policy1.ExecuteAsync(async () =>
                {
                    await Console.Out.WriteLineAsync($"Attempt: {i}; available: {bulkheadAvailableCount}");
                    return await _client.GetAsync($"/api/books/{id}");
                });

                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());



            return await Task.FromResult(new Book
            {
                Category = "Fiction",
                Id = Guid.NewGuid(),
                Title = "War and Peace"
            });    //  see the extension method
        }
    }
}
