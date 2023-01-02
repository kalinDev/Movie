using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPosterURI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InTheaters",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "PosterUri",
                table: "Movies",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterUri",
                table: "Movies");

            migrationBuilder.AddColumn<bool>(
                name: "InTheaters",
                table: "Movies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
