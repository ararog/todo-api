using FluentMigrator;

[Migration(1)]
public class CreateUserTable : Migration
{
  public override void Up()
  {
    Create.Table("User")
      .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
      .WithColumn("Email").AsString(255).NotNullable()
      .WithColumn("Password").AsString(255).NotNullable()
      .WithColumn("Auth0Id").AsString(10).NotNullable()
      .WithColumn("PermissionLevel").AsString(20).NotNullable();

    Create.Table("TodoItem")
      .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
      .WithColumn("Title").AsString(255).NotNullable()
      .WithColumn("Description").AsString(1000).NotNullable()
      .WithColumn("Completed").AsBoolean().Nullable()
      .WithColumn("CreatedAt").AsDateTime().WithDefaultValue(DateTime.Now)
      .WithColumn("CompletedAt").AsDateTime().Nullable();
  }

  public override void Down()
  {
    Delete.Table("TodoItem");
    Delete.Table("User");
  }
}