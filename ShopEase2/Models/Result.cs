using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed class Result
    {
        private Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public string? Error { get; }

        public static Result Ok() =>
            new(true, null);

        public static Result Fail(string error) =>
            new(false, error);
    }

    public sealed class Result<T>
    {
        private Result(
            bool isSuccess,
            T? data,
            string? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public bool IsSuccess { get; }

        public T? Data { get; }

        public string? Error { get; }

        public static Result<T> Ok(T data) =>
            new(true, data, null);

        public static Result<T> Fail(string error) =>
            new(false, default, error);
    }
}
