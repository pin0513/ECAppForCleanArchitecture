using ECApp.Domain.Enums;

namespace ECApp.Application.Common
{
    public class Result
    {
        private Result()
        {
        }

        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Error { get; set; }
        public string? ErrorMessage { get; set; }

        public bool IsSuccess => Status == (int)ApiResultStatus.Success;
        public bool IsFailure => !IsSuccess;
        
        public static Result Success(string? message = null)
        {
            return new Result
            {
                Status = (int)ApiResultStatus.Success,
                Message = message
            };
        }

        public static Result<T> Success<T>(T data, string message = null) where T : class
        {
            return new Result<T>()
            {
                Status = (int)ApiResultStatus.Success,
                Data = data,
                Message = message
            };
        }

        public static Result Failure(string? errorMessage, object? error = null)
        {
            return new Result()
            {
                Status = (int)ApiResultStatus.Failure,
                Error = error,
                ErrorMessage = errorMessage
            };
        }
    }

    
    public class Result<TData>
    {
        internal Result()
        {
        }

        public int Status { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }
        public object Error { get; set; }
        public string ErrorMessage { get; set; }

        public static Result<TData> Success(TData? data, string message = null)
        {
            return new Result<TData>()
            {
                Status = (int)ApiResultStatus.Success,
                Data = data,
                Message = message
            };
        }

        public static Result<TData> Failure(string errorMessage, object error = null)
        {
            return new Result<TData>()
            {
                Status = (int)ApiResultStatus.Failure,
                Error = error,
                ErrorMessage = errorMessage
            };
        }

        public Result<T> WithTypeAs<T>() where T : class
        {
            return new Result<T>()
            {
                Status = Status,
                Data = Data as T,
                Message = Message,
                Error = Error,
                ErrorMessage = ErrorMessage
            };
        }
    }

}