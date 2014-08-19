namespace TaskBook.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskCompletedDate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Tasks", name: "AssigneToId", newName: "AssignedToId");
            RenameIndex(table: "dbo.Tasks", name: "IX_AssigneToId", newName: "IX_AssignedToId");
            AddColumn("dbo.Tasks", "CompletedDate", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "CompletedDate");
            RenameIndex(table: "dbo.Tasks", name: "IX_AssignedToId", newName: "IX_AssigneToId");
            RenameColumn(table: "dbo.Tasks", name: "AssignedToId", newName: "AssigneToId");
        }
    }
}
