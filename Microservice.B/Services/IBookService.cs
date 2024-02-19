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
        Task<Book> GetBook(string id);
    }

    public class BookService : IBookService
    {
        private readonly HttpClient client;

        public BookService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Book> GetBook(string id)
        {
            var response = await client.GetAsync($"/api/books/{id}");
            return await response.GetAs<Book>();    //  see the extension method
        }
    }
}
