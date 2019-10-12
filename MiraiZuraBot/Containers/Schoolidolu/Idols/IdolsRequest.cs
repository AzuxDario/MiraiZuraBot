using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Idols
{
    class IdolsRequest
    {
        // Optional parameters for filtering
        public string Search { get; set; }
        public string Japanese_name { get; set; }
        public int? Age { get; set; }
        public string School { get; set; }
        public string Astrological_sign { get; set; }
        public string Blood { get; set; }
        public string Attribute { get; set; }
        public string Year { get; set; }
        public string Main_unit { get; set; }
        public string Sub_unit { get; set; }
        public bool? Main { get; set; }
        public bool? Card__is_special { get; set; }

        // Ordering by any field is possible.
        // Ordering by random is possible.
        public string Ordering { get; set; }
    }
}
