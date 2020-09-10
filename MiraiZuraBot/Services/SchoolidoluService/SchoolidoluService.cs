using MiraiZuraBot.Containers.Schoolidolu;
using MiraiZuraBot.Containers.Schoolidolu.Cards;
using MiraiZuraBot.Containers.Schoolidolu.Event;
using MiraiZuraBot.Containers.Schoolidolu.Idols;
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

        public SchoolidoluResponse<PaginatedResponse<CardObject>> GetCard(Dictionary<string, string> options)
        {
            var client = new HttpClient();
            PaginatedResponse<CardObject> cardObject;

            var response = client.GetAsync(apiBase + "cards/?" + CombineGetParameters(options)).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cardObject = JsonConvert.DeserializeObject<PaginatedResponse<CardObject>>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<PaginatedResponse<CardObject>>(cardObject, response.StatusCode);
            }

            return new SchoolidoluResponse<PaginatedResponse<CardObject>>(null, response.StatusCode);
        }

        public SchoolidoluResponse<IdolObject> GetIdolByName(string name)
        {
            var client = new HttpClient();
            IdolObject idolObject;

            var response = client.GetAsync(apiBase + "idols/" + name + "/").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                idolObject = JsonConvert.DeserializeObject<IdolObject>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<IdolObject>(idolObject, response.StatusCode);
            }

            return new SchoolidoluResponse<IdolObject>(null, response.StatusCode);
        }

        public SchoolidoluResponse<PaginatedResponse<IdolObject>> GetIdol(Dictionary<string, string> options)
        {
            var client = new HttpClient();
            PaginatedResponse<IdolObject> idolObject;

            var response = client.GetAsync(apiBase + "idols/?" + CombineGetParameters(options)).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                idolObject = JsonConvert.DeserializeObject<PaginatedResponse<IdolObject>>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<PaginatedResponse<IdolObject>>(idolObject, response.StatusCode);
            }

            return new SchoolidoluResponse<PaginatedResponse<IdolObject>>(null, response.StatusCode);
        }

        public SchoolidoluResponse<PaginatedResponse<EventObject>> GetEvent(Dictionary<string, string> options)
        {
            var client = new HttpClient();
            PaginatedResponse<EventObject> eventObject;

            var response = client.GetAsync(apiBase + "events/?" + CombineGetParameters(options)).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                eventObject = JsonConvert.DeserializeObject<PaginatedResponse<EventObject>>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<PaginatedResponse<EventObject>>(eventObject, response.StatusCode);
            }

            return new SchoolidoluResponse<PaginatedResponse<EventObject>>(null, response.StatusCode);
        }

        public SchoolidoluResponse<EventObject> GetEventByName(string name)
        {
            var client = new HttpClient();
            EventObject eventObject;

            var response = client.GetAsync(apiBase + "events/" + name + "/").Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                eventObject = JsonConvert.DeserializeObject<EventObject>(response.Content.ReadAsStringAsync().Result);
                return new SchoolidoluResponse<EventObject>(eventObject, response.StatusCode);
            }

            return new SchoolidoluResponse<EventObject>(null, response.StatusCode);
        }

        private string CombineGetParameters(Dictionary<string, string> options)
        {
            StringBuilder parameters = new StringBuilder();
            foreach (var elem in options)
            {
                parameters.Append(elem.Key).Append("=").Append(elem.Value).Append("&");
            }
            return parameters.ToString();
        }
    }
}
