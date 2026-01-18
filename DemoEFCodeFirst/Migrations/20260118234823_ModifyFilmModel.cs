using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoEFCodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class ModifyFilmModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "La durée du film en minutes");

            migrationBuilder.AddColumn<string>(
                name: "CharacterFirstname",
                table: "FilmActors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CharacterLastname",
                table: "FilmActors",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 1, 1 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Jake", "Sulley" });

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 1, 2 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Jake", "Sulley" });

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 1, 3 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Jake", "Sulley" });

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 2, 1 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Neytiri", null });

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 2, 2 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Neytiri", null });

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 2, 3 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Neytiri", null });

            migrationBuilder.UpdateData(
                table: "FilmActors",
                keyColumns: new[] { "ActorId", "FilmId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "CharacterFirstname", "CharacterLastname" },
                values: new object[] { "Grace", "Augustine" });

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 1,
                column: "Duration",
                value: 162);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 2,
                column: "Duration",
                value: 192);

            migrationBuilder.UpdateData(
                table: "Films",
                keyColumn: "Id",
                keyValue: 3,
                column: "Duration",
                value: 210);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Film_Duration_Positive",
                table: "Films",
                sql: "Duration > 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Film_Duration_Positive",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "CharacterFirstname",
                table: "FilmActors");

            migrationBuilder.DropColumn(
                name: "CharacterLastname",
                table: "FilmActors");
        }
    }
}
