﻿using System;
using GraphQlRethinkDbLibrary.Database;
using GraphQlRethinkDbLibrary.Handlers;
using Listen.Api.Handlers;
using Listen.Api.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Listen.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE");
            BaseHandler.Setup(
                app,
                env,
                new DatabaseName(Program.DatabaseName),
                new DatabaseUrl(databaseUrl),
                new GraphQlHandler<Query, Mutation>(),
                new ImageHandler());
        }
    }
}
