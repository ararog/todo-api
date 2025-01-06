using FluentMigrator;

[Migration(1)]
public class CreateUserTable : Migration
{
  public override void Up()
  {
    Create.Table("todoitem")
      .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
      .WithColumn("title").AsString(255).NotNullable()
      .WithColumn("description").AsString(1000).NotNullable()
      .WithColumn("completed").AsBoolean().Nullable()
      .WithColumn("createdat").AsDateTime().WithDefaultValue(DateTime.Now)
      .WithColumn("completedat").AsDateTime().Nullable()
      .WithColumn("userid").AsString(50).NotNullable();
  }

  public override void Down()
  {
    Delete.Table("todoitem");
  }
}