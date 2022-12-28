using System;

namespace ProjectOrigin
{
    /// <summary>An implementation of ILogger that takes log messages and writes them to console with
    /// the date/time.</summary>
    public class Logger : ILogger
    {
        /// <summary>Log a message by printing it to console with the date/time.</summary>
        public void Log(string message)
        {
            if (message is null)
                throw new ArgumentException("message cannot be null.");

            Console.WriteLine($"[{DateTime.Now.ToString("dd/M HH:mmtt")}] - {message}");
        }
    }
}