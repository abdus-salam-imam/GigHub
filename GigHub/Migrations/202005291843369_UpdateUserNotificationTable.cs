namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserNotificationTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            CreateIndex("dbo.UserNotifications", "NotificationId");
            AddForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.UserNotifications", "Notification");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserNotifications", "Notification", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            AddForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
