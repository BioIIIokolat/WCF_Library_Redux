namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LatiTude", c => c.Double(nullable: false));
            AddColumn("dbo.Users", "LongiTude", c => c.Double(nullable: false));
            DropColumn("dbo.Filters", "Job");
            DropColumn("dbo.Filters", "Searchingfor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Filters", "Searchingfor", c => c.String());
            AddColumn("dbo.Filters", "Job", c => c.String());
            DropColumn("dbo.Users", "LongiTude");
            DropColumn("dbo.Users", "LatiTude");
        }
    }
}
