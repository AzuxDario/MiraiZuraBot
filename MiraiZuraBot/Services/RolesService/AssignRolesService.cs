using Microsoft.EntityFrameworkCore;
using MiraiZuraBot.Database;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiraiZuraBot.Services.RolesService
{
    class AssignRolesService
    {
        public List<ulong> GetRoles(ulong serverId)
        {
            List<ulong> roles = new List<ulong>();
            using (var databaseContext = new DynamicDBContext())
            {
                Server dbServer = GetServerFromDatabase(databaseContext, serverId);

                return dbServer.AssignRoles.Select(p => ulong.Parse(p.RoleID)).ToList();
            }
        }

        private Server GetServerFromDatabase(DynamicDBContext databaseContext, ulong serverId)
        {
            Server dbServer = databaseContext.Servers.Where(p => p.ServerID == serverId.ToString()).Include(p => p.AssignRoles).FirstOrDefault();

            //If server is not present in database add it.
            if (dbServer == null)
            {
                dbServer = new Server(serverId)
                {
                    AssignRoles = new List<AssignRole>()
                };
                databaseContext.Add(dbServer);
                databaseContext.SaveChanges();
            }
            return dbServer;
        }
    }
}
