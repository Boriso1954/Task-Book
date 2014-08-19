namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskCompletedDateNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "CompletedDate", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "CompletedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
