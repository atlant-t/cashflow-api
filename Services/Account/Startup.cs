using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Cashflow.Services.Account.Filters;
using Cashflow.Services.Account.Services;

namespace Cashflow.Services.Account
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddScoped<IMongoDatabase>(provider => databaseFactory())
              .AddTransient<AccountService, AccountService>()
              .AddControllers(options =>
              {
                options.Filters.Add(typeof(MongoConfigurationExceptionFilter));
              });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

    private IMongoDatabase databaseFactory() {
      string connectionString = Configuration["db-url"]
                             ?? Configuration["CASHFLOW_ACCOUNT_DB_URL"]
                             ?? "mongodb://localhost:27017";
      MongoUrlBuilder connection = new MongoUrlBuilder(connectionString);
      connection.DatabaseName = connection.DatabaseName ?? "cashflow-account";

      MongoClient client = new MongoClient(connection.ToMongoUrl());
      return client.GetDatabase(connection.DatabaseName);
    }
  }
}
