using System;
using System.Collections.Generic;
using System.Text;

namespace NOnStar.CommandAndControl
{
    public class CommandRequestStatus
    {
        public bool Successful { get; internal set; }
        public string ErrorMessage { get; internal set; }

        public static CommandRequestStatus GetSuccessful()
        {
            return new CommandRequestStatus() { Successful = true };
        }

        public static CommandRequestStatus GetFailed(string errorMessage)
        {
            return new CommandRequestStatus() { Successful = false, ErrorMessage = errorMessage };
        }
    }

    public class CommandRequestStatus<T> : CommandRequestStatus
    {
        public T Content { get; internal set; }

        public static CommandRequestStatus<T> GetSuccessful(T content)
        {
            return new CommandRequestStatus<T>() { Successful = true, Content = content };
        }

        public static new CommandRequestStatus<T> GetFailed(string errorMessage)
        {
            return new CommandRequestStatus<T>() { Successful = false, ErrorMessage = errorMessage };
        }
    }
}
