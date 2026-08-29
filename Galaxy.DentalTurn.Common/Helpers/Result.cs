using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Galaxy.DentalTurn.Common.Helpers
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string? Message { get; }
        public IReadOnlyCollection<string> Errors { get; }

        protected internal Result(bool isSuccess, string? message, IEnumerable<string> errors = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors?.ToList() ?? new List<string>();
        }


        public static Result Success(string? message) => new(true, message);
        public static Result Failure(string error) => new(false, error);
        public static Result Failure(IEnumerable<string> error) => new(false, null, error);
        public static Result<T> Success<T>(T value) => new(value, true, null);

        public static Result<T> Failure<T>(string error) => new(default, false, error);
        public static Result<T> Failure<T>(IEnumerable<string> error) => new(default, false, null, error);

    }
        
    public class Result<T> : Result
    {
        public T? Value { get; }
        protected internal Result(T? value, bool isSuccess, string? message, IEnumerable<string> errors = null)
            : base(isSuccess, message, errors)
        {
            Value = value;
        }   

    }
}
