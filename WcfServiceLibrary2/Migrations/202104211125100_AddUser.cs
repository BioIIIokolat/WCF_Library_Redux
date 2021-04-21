namespace WcfServiceLibrary2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Datings",
                c => new
                    {
                        DatingId = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        IsAcсeptedByFirst = c.Boolean(nullable: false),
                        IsAcсeptedBySecond = c.Boolean(nullable: false),
                        Adress = c.String(),
                        Typeofplace = c.String(),
                        User_UserId = c.Int(),
                        female_UserId = c.Int(),
                        male_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.DatingId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .ForeignKey("dbo.Users", t => t.female_UserId)
                .ForeignKey("dbo.Users", t => t.male_UserId)
                .Index(t => t.User_UserId)
                .Index(t => t.female_UserId)
                .Index(t => t.male_UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Password = c.String(),
                        Avatarka = c.String(),
                        Description = c.String(),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        ColorHairCut = c.String(),
                        ColorEye = c.String(),
                        Faith = c.String(),
                        Job = c.String(),
                        Gender = c.String(),
                        Orientation = c.String(),
                        Birthday = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.ChatItemUsers",
                c => new
                    {
                        ChatItemId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChatItemId, t.UserId })
                .ForeignKey("dbo.ChatItems", t => t.ChatItemId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ChatItemId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ChatItems",
                c => new
                    {
                        ChatItemId = c.Int(nullable: false, identity: true),
                        ImagePath = c.String(),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.ChatItemId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        TimeSending = c.DateTime(nullable: false),
                        IsRecieved = c.Boolean(nullable: false),
                        Mes = c.String(),
                        ImagePath = c.String(),
                        chatItem_ChatItemId = c.Int(),
                        user_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.ChatItems", t => t.chatItem_ChatItemId)
                .ForeignKey("dbo.Users", t => t.user_UserId)
                .Index(t => t.chatItem_ChatItemId)
                .Index(t => t.user_UserId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikeID = c.Int(nullable: false, identity: true),
                        Date_Like = c.DateTime(nullable: false),
                        User_Liked_ID_UserId = c.Int(),
                        User_Who_Liked_ID_UserId = c.Int(),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.LikeID)
                .ForeignKey("dbo.Users", t => t.User_Liked_ID_UserId)
                .ForeignKey("dbo.Users", t => t.User_Who_Liked_ID_UserId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_Liked_ID_UserId)
                .Index(t => t.User_Who_Liked_ID_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Filters",
                c => new
                    {
                        FiltersId = c.Int(nullable: false, identity: true),
                        MaxAge = c.Int(nullable: false),
                        MinAge = c.Int(nullable: false),
                        ColorHair = c.Int(nullable: false),
                        ColorEye = c.String(),
                        Height = c.Int(nullable: false),
                        Job = c.String(),
                        MaxDistance = c.Int(nullable: false),
                        Searchingfor = c.String(),
                        UserId_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.FiltersId)
                .ForeignKey("dbo.Users", t => t.UserId_UserId)
                .Index(t => t.UserId_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Filters", "UserId_UserId", "dbo.Users");
            DropForeignKey("dbo.Datings", "male_UserId", "dbo.Users");
            DropForeignKey("dbo.Datings", "female_UserId", "dbo.Users");
            DropForeignKey("dbo.Likes", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Likes", "User_Who_Liked_ID_UserId", "dbo.Users");
            DropForeignKey("dbo.Likes", "User_Liked_ID_UserId", "dbo.Users");
            DropForeignKey("dbo.Datings", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.ChatItemUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.ChatItemUsers", "ChatItemId", "dbo.ChatItems");
            DropForeignKey("dbo.Messages", "user_UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "chatItem_ChatItemId", "dbo.ChatItems");
            DropIndex("dbo.Filters", new[] { "UserId_UserId" });
            DropIndex("dbo.Likes", new[] { "User_UserId" });
            DropIndex("dbo.Likes", new[] { "User_Who_Liked_ID_UserId" });
            DropIndex("dbo.Likes", new[] { "User_Liked_ID_UserId" });
            DropIndex("dbo.Messages", new[] { "user_UserId" });
            DropIndex("dbo.Messages", new[] { "chatItem_ChatItemId" });
            DropIndex("dbo.ChatItemUsers", new[] { "UserId" });
            DropIndex("dbo.ChatItemUsers", new[] { "ChatItemId" });
            DropIndex("dbo.Datings", new[] { "male_UserId" });
            DropIndex("dbo.Datings", new[] { "female_UserId" });
            DropIndex("dbo.Datings", new[] { "User_UserId" });
            DropTable("dbo.Filters");
            DropTable("dbo.Likes");
            DropTable("dbo.Messages");
            DropTable("dbo.ChatItems");
            DropTable("dbo.ChatItemUsers");
            DropTable("dbo.Users");
            DropTable("dbo.Datings");
        }
    }
}
