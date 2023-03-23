
// custom exception class for throwing application specific exceptions (e.g. for validation) 
// that can be caught and handled within the application
namespace BlazorAnnuaireProject.Helpers
{
    public class AppException : Exception
    {
        public int HttpStatusCode { get; set; }

        public AppException(int httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }
    }

}