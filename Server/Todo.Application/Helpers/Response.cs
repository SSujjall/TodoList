using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Helpers
{
    public class Response
    {
        public Response(object? data, List<string> errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = data;
            Errors = errors ?? new List<string>();
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
