namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Datings", "User_UserId", "dbo.Users");
            DropIndex("dbo.Datings", new[] { "User_UserId" });
            CreateTable(
                "dbo.BlackLists",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        User_UserId = c.Int(),
                        UserEnemy_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.Users", t => t.UserEnemy_UserId)
                .Index(t => t.User_UserId)
                .Index(t => t.UserEnemy_UserId);
            
            CreateTable(
                "dbo.Hobbies",
                c => new
                    {
                        HobbieID = c.Int(nullable: false, identity: true),
                        Hobbie = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.HobbieID)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        Photo = c.String(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PhotoID)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            AddColumn("dbo.Users", "Education", c => c.String());
            DropColumn("dbo.Datings", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Datings", "User_UserId", c => c.Int());
            DropForeignKey("dbo.Photos", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Hobbies", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.BlackLists", "UserEnemy_UserId", "dbo.Users");
            DropForeignKey("dbo.BlackLists", "User_UserId", "dbo.Users");
            DropIndex("dbo.Photos", new[] { "User_UserId" });
            DropIndex("dbo.Hobbies", new[] { "User_UserId" });
            DropIndex("dbo.BlackLists", new[] { "UserEnemy_UserId" });
            DropIndex("dbo.BlackLists", new[] { "User_UserId" });
            DropColumn("dbo.Users", "Education");
            DropTable("dbo.Photos");
            DropTable("dbo.Hobbies");
            DropTable("dbo.BlackLists");
            CreateIndex("dbo.Datings", "User_UserId");
            AddForeignKey("dbo.Datings", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
