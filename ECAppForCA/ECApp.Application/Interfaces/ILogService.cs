namespace ECApp.Application.Interfaces;

public interface ILogService
{
    void LogUpdate(string tableName, long rowId, string columnName, object? originalValue, object? currentValue);
    void LogStep(ICurrentUserService currentUserService, string stepName);
    void Debug(string message, Exception e = null, ICurrentUserService currentUserService = null);
    void Information(string message, Exception e = null, ICurrentUserService currentUserService = null);
    void LogAccess(string message, Exception e = null, ICurrentUserService currentUserService = null);
    void Warning(string message, Exception e = null, ICurrentUserService currentUserService = null);
    void Error(string message, Exception e = null, ICurrentUserService currentUser = null);
    void Fatal(string message, Exception e = null, ICurrentUserService currentUser = null);
}