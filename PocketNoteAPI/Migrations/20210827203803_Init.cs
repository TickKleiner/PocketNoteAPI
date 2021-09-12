using Microsoft.EntityFrameworkCore.Migrations;

namespace PocketNoteAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PocketNoteAPIItems",
                columns: table => new
                {
                    GoogleUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pin = table.Column<short>(type: "smallint", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PocketNoteAPIItems", x => x.GoogleUserId);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AuthDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PocketNoteAPIItemGoogleUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Device_PocketNoteAPIItems_PocketNoteAPIItemGoogleUserId",
                        column: x => x.PocketNoteAPIItemGoogleUserId,
                        principalTable: "PocketNoteAPIItems",
                        principalColumn: "GoogleUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EncryptedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PocketNoteAPIItemGoogleUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_File_PocketNoteAPIItems_PocketNoteAPIItemGoogleUserId",
                        column: x => x.PocketNoteAPIItemGoogleUserId,
                        principalTable: "PocketNoteAPIItems",
                        principalColumn: "GoogleUserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Device_PocketNoteAPIItemGoogleUserId",
                table: "Device",
                column: "PocketNoteAPIItemGoogleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_File_PocketNoteAPIItemGoogleUserId",
                table: "File",
                column: "PocketNoteAPIItemGoogleUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "PocketNoteAPIItems");
        }
    }
}
