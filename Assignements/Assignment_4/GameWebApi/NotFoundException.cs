using System;
using Newtonsoft.Json.Linq;

public class NotFoundException : Exception
{
    public int StatusCode { get; set; }
    public string ContentType { get; set; } = @"text/plain";

    public NotFoundException (int statusCode)
    {
        this.StatusCode = statusCode;
    }

    public NotFoundException (int statusCode, string message) : base(message)
    {
        this.StatusCode = statusCode;
    }

    public NotFoundException (int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

    public NotFoundException (int statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
    {
        this.ContentType = @"application/json";
    }
}