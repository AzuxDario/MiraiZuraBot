using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Cards
{
    public class CardsRequest
    {
        // Optional parameters for filtering
        public string Search { get; set; }
        public string Ids { get; set; }
        public string Name { get; set; }
        public string Japanese_name { get; set; }
        public string Rarity { get; set; }
        public string Attribute { get; set; }
        public string Japanese_collection { get; set; }
        public int? Hp { get; set; }
        public string Skill { get; set; }
        public string Center_skill { get; set; }
        public string Translated_collection { get; set; }
        public string Idol_year { get; set; }
        public string Idol_main_unit { get; set; }
        public string Idol_sub_unit { get; set; }
        public string Idol_school { get; set; }
        public string Event_japanese_name { get; set; }
        public string Event_english_name { get; set; }
        public string Ur_pair_name { get; set; }
        public bool? Japan_only { get; set; }
        public bool? Is_promo { get; set; }
        public bool? Is_special { get; set; }
        public bool? Is_event { get; set; }

        // Ordering by any field is possible.
        // Ordering by random is possible.
        public string Ordering { get; set; }

        // Optional parameters to get more data
        /*public bool? Expand_idol { get; set; }
        public bool? Expand_event { get; set; }
        public bool? Expand_ur_pair { get; set; }*/
    }
}
