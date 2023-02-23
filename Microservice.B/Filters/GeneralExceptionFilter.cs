using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microservice.B.Filters
{
    public class GeneralExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => Int32.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ExceptionHandled && context.Exception != null)
            {
                context.Result = new ObjectResult(new { Message = "Cannot process..." })
                {
                    StatusCode = 400
                };

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}