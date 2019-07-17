using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Cards
{
    public class CardsResponse
    {
        public int? Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<CardObject> Results { get; set; }
    }
}
