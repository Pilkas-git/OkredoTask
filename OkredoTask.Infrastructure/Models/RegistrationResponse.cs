using System.Collections.Generic;

namespace OkredoTask.Infrastructure.Models
{
    public class RegistrationResponse
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}