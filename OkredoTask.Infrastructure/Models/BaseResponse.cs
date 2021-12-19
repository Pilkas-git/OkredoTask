namespace OkredoTask.Infrastructure.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public BaseResponse()
        {
            Success = true;
        }

        public BaseResponse(string error)
        {
            Success = false;
            Error = error;
        }
    }
}