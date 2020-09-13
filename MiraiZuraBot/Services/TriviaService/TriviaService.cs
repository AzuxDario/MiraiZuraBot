using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB.Trivia;
using MiraiZuraBot.Helpers.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.TriviaService
{
    class TriviaService
    {
        private Random rand;
        public TriviaService()
        {
            rand = new Random();
        }
        public List<string> GetTopics()
        {
            List<string> topics = new List<string>();
            using (var databaseContext = new DynamicDBContext())
            {
                return databaseContext.TriviaTopics.Select(p => p.Name).ToList();
            }
        }

        public TriviaResponse GetTrivia(string topic = null)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                if (topic == null)
                {
                    var trivia = databaseContext.TriviaContents.OrderBy(r => Guid.NewGuid()).First();
                    return new TriviaResponse(trivia.Content, trivia.Source);
                }
                else
                {
                    var trivia = databaseContext.TriviaTopicContents.Select(p => p.Content).Where(q => q.TriviaTopicContents.Any(r => r.Topic.Name == topic)).OrderBy(r => Guid.NewGuid()).First();
                    return new TriviaResponse(trivia.Content, trivia.Source);
                }
            }
        }
    }
}
