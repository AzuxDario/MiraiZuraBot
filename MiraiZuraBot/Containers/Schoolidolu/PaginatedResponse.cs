using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu
{
    public class PaginatedResponse<T>
    {
        public int? Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<T> Results { get; set; }
    }

}
