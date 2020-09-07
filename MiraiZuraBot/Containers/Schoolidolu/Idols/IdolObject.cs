using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Idols
{
    public class IdolObject
    {
        public string Name { get; set; }
        public string Japanese_name { get; set; }
        public bool Main { get; set; }
        public int? Age { get; set; }
        public string School { get; set; }
        public string Birthday { get; set; }
        public string Astrological_sign { get; set; }
        public string Blood { get; set; }
        public int? Height { get; set; }
        public string Measurements { get; set; }
        public string Favorite_food { get; set; }
        public string Least_favorite_food { get; set; }
        public string Hobbies { get; set; }
        public string Attribute { get; set; }
        public string Year { get; set; }
        public string Main_unit { get; set; }
        public string Sub_unit { get; set; }
        public CVObject Cv { get; set; }
        public string Summary { get; set; }
        public string Website_url { get; set; }
        public string Wiki_url { get; set; }
        public string Wikia_url { get; set; }
        public string Official_url { get; set; }
        public string Chibi { get; set; }
        public string Chibi_small { get; set; }
    }
}
