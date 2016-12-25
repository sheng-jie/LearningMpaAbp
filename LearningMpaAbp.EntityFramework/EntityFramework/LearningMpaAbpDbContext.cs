using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using LearningMpaAbp.Authorization.Roles;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Users;

namespace LearningMpaAbp.EntityFramework
{
    public class LearningMpaAbpDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        //TODO: Define an IDbSet for your Entities...

        public IDbSet<Task> Tasks { get; set; }


        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public LearningMpaAbpDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in LearningMpaAbpDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of LearningMpaAbpDbContext since ABP automatically handles it.
         */
        public LearningMpaAbpDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        //This constructor is used in tests
        public LearningMpaAbpDbContext(DbConnection connection)
            : base(connection, true)
        {

        }
    }
}
