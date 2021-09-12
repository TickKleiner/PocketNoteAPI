using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketNoteAPI.Migrations
{
    public partial class New : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Device_PocketNoteAPIItems_PocketNoteAPIItemGoogleUserId",
                table: "Device");

            migrationBuilder.DropForeignKey(
                name: "FK_File_PocketNoteAPIItems_PocketNoteAPIItemGoogleUserId",
                table: "File");

            migrationBuilder.DropPrimaryKey(
                name: "PK_File",
                table: "File");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Device",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "AuthDate",
                table: "Device");

            migrationBuilder.DropColumn(
                name: "Ip",
                table: "Device");

            migrationBuilder.RenameTable(
                name: "File",
                newName: "Files");

            migrationBuilder.RenameTable(
                name: "Device",
                newName: "Devices");

            migrationBuilder.RenameColumn(
                name: "PocketNoteAPIItemGoogleUserId",
                table: "Files",
                newName: "GoogleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_File_PocketNoteAPIItemGoogleUserId",
                table: "Files",
                newName: "IX_Files_GoogleUserId");

            migrationBuilder.RenameColumn(
                name: "PocketNoteAPIItemGoogleUserId",
                table: "Devices",
                newName: "GoogleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Device_PocketNoteAPIItemGoogleUserId",
                table: "Devices",
                newName: "IX_Devices_GoogleUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Devices",
                table: "Devices",
                column: "DeviceId");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    GoogleUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => new { x.DeviceId, x.GoogleUserId });
                    table.ForeignKey(
                        name: "FK_Sessions_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_PocketNoteAPIItems_GoogleUserId",
                table: "Devices",
                column: "GoogleUserId",
                principalTable: "PocketNoteAPIItems",
                principalColumn: "GoogleUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_PocketNoteAPIItems_GoogleUserId",
                table: "Files",
                column: "GoogleUserId",
                principalTable: "PocketNoteAPIItems",
                principalColumn: "GoogleUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_PocketNoteAPIItems_GoogleUserId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_PocketNoteAPIItems_GoogleUserId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Devices",
                table: "Devices");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "File");

            migrationBuilder.RenameTable(
                name: "Devices",
                newName: "Device");

            migrationBuilder.RenameColumn(
                name: "GoogleUserId",
                table: "File",
                newName: "PocketNoteAPIItemGoogleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_GoogleUserId",
                table: "File",
                newName: "IX_File_PocketNoteAPIItemGoogleUserId");

            migrationBuilder.RenameColumn(
                name: "GoogleUserId",
                table: "Device",
                newName: "PocketNoteAPIItemGoogleUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_GoogleUserId",
                table: "Device",
                newName: "IX_Device_PocketNoteAPIItemGoogleUserId");

            migrationBuilder.AddColumn<string>(
                name: "AuthDate",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ip",
                table: "Device",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_File",
                table: "File",
                column: "FileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Device",
                table: "Device",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Device_PocketNoteAPIItems_PocketNoteAPIItemGoogleUserId",
                table: "Device",
                column: "PocketNoteAPIItemGoogleUserId",
                principalTable: "PocketNoteAPIItems",
                principalColumn: "GoogleUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_File_PocketNoteAPIItems_PocketNoteAPIItemGoogleUserId",
                table: "File",
                column: "PocketNoteAPIItemGoogleUserId",
                principalTable: "PocketNoteAPIItems",
                principalColumn: "GoogleUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
