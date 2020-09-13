using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB.Trivia
{
    class TriviaTopic
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual List<TriviaTopicContent> TriviaTopicContents { get; set; }
    }
}
