using Microservice.B.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microservice.B.Filters;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;

namespace Microservice.B
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //  Install packages: Polly and Microsoft.Extensions.Http.Polly
            var policy1 = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(
                    2,
                    TimeSpan.FromSeconds(30),
                    (r, ts) => Console.WriteLine("Broken"),
                    () => Console.WriteLine("Closed"),
                    () => Console.WriteLine("Half open")
                    );
            
            //  10% of requests should fail in 60 seconds, with minimum threshold of 3
            //  If so, then the circuit remains broken for the next 1 minute
            var policy2 = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .AdvancedCircuitBreakerAsync(0.1, TimeSpan.FromSeconds(30), 3, TimeSpan.FromMinutes(1));

            services.AddControllers(options => options.Filters.Add<GeneralExceptionFilter>());
            services.AddHttpClient("appHttpClient", client => { client.BaseAddress = new Uri(_configuration["BookServiceUrl"]); })
                .AddPolicyHandler(policy1);


            services.AddScoped<IBookService, BookService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}