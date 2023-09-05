using System.Net;

namespace MagicVilla_VillaAPI.Models
{
    public class APIResponce
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
