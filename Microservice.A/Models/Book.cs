using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.A.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int NPages { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Blurb { get; set; }
        public string FrontCoverImageUrl { get; set; }
        public string BackCoverImageUrl { get; set; }
    }
}
