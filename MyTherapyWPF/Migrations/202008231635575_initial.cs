namespace MyTherapyWPF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorAppointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        INR = c.Double(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DailyTherapies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Dose = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsTaken = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DailyTherapies");
            DropTable("dbo.DoctorAppointments");
        }
    }
}
