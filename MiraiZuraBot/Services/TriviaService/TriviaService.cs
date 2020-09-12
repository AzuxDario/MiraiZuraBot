using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB.Trivia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.TriviaService
{
    class TriviaService
    {
        public List<string> GetTopics()
        {
            List<string> topics = new List<string>();
            using (var databaseContext = new DynamicDBContext())
            {
                return databaseContext.TriviaTopics.Select(p => p.Name).ToList();
            }
        }
    }
}
