using Microservice.B.Models;
using Microservice.B.Services;
using Microservice.B.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IBookService _bookService;

        public ReviewsController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Review>> Get(Guid id)
        {
            return Ok(new Review
            {
                BookId = Guid.NewGuid(),
                Id = id,
                Body = "Review body goes here",
                Rating = 4,
                Reviewer = "Santosh",
                Title = "Good book for learners of Asp.Net Core"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(ReviewCreateViewModel viewModel)
        {
            //  Get book details here from Book microservice
            var book = await _bookService.GetBook(viewModel.BookId);

            //  create domain object and save
            var review = new Review
            {
                BookId = book.Id,
                BookTitle = book.Title,
                BookCategory = book.Category,
                Id = Guid.NewGuid(),
                Body = viewModel.Body,
                Title = viewModel.Title,
                Rating = viewModel.Rating,
                Reviewer = viewModel.Reviewer
            };


            return Ok(new { Message = "Created", Payload = review });
        }

    }
}
