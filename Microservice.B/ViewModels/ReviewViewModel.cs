using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.B.ViewModels
{
    public class ReviewViewModel
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string Reviewer { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
    }
}
