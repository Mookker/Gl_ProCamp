using System.Net;

namespace CommonLibrary.Models.Errors
{
    public class BadRequestResponse : ErrorResponse
    {
        public BadRequestResponse(string description) : base(HttpStatusCode.BadRequest, description)
        {
        }
    }
}