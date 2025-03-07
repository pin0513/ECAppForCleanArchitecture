using System.ComponentModel.DataAnnotations;
using ECApp.Application.Common;
using ECApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECApp.Backend.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _getErrorMessageFromDb;

        public ApiExceptionFilterAttribute(IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService, ILogService logService)
        {
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            LogError(context);

            base.OnException(context);
        }

        private void LogError(ExceptionContext context)
        {
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleValidationException(ExceptionContext context)
        {
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
        }

        private void HandleErrorCodeMessageException(ExceptionContext context)
        {
        }


        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
        }

        private void HandleForbiddenAccessException(ExceptionContext context)
        {
        }

        private void HandleUnknownException(ExceptionContext context)
        {
        }
    }
}