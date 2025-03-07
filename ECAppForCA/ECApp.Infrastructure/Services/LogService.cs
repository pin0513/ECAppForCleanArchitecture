using System.Diagnostics;
using System.Reflection;
using ECApp.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ECApp.Infrastructure.Services
{
    /// <summary>
    /// LogService
    /// </summary>
    public class LogService : ILogService
    {
        private const string RequestId = "RequestId";

        private Stopwatch _stopwatch = new Stopwatch();
        private ICurrentUserService _currentUserService { get; set; }

        public LogService(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }


        public void LogUpdate(string tableName, long rowId, string columnName, object? originalValue,
            object? currentValue)
        {
        }

        public void LogStep(ICurrentUserService currentUserService, string stepName)
        {
        }

        public void Debug(string message, Exception e = null, ICurrentUserService currentUserService = null)
        {
        }

        public void Information(string message, Exception e = null, ICurrentUserService currentUserService = null)
        {
        }

        public void LogAccess(string message, Exception e = null, ICurrentUserService currentUserService = null)
        {
        }

        public void Warning(string message, Exception e = null, ICurrentUserService currentUserService = null)
        {
        }

        public void Error(string message, Exception e = null, ICurrentUserService? currentUserService = null)
        {
        }

        
        public void Fatal(string message, Exception e = null, ICurrentUserService? currentUserService = null)
        {
        }

        private static StreamWriter? _logStream = null;
    }
}