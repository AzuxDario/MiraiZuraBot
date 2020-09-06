using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MiraiZuraBot.Services.SchoolidoluService
{
    class SchoolidoluResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; }

        public SchoolidoluResponse(T data, HttpStatusCode status)
        {
            Data = data;
            StatusCode = status;
        }
    }
}
