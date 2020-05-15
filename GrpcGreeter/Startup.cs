﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrpcGreeter.Protos;
using GrpcGreeter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcGreeter
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddGrpc();

      services.AddDbContext<AppDbContext>(options => options.UseSqlServer("data source=.\\SQLEXPRESS; initial catalog=NewBank;integrated security=true"));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGrpcService<PersonCRUDService>();
        endpoints.MapGrpcService<SkillCRUDService>();
        endpoints.MapGrpcService<AccountCRUDService>();

        endpoints.MapGet("/", async context =>
              {
            await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
          });
      });
    }
  }
}
