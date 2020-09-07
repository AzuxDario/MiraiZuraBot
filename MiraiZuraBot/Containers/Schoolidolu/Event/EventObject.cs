using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Event
{
    public class EventObject
    {
        public string Japanese_name { get; set; }
        public string Romaji_name { get; set; }
        public string English_name { get; set; }
        public string Translated_name { get; set; }
        public string Image { get; set; }
        public string English_image { get; set; }
        public DateTime? Beginning { get; set; }
        public DateTime? End { get; set; }
        public DateTime? English_beginning { get; set; }
        public DateTime? English_end { get; set; }
        public bool? Japan_current { get; set; }
        public bool? World_current { get; set; }
        public string English_status { get; set; }
        public string Japan_status { get; set; }
        public int? Japanese_t1_points { get; set; }
        public int? Japanese_t1_rank { get; set; }
        public int? Japanese_t2_points { get; set; }
        public int? Japanese_t2_rank { get; set; }
        public int? English_t1_points { get; set; }
        public int? English_t1_rank { get; set; }
        public int? English_t2_points { get; set; }
        public int? English_t2_rank { get; set; }
        public string Note { get; set; }
        public string Website_url { get; set; }
    }
}
