using System;

namespace CommunicationLibrary.Exceptions
{
    public class InternalServiceException : Exception
    {
        public InternalServiceException(Exception ex) : base("Internal server error", ex)
        {
            
        }
    }
}