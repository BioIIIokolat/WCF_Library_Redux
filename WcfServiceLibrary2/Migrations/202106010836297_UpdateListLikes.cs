namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateListLikes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Filters", "ColorHair", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Filters", "ColorHair", c => c.String());
        }
    }
}
