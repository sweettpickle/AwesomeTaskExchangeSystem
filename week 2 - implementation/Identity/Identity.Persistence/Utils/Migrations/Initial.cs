using FluentMigrator;

namespace Identity.Persistence.Utils.Migrations
{
    [Migration(20230626161500)]
    public class Initial : Migration
    {
        public override void Down()
        {
            Delete.Table("PersonalAccountInfo").InSchema("dbo");
            Delete.Table("AutiInfo").InSchema("dbo");
            Delete.Table("Parrot").InSchema("dbo");
            Delete.Table("Role").InSchema("dbo");
        }

        public override void Up()
        {
            Create.Table("Role")
                .InSchema("dbo")
                .WithColumn("Name").AsString(255).PrimaryKey("PK_Role")
                .WithColumn("Description").AsString(255).Nullable()
                ;

            Insert.IntoTable("Role").Row(new { Name = "Admin" });
            Insert.IntoTable("Role").Row(new { Name = "Manager" });
            Insert.IntoTable("Role").Row(new { Name = "Accountant" });
            Insert.IntoTable("Role").Row(new { Name = "Worker" });

            Create.Table("Parrot")
                .InSchema("dbo")
                .WithColumn("Id").AsInt64().PrimaryKey("PK_Parrot").Identity()
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentDateTimeOffset)
                .WithColumn("PublicId").AsString(255).NotNullable()

                .WithColumn("Nickname").AsString(255).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Address").AsString(255).Nullable()
                .WithColumn("Role").AsString(255).NotNullable()
                    .ForeignKey("FK_Parrot_Role", "Role", "Name")
                ;

            Create.Table("AuthInfo")
                .InSchema("dbo")
                .WithColumn("Id").AsInt64().PrimaryKey("PK_AuthInfo")
                    .ForeignKey("FK_AuthInfo_Parrot", "Parrot", "Id")
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentDateTimeOffset)
                .WithColumn("PublicId").AsString(255).NotNullable()

                .WithColumn("Login").AsString(255).NotNullable()
                .WithColumn("FavoriteTreatHash").AsCustom("varbinary(max)").NotNullable()
                ;

            Create.Table("PersonalAccountInfo")
                .InSchema("dbo")
                .WithColumn("Id").AsInt64().PrimaryKey("PK_PersonalAccountInfo")
                    .ForeignKey("FK_PersonalAccountInfo_Parrot", "Parrot", "Id")
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentDateTimeOffset)
                .WithColumn("PublicId").AsString(255).NotNullable()

                .WithColumn("AccountNumber").AsString(255).NotNullable()
                .WithColumn("AccountNickname").AsString(255).NotNullable()
                ;
        }
    }
}
