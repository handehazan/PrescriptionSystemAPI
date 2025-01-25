using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prescriptionSystemApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrescriptionMedicineModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrescriptionMedicines_Medicine_MedicineId",
                table: "PrescriptionMedicines");

            migrationBuilder.DropTable(
                name: "Medicine");

            migrationBuilder.DropIndex(
                name: "IX_PrescriptionMedicines_MedicineId",
                table: "PrescriptionMedicines");

            migrationBuilder.DropColumn(
                name: "MedicineId",
                table: "PrescriptionMedicines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MedicineId",
                table: "PrescriptionMedicines",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Medicine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ATCAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ATCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktifUrunlerListesineAlindigiTarih = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Barkod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CocukTemelIlacListesiDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Durumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlacAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceteTuru = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemelIlacListesiDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YenidoganTemelIlacListesiDurumu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionMedicines_MedicineId",
                table: "PrescriptionMedicines",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrescriptionMedicines_Medicine_MedicineId",
                table: "PrescriptionMedicines",
                column: "MedicineId",
                principalTable: "Medicine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
