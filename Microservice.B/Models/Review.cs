using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.B.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
    }
    public class Review
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookCategory { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
    }
}
