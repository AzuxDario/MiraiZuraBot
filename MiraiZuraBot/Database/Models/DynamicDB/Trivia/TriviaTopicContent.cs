using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database.Models.DynamicDB.Trivia
{
    class TriviaTopicContent
    {
        public int ID { get; set; }

        public int TopicID { get; set; }
        public virtual TriviaTopic Topic { get; set; }

        public int ContentID { get; set; }
        public virtual TriviaContent Content { get; set; }
    }
}
