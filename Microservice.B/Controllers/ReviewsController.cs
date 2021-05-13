using Microservice.B.Models;
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

        [HttpPost("{bookId:guid}")]
        public async Task<IActionResult> Post(ReviewViewModel viewModel)
        {
            //  Get book details here from Book microservice
            //  create domain object and save

            return Ok(new { Message = "Not yet implemented" });
        }

    }
}
