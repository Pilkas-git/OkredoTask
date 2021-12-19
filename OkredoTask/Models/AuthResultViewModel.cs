using System.Collections.Generic;

namespace OkredoTask.Web.Models
{
    public class AuthResultViewModel
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
    }
}