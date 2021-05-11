using Domain;
using Domain.Read.ToDo.Projection;
using Domain.Read.ToDoList.Projection;
using Domain.Write.ToDo;
using Domain.Write.ToDoList;
using FluentValidation;
using Infrastructure.InMemory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            // Event Sourcing
            services.AddSingleton<IAggregateRepository<ToDo>, InMemoryToDoRepository>();
            services.AddSingleton<IAggregateRepository<ToDoList>, InMemoryToDoListRepository>();

            // CQRS
            services.AddMediatR(typeof(Domain.Write.AssemblyFinder), typeof(Domain.Read.AssemblyFinder));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddSingleton<IToDoProjectionRepository, InMemoryToDoProjectionRepository>();
            services.AddSingleton<IToDoListProjectionRepository, InMemoryToDoListProjectionRepository>();
            
            // Validation
            services.AddValidatorsFromAssembly(typeof(Domain.Write.AssemblyFinder).Assembly);
            
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Application", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Application v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}