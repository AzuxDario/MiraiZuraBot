using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Containers.Schoolidolu.Cards
{
    public class CardObject
    {
        public string Id { get; set; }
        public string Game_id { get; set; }
        public MiniIdolObject Idol { get; set; }
        public string Rarity { get; set; }
        public string Attribute { get; set; }
        public string Japanese_collection { get; set; }
        public string Translated_collection { get; set; }
        public string Japanese_attribute { get; set; }
        public bool? Is_promo { get; set; }
        public string Promo_item { get; set; }
        public string Promo_link { get; set; }
        public string Release_date { get; set; }
        public bool? Japan_only { get; set; }
        public MiniEventObject Event { get; set; }
        public MiniEventObject Other_event { get; set; }
        public bool? Is_special { get; set; }
        public int? Hp { get; set; }
        public string Minimum_statistics_smile { get; set; }
        public string Minimum_statistics_pure { get; set; }
        public string Minimum_statistics_cool { get; set; }
        public string Non_idolized_maximum_statistics_smile { get; set; }
        public string Non_idolized_maximum_statistics_pure { get; set; }
        public string Non_idolized_maximum_statistics_cool { get; set; }
        public string Idolized_maximum_statistics_smile { get; set; }
        public string Idolized_maximum_statistics_pure { get; set; }
        public string Idolized_maximum_statistics_cool { get; set; }
        public string Skill { get; set; }
        public string Skill_details { get; set; }
        public string Japanese_skill_details { get; set; }
        public string Center_skill { get; set; }
        public string Center_skill_extra_type { get; set; }
        public string Center_skill_details { get; set; }
        public string Japanese_center_skill { get; set; }
        public string Japanese_center_skill_details { get; set; }
        public string Card_image { get; set; }
        public string Card_idolized_image { get; set; }
        public string English_card_image { get; set; }
        public string English_card_idolized_image { get; set; }
        public string Round_card_image { get; set; }
        public string Round_card_idolized_image { get; set; }
        public string English_round_card_image { get; set; }
        public string English_round_card_idolized_image { get; set; }
        public string Video_story { get; set; }
        public string Japanese_video_story { get; set; }
        public string Website_url { get; set; }
        public string Non_idolized_max_level { get; set; }
        public string Idolized_max_level { get; set; }
        public string Transparent_image { get; set; }
        public string Transparent_idolized_image { get; set; }
        public string Clean_ur { get; set; }
        public string Clean_ur_idolized { get; set; }
        public List<TinyCardForSkillupObject> Skill_up_cards { get; set; }
        public URPairObject Ur_pair { get; set; }
        public int? Total_owners { get; set; }
        public int? Total_wishlist { get; set; }
        public int? Ranking_attribute { get; set; }
        public int? Ranking_rarity { get; set; }
        public int? Ranking_special { get; set; }
    }
}
