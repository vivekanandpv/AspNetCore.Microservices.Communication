using Confluent.Kafka;
using Microservice.B.Models;
using Microservice.B.MQ;
using Microservice.B.Services;
using Microservice.B.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IRabbitMQManager _rabbitMQManager;

        public ReviewsController(IBookService bookService, IRabbitMQManager rabbitMQManager)
        {
            _bookService = bookService;
            _rabbitMQManager = rabbitMQManager;
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

            //  Send message to Microservice.C
            var messageModel = new Message
            {
                CreatedOn = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                MessageBody = $"Review written for Book id: {viewModel.BookId} by: {viewModel.Reviewer}",
                MessageTitle = "New Review"
            };

            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-b-routing");

            return Ok(new { Message = "Created", Payload = review });
        }

    }
}
