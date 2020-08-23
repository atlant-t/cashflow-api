using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Cashflow.Services.Account.Filters
{
  /// <summary>
  /// A filter that runs after an Mongo action has thrown exception.
  /// </summary>
  public class MongoConfigurationExceptionFilter : IExceptionFilter
  {
    private readonly ILogger<MongoConfigurationExceptionFilter> logger;

    public MongoConfigurationExceptionFilter(ILogger<MongoConfigurationExceptionFilter> logger)
    {
      this.logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
      if (context.Exception == null)
      {
        return;
      }

      // In the absence of a response from a dependent server (as Mongodb),
      // we notify the client about the impossibility to process the request.
      if (context.Exception is TimeoutException timeoutException)
      {
        logger.LogError(context.Exception, timeoutException.Message);

        context.Result = _status521ServerIsDown();
        context.ExceptionHandled = true;
      }

      // In case of misconfiguration of Mongodb,
      // we notify the client that the request cannot be processed.
      if (context.Exception is MongoConfigurationException configurationException)
      {
        logger.LogError(context.Exception, configurationException.Message);

        context.Result = _status521ServerIsDown();
        context.ExceptionHandled = true;
      }
    }

    private IActionResult _status521ServerIsDown()
    {
      var details = new ProblemDetails()
      {
        Status = 521,
        Title = "Server is down",
      };

      var result = new ObjectResult(details)
      {
        StatusCode = details.Status,
      };

      return result;
    }
  }
}
