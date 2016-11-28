namespace LearningMpaAbp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Person_Entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "AssignedPersonId", c => c.Int());
            CreateIndex("dbo.Tasks", "AssignedPersonId");
            AddForeignKey("dbo.Tasks", "AssignedPersonId", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "AssignedPersonId", "dbo.People");
            DropIndex("dbo.Tasks", new[] { "AssignedPersonId" });
            DropColumn("dbo.Tasks", "AssignedPersonId");
            DropTable("dbo.People");
        }
    }
}
