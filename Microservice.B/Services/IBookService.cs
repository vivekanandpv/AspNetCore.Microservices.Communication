using Microservice.B.Extensions;
using Microservice.B.Models;
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
            var response = await _client.GetAsync($"/api/books/{id}");
            return await response.GetAs<Book>();    //  see the extension method
        }
    }
}
