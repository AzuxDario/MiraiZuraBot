using Microsoft.EntityFrameworkCore;
using MiraiZuraBot.Database.Models.DynamicDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiraiZuraBot.Database
{
    class DynamicDBContext : DbContext
    {
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<Emoji> Emojis { get; set; }
        public virtual DbSet<BirthdayChannel> BirthdayChannels { get; set; }
        public virtual DbSet<AssignRole> AssignRoles { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Birthday> Birthdays { get; set; }
        public virtual DbSet<PostedBirthday> PostedBirthdays { get; set; }
        public virtual List<BirthdayRole> BirthdayRoles { get; set; }
        public DynamicDBContext() : base(GetOptions("Data Source=DynamicDatabase.sqlite"))
        {

        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqliteDbContextOptionsBuilderExtensions.UseSqlite(new DbContextOptionsBuilder(), connectionString).Options;
        }

    }
}
