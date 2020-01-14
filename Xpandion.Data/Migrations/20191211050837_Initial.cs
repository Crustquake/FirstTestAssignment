using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xpandion.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataStructures",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataStructures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    StructureId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ProcessingDate = table.Column<DateTime>(nullable: false),
                    NumberOfColumns = table.Column<int>(nullable: false),
                    NumberOfRows = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Files_DataStructures_StructureId",
                        column: x => x.StructureId,
                        principalTable: "DataStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    StructureId = table.Column<int>(nullable: false),
                    CsvFileEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Columns_Files_CsvFileEntityId",
                        column: x => x.CsvFileEntityId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Columns_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Columns_DataStructures_StructureId",
                        column: x => x.StructureId,
                        principalTable: "DataStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Columns_CsvFileEntityId",
                table: "Columns",
                column: "CsvFileEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_CustomerId",
                table: "Columns",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_StructureId",
                table: "Columns",
                column: "StructureId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CustomerId",
                table: "Files",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_StructureId",
                table: "Files",
                column: "StructureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Columns");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "DataStructures");
        }
    }
}
