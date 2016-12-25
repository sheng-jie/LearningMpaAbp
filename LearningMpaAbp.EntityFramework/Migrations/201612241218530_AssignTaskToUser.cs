namespace LearningMpaAbp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssignTaskToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "AssignedPersonId", c => c.Long());
            CreateIndex("dbo.Tasks", "AssignedPersonId");
            AddForeignKey("dbo.Tasks", "AssignedPersonId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "AssignedPersonId", "dbo.AbpUsers");
            DropIndex("dbo.Tasks", new[] { "AssignedPersonId" });
            DropColumn("dbo.Tasks", "AssignedPersonId");
        }
    }
}
