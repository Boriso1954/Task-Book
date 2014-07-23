namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Task : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 64),
                        Description = c.String(nullable: false, maxLength: 512),
                        CreatedDate = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedById = c.String(maxLength: 128),
                        DueDate = c.DateTimeOffset(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                        AssigneToId = c.String(maxLength: 128),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssigneToId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .Index(t => t.CreatedById)
                .Index(t => t.AssigneToId);
            
            AddColumn("dbo.Projects", "CreatedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "CreatedById", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tasks", "AssigneToId", "dbo.AspNetUsers");
            DropIndex("dbo.Tasks", new[] { "AssigneToId" });
            DropIndex("dbo.Tasks", new[] { "CreatedById" });
            DropColumn("dbo.Projects", "CreatedDate");
            DropTable("dbo.Tasks");
        }
    }
}
