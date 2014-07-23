namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Project : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 32),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "ProjectId", c => c.Long());
            CreateIndex("dbo.AspNetUsers", "ProjectId");
            AddForeignKey("dbo.AspNetUsers", "ProjectId", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ProjectId", "dbo.Projects");
            DropIndex("dbo.AspNetUsers", new[] { "ProjectId" });
            DropColumn("dbo.AspNetUsers", "ProjectId");
            DropTable("dbo.Projects");
        }
    }
}
