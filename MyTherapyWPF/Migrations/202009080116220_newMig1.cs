namespace MyTherapyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMig1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DailyTherapies", "Dose", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DailyTherapies", "Dose", c => c.Single(nullable: false));
        }
    }
}
