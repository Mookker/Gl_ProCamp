using System;
using System.Net;

namespace CommonLibrary.Models.Errors
{
    [Serializable]
    public class NotFoundErrorResponse : ErrorResponse
    {
        public NotFoundErrorResponse(string resourceDescription) : base(HttpStatusCode.NotFound, $"Not found {resourceDescription}")
        {
            
        }
    }
}