namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFilterInTheTableUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Filters_FiltersId", "dbo.Filters");
            DropIndex("dbo.Users", new[] { "Filters_FiltersId" });
            DropColumn("dbo.Users", "Filters_FiltersId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Filters_FiltersId", c => c.Int());
            CreateIndex("dbo.Users", "Filters_FiltersId");
            AddForeignKey("dbo.Users", "Filters_FiltersId", "dbo.Filters", "FiltersId");
        }
    }
}
