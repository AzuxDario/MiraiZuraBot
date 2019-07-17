using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Idols
{
    class IdolsResponse
    {
        public int? Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<IdolObject> Results { get; set; }
    }
}
