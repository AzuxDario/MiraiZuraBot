using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Songs
{
    public class SongObject
    {
        public string Name { get; set; }
        public string Romaji_name { get; set; }
        public string Translated_name { get; set; }
        public string Attribute { get; set; }
        public string Main_unit { get; set; }
        public int? Bpm { get; set; }
        public int? Time { get; set; }
        public string Event { get; set; }
        public int? Rank { get; set; }
        public string Daily_rotation { get; set; }
        public int? Daily_rotation_position { get; set; }
        public string Image { get; set; }
        public int? Easy_difficulty { get; set; }
        public int? Easy_notes { get; set; }
        public int? Normal_difficulty { get; set; }
        public int? Normal_notes { get; set; }
        public int? Hard_difficulty { get; set; }
        public int? Hard_notes { get; set; }
        public int? Expert_difficulty { get; set; }
        public int? Expert_notes { get; set; }
        public int? Master_difficulty { get; set; }
        public int? Master_notes { get; set; }
        public bool? Available { get; set; }
        public int? Itunes_id { get; set; }
        public string Website_url { get; set; }
    }
}
