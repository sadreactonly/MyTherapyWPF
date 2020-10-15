namespace MyTherapyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyTherapies", "Dose", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyTherapies", "Dose", c => c.Double(nullable: false));
        }
    }
}
