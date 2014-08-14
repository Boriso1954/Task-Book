namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectNameUnique : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Projects", "Title", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", new[] { "Title" });
        }
    }
}
