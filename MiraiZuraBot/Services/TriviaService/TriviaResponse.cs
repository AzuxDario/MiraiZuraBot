using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Services.TriviaService
{
    class TriviaResponse
    {
        public string Content { get; }
        public string Source { get; }

        public TriviaResponse(string content, string source)
        {
            Content = content;
            Source = source;
        }
    }
}
