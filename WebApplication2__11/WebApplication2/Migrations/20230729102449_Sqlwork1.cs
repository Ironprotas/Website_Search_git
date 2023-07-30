using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Sqlwork1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "SearchItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "SearchItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StargazersCount",
                table: "SearchItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WatchersCount",
                table: "SearchItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "SearchItems");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "SearchItems");

            migrationBuilder.DropColumn(
                name: "StargazersCount",
                table: "SearchItems");

            migrationBuilder.DropColumn(
                name: "WatchersCount",
                table: "SearchItems");
        }
    }
}
