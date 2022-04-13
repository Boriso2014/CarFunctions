using System;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace CarFunctions
{
    public sealed class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger _logger;

        public ExceptionMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected Error in {EntryPoint}: {Message}", context.FunctionDefinition.EntryPoint, ex.Message);
                throw;
            }
        }
    }
}
