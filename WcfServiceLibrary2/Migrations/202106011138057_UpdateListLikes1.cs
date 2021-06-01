namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateListLikes1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Filters", "ColorHair", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Filters", "ColorHair", c => c.Int(nullable: false));
        }
    }
}
