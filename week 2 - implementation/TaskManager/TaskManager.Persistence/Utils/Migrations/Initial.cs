using FluentMigrator;

namespace TaskManager.Persistence.Utils.Migrations
{
    [Migration(20230626161500)]
    public class Initial : Migration
    {
        public override void Up()
        {
            Create.Table("TaskStatus")
                .InSchema("dbo")
                .WithColumn("Name").AsString(255).PrimaryKey("PK_TaskStatus")
                .WithColumn("Description").AsString(255).Nullable()
                ;

            Insert.IntoTable("TaskStatus").Row(new { Name = "Opened" });
            Insert.IntoTable("TaskStatus").Row(new { Name = "Completed" });
            Insert.IntoTable("TaskStatus").Row(new { Name = "Canceled" });

            Create.Table("Parrot")
                .InSchema("dbo")
                .WithColumn("Id").AsInt64().PrimaryKey("PK_Parrot").Identity()
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentDateTimeOffset)
                .WithColumn("PublicId").AsString(255).NotNullable()
                .WithColumn("Role").AsString(255).NotNullable()
                ;

            Create.Table("Task")
                .InSchema("dbo")
                .WithColumn("Id").AsInt64().PrimaryKey("PK_Task").Identity()
                .WithColumn("CreatedAt").AsDateTimeOffset().NotNullable().WithDefault(SystemMethods.CurrentDateTimeOffset)
                .WithColumn("PublicId").AsString(255).NotNullable()

                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Description").AsString(255).NotNullable()
                .WithColumn("WriteOffAmount").AsDecimal().NotNullable()
                .WithColumn("AccrualAmount").AsDecimal().NotNullable()
                .WithColumn("Status").AsString(255).NotNullable()
                    .ForeignKey("FK_Task_TaskStatus", "TaskStatus", "Name")
                .WithColumn("ParrotId").AsInt64().NotNullable()
                    .ForeignKey("FK_Task_Parrot", "Parrot", "Id")
                ;
        }

        public override void Down()
        {
            Delete.Table("Task").InSchema("dbo");
            Delete.Table("Parrot").InSchema("dbo");
            Delete.Table("TaskStatus").InSchema("dbo");
        }
    }
}
