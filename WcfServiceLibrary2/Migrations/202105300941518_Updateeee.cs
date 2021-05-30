namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updateeee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatItems", "ImagePath", c => c.String());
            AddColumn("dbo.Filters", "Height", c => c.Int(nullable: false));
            AlterColumn("dbo.Filters", "ColorHair", c => c.Int(nullable: false));
            DropColumn("dbo.Filters", "MaxHeight");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Filters", "MaxHeight", c => c.Int(nullable: false));
            AlterColumn("dbo.Filters", "ColorHair", c => c.String());
            DropColumn("dbo.Filters", "Height");
            DropColumn("dbo.ChatItems", "ImagePath");
        }
    }
}
