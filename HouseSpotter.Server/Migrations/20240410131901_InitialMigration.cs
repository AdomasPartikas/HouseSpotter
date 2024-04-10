using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace HouseSpotter.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Housing",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AnketosKodas = table.Column<string>(type: "longtext", nullable: true),
                    Nuotrauka = table.Column<string>(type: "longtext", nullable: true),
                    Link = table.Column<string>(type: "longtext", nullable: true),
                    BustoTipas = table.Column<string>(type: "longtext", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    Kaina = table.Column<double>(type: "double", nullable: true),
                    Gyvenviete = table.Column<string>(type: "longtext", nullable: true),
                    Gatve = table.Column<string>(type: "longtext", nullable: true),
                    KainaMen = table.Column<double>(type: "double", nullable: true),
                    NamoNumeris = table.Column<string>(type: "longtext", nullable: true),
                    ButoNumeris = table.Column<string>(type: "longtext", nullable: true),
                    KambariuSk = table.Column<int>(type: "int", nullable: true),
                    Plotas = table.Column<double>(type: "double", nullable: true),
                    SklypoPlotas = table.Column<string>(type: "longtext", nullable: true),
                    Aukstas = table.Column<int>(type: "int", nullable: true),
                    AukstuSk = table.Column<int>(type: "int", nullable: true),
                    Metai = table.Column<int>(type: "int", nullable: true),
                    Irengimas = table.Column<string>(type: "longtext", nullable: true),
                    NamoTipas = table.Column<string>(type: "longtext", nullable: true),
                    PastatoTipas = table.Column<string>(type: "longtext", nullable: true),
                    Sildymas = table.Column<string>(type: "longtext", nullable: true),
                    PastatoEnergijosSuvartojimoKlase = table.Column<string>(type: "longtext", nullable: true),
                    Ypatybes = table.Column<string>(type: "longtext", nullable: true),
                    PapildomosPatalpos = table.Column<string>(type: "longtext", nullable: true),
                    PapildomaIranga = table.Column<string>(type: "longtext", nullable: true),
                    Apsauga = table.Column<string>(type: "longtext", nullable: true),
                    Vanduo = table.Column<string>(type: "longtext", nullable: true),
                    IkiTelkinio = table.Column<int>(type: "int", nullable: true),
                    ArtimiausiasTelkinys = table.Column<string>(type: "longtext", nullable: true),
                    RCNumeris = table.Column<string>(type: "longtext", nullable: true),
                    Aprasymas = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Housing", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Scrape",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ScrapeType = table.Column<int>(type: "int", nullable: false),
                    ScrapeStatus = table.Column<int>(type: "int", nullable: false),
                    ScrapedSite = table.Column<int>(type: "int", nullable: false),
                    DateScraped = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ScrapeTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Message = table.Column<string>(type: "longtext", nullable: true),
                    TotalQueries = table.Column<int>(type: "int", nullable: true),
                    NewQueries = table.Column<int>(type: "int", nullable: true),
                    TotalErrors = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scrape", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ScrapeError",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "longtext", nullable: true),
                    StackTrace = table.Column<string>(type: "longtext", nullable: true),
                    ScrapeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapeError", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ScrapeError_Scrape_ScrapeID",
                        column: x => x.ScrapeID,
                        principalTable: "Scrape",
                        principalColumn: "ID");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SavedSearch",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    HousingID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedSearch", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SavedSearch_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SavedSearch_UserID",
                table: "SavedSearch",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapeError_ScrapeID",
                table: "ScrapeError",
                column: "ScrapeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Housing");

            migrationBuilder.DropTable(
                name: "SavedSearch");

            migrationBuilder.DropTable(
                name: "ScrapeError");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Scrape");
        }
    }
}
