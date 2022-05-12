using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Projecten2_TicketingPlatform.Data.Migrations
{
    public partial class DatabaseFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "ContractTypes",
                columns: table => new
                {
                    ContractTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ManierVanAanmakenTicket = table.Column<int>(nullable: false),
                    TijdstipTicketAanmaken = table.Column<int>(nullable: false),
                    MinimaleDoorlooptijd = table.Column<int>(nullable: false),
                    MaximaleAfhandeltijd = table.Column<int>(nullable: false),
                    ContractPrijs = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTypes", x => x.ContractTypeId);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeBase",
                columns: table => new
                {
                    KnowledgeBaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(nullable: true),
                    DatumToevoegen = table.Column<DateTime>(nullable: false),
                    Omschrijving = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeBase", x => x.KnowledgeBaseId);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Ticketid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DatumAanmaken = table.Column<DateTime>(nullable: false),
                    Omschrijving = table.Column<string>(nullable: true),
                    TypeTicket = table.Column<int>(nullable: false),
                    KlantId = table.Column<string>(nullable: true),
                    TechniekerId = table.Column<string>(nullable: true),
                    Opmerkingen = table.Column<string>(nullable: true),
                    Bijlage = table.Column<string>(nullable: true),
                    Kwaliteit = table.Column<int>(nullable: false),
                    Oplossing = table.Column<bool>(nullable: false),
                    SupportNodig = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Ticketid);
                });

           

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDatum = table.Column<DateTime>(nullable: false),
                    ContractTypeId = table.Column<int>(nullable: false),
                    EindDatum = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<string>(nullable: true),
                    Doorlooptijd = table.Column<int>(nullable: false),
                    ContractStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contract_ContractTypes_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "ContractTypes",
                        principalColumn: "ContractTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ContractTypeId",
                table: "Contract",
                column: "ContractTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "KnowledgeBase");

            migrationBuilder.DropTable(
                name: "Ticket");


            migrationBuilder.DropTable(
                name: "ContractTypes");
        }
    }
}
