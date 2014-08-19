namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectIdInTasksTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ProjectId", c => c.Long(nullable: false));
            CreateIndex("dbo.Tasks", "ProjectId");
            AddForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropColumn("dbo.Tasks", "ProjectId");
        }
    }
}
