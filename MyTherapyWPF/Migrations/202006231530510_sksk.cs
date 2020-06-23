namespace MyTherapyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sksk : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DailyTherapies", "LastModified");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DailyTherapies", "LastModified", c => c.DateTime(nullable: false));
        }
    }
}
