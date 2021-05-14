namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewD2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlackLists", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.BlackLists", "UserEnemy_UserId", "dbo.Users");
            DropForeignKey("dbo.Hobbies", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Photos", "User_UserId", "dbo.Users");
            DropIndex("dbo.BlackLists", new[] { "User_UserId" });
            DropIndex("dbo.BlackLists", new[] { "UserEnemy_UserId" });
            DropIndex("dbo.Hobbies", new[] { "User_UserId" });
            DropIndex("dbo.Photos", new[] { "User_UserId" });
            AddColumn("dbo.BlackLists", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.BlackLists", "UserEnemyID", c => c.Int(nullable: false));
            AddColumn("dbo.Hobbies", "UserID", c => c.Int(nullable: false));
            AddColumn("dbo.Photos", "UserID", c => c.Int(nullable: false));
            DropColumn("dbo.BlackLists", "User_UserId");
            DropColumn("dbo.BlackLists", "UserEnemy_UserId");
            DropColumn("dbo.Hobbies", "User_UserId");
            DropColumn("dbo.Photos", "User_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Photos", "User_UserId", c => c.Int());
            AddColumn("dbo.Hobbies", "User_UserId", c => c.Int());
            AddColumn("dbo.BlackLists", "UserEnemy_UserId", c => c.Int());
            AddColumn("dbo.BlackLists", "User_UserId", c => c.Int());
            DropColumn("dbo.Photos", "UserID");
            DropColumn("dbo.Hobbies", "UserID");
            DropColumn("dbo.BlackLists", "UserEnemyID");
            DropColumn("dbo.BlackLists", "UserID");
            CreateIndex("dbo.Photos", "User_UserId");
            CreateIndex("dbo.Hobbies", "User_UserId");
            CreateIndex("dbo.BlackLists", "UserEnemy_UserId");
            CreateIndex("dbo.BlackLists", "User_UserId");
            AddForeignKey("dbo.Photos", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Hobbies", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.BlackLists", "UserEnemy_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.BlackLists", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
