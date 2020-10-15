namespace MyTherapyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMigrationWithGuid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DailyTherapies", "Guid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DailyTherapies", "Guid");
        }
    }
}
