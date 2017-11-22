using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using Abp.Zero.EntityFramework;
using LearningMpaAbp.Authorization.Roles;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Users;
using MySql.Data.Entity;

namespace LearningMpaAbp.EntityFramework
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class LearningMpaAbpDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */

        public LearningMpaAbpDbContext()
            : base("Default")
        {
            //调试时输出EF执行sql到VS的输出窗口
            Database.Log = sql => Debug.Write(sql);
            //输出EF执行sql到log文件
            //Database.Log = sql => Logger.Log(LogSeverity.Info, sql);
        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in LearningMpaAbpDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of LearningMpaAbpDbContext since ABP automatically handles it.
         */

        public LearningMpaAbpDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //调试时输出EF执行sql到VS的输出窗口
            Database.Log = sql => Debug.Write(sql);
            //输出EF执行sql到log文件
            //Database.Log = sql => Logger.Log(LogSeverity.Info, sql);
        }

        //This constructor is used in tests
        public LearningMpaAbpDbContext(DbConnection connection)
            : base(connection, true)
        {
            //调试时输出EF执行sql到VS的输出窗口
            Database.Log = sql => Debug.Write(sql);
            //输出EF执行sql到log文件
            //Database.Log = sql => Logger.Log(LogSeverity.Info, sql);
        }

        //TODO: Define an IDbSet for your Entities...

        public IDbSet<Task> Tasks { get; set; }
    }
}