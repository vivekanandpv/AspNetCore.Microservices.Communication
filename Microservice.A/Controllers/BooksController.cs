using Microservice.A.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Book>> Get(Guid id)
        {
            return Ok(new Book
            {
                Id = id,
                BackCoverImageUrl = "http://BackCoverImageUrl",
                Blurb = "Book blurb goes here",
                Category = "Non-fiction",
                Description = "Book description goes here",
                FrontCoverImageUrl = "http://FrontCoverImageUrl",
                NPages = 524,
                Price = 1000.25,
                Title = "Asp.Net Core Programming"
            });
        }
    }
}
