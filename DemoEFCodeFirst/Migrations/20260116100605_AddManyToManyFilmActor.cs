using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DemoEFCodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddManyToManyFilmActor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FilmActors",
                columns: new[] { "ActorId", "FilmId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 3, 1 });
        }
    }
}
