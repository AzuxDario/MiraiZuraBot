using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MiraiZuraBot.Services.SchoolidoluService
{
    class SchoolidoluService
    {
        private readonly string apiBase = "http://schoolido.lu/api/";

        public SchoolidoluResponse<CardObject> GetCardById(string id)
        {
            var client = new HttpClient();
            CardObject cardObject;

            var response = client.GetAsync(apiBase + "cards/" + id + "/").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cardObject = JsonConvert.DeserializeObject<CardObject>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<CardObject>(cardObject, response.StatusCode);
            }

            return new SchoolidoluResponse<CardObject>(null, response.StatusCode);
        }

        public SchoolidoluResponse<PaginatedResponse<CardObject>> GetRandomCard(string name)
        {
            var client = new HttpClient();
            PaginatedResponse<CardObject> cardObject;

            var response = client.GetAsync(apiBase + "cards/?name=" + name + "&ordering=random&page_size=1").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cardObject = JsonConvert.DeserializeObject<PaginatedResponse<CardObject>>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<PaginatedResponse<CardObject>>(cardObject, response.StatusCode);
            }

            return new SchoolidoluResponse<PaginatedResponse<CardObject>>(null, response.StatusCode);
        }
    }
}
