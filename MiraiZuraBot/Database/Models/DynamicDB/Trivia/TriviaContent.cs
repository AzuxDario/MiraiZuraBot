using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB.Trivia
{
    class TriviaContent
    {
        public int ID { get; set; }
        public string Content { get; set; }

        public virtual List<TriviaTopicContent> TriviaTopicContents { get; set; }
    }
}
