using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class PlayerLevelTooLowForSwordException : Exception
{
    /*
    public PlayerLevelTooLowForSwordException()
    {
    }

    public PlayerLevelTooLowForSwordException(string message)
        : base(message)
    {
    }

    public PlayerLevelTooLowForSwordException(string message, Exception inner)
        : base(message, inner)
    {
    }
    */
}
public class PlayerLevelTooLowForSwordExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if(context.Exception is PlayerLevelTooLowForSwordException)
        {
            context.Result = new ContentResult{Content = "Player level too low for sword"};
        }
    }
}