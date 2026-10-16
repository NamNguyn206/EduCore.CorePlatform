using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduCore.CorePlatform.Application.Common.Models;

// =========================
// BASE RESULT
// =========================
public class Result
{
    public bool IsSuccess { get; protected set; }

    public string? Message { get; protected set; }

    public ErrorResult? Error { get; protected set; }

    protected Result()
    {
    }

    // -------- SUCCESS --------
    public static Result Success(string? message = null)
    {
        return new Result
        {
            IsSuccess = true,
            Message = message
        };
    }

    // -------- FAIL --------
    public static Result Failure(string code, string message)
    {
        return new Result
        {
            IsSuccess = false,
            Error = new ErrorResult(code, message)
        };
    }
}

// =========================
// GENERIC RESULT
// =========================
public sealed class Result<T> : Result
{
    public T? Data { get; private set; }

    private Result()
    {
    }

    // -------- SUCCESS --------
    public static Result<T> Success(
        T data,
        string? message = null)
    {
        return new Result<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    // -------- FAIL --------
    public static Result<T> Failure(
        string code,
        string message)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Error = new ErrorResult(code, message)
        };
    }
}

