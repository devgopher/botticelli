using System;

namespace Viber.Api.Exceptions
{
    public class ViberClientException : Exception
    {
        public ViberClientException(string message, Exception? inner = null) : base(message, inner)
        {
        }
    }
}