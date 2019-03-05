namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedUsers : DbMigration
    {
        public override void Up()
        {
            Sql(
                @"INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'7c3ddd55-08db-4092-b7a1-2e0f86fc4de2', N'admin@longwoodlibrary.org', 0, N'AIa2hDHFmalkYaB7wZeA8bglViPfHgBm1X4qPw7HhE+iUCphflXrq8i6thyrazGEjA==', N'd9b58c4f-a176-4609-9480-fe6786ab267f', NULL, 0, 0, NULL, 1, 0, N'admin@longwoodlibrary.org')
                  INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'95bca652-2cc3-451f-80dc-e56a95349545', N'guest@longwoodlibrary.org', 0, N'AGB5GDlPnIxs8tG53VaxCKZ6bTs8wnGO9P+K70l7qP7QxbrQQkYzjhWIKGH/IROLGw==', N'b8a694c6-55c1-479b-85c4-02b98ad399e2', NULL, 0, 0, NULL, 1, 0, N'guest@longwoodlibrary.org')
                  INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3dea80ab-f1d9-4c5f-9128-5d6aa8a17257', N'CanManagePatrons')                  
                  INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7c3ddd55-08db-4092-b7a1-2e0f86fc4de2', N'3dea80ab-f1d9-4c5f-9128-5d6aa8a17257')");
        }
        
        public override void Down()
        {
        }
    }
}
