using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeMaster.Migrations
{
    /// <inheritdoc />
    public partial class ConferenceRoomTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "ConferenceRooms",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "ConferenceRooms");
        }
    }
}
