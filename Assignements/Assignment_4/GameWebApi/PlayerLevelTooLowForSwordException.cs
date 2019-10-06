using System;

public class PlayerLevelTooLowForSwordException : Exception
{
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
}