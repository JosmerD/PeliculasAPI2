using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasApi.Filtros
{
    public class FiltrosDeExcepcion:ExceptionFilterAttribute
    {
        private readonly ILogger<FiltrosDeExcepcion> logger;

        public FiltrosDeExcepcion(ILogger<FiltrosDeExcepcion>logger)
        {
            this.logger = logger;
        }
        public override void OnException (ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }

    }
}
