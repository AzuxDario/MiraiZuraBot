using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using MiraiZuraBot.Translators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.LanguageService
{
    class LanguageService
    {
        private Translator _translator;

        public LanguageService(Translator translator)
        {
            _translator = translator;
        }

        public Translator.Language GetServerLanguage(ulong serverId)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).FirstOrDefault();

                //If server is not present in database return default language.
                if (dbServer == null)
                {
                    return _translator.GetDefaultLanguage();
                }
                return dbServer.Language;
            }
        }

        public void ChangeLanguage(ulong serverId, Translator.Language language)
        {
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).FirstOrDefault();

                //If server is not present in database add it.
                if (dbServer == null)
                {
                    dbServer = new Server(serverId);
                    databaseContext.Add(dbServer);
                }
                dbServer.Language = language;
                databaseContext.SaveChanges();
            }
        }
    }
}
