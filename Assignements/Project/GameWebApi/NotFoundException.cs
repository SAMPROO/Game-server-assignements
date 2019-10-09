using System;
using Newtonsoft.Json.Linq;

public class NotFoundException : Exception
{
    public enum ErrorType
    {
        GUID,
        STRING,
        INTEGER,

        OTHER,
    }

    private void ErrorMessage(ErrorType type, object usedParam)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("TYPE (" + type + ") Not Found: "+ usedParam.ToString());
        Console.ResetColor();
    }
    public int StatusCode { get; set; }
    public string ContentType { get; set; } = @"text/plain";

    public NotFoundException()
    {
        
    }

    public NotFoundException(ErrorType type, object usedParam)
    {
        switch (type)
        {
            case ErrorType.GUID:
                ErrorMessage(type, usedParam);
                break;
            case ErrorType.INTEGER:
                ErrorMessage(type, usedParam);
                break;
            case ErrorType.STRING:
                ErrorMessage(type, usedParam);
                break;
            case ErrorType.OTHER:
                ErrorMessage(type, usedParam);
                break;
            default:
                ErrorMessage(type, usedParam);
                break;
        }
    }
    public NotFoundException (int statusCode)
    {
        this.StatusCode = statusCode;
    }

    public NotFoundException (string message) : base(message)
    {
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