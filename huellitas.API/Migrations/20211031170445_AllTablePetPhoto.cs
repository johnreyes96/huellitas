using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace huellitas.API.Migrations
{
    public partial class AllTablePetPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingDetails_Billings_BillingId",
                table: "BillingDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_UserId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetTypes_petTypeId",
                table: "Pets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillingDetails",
                table: "BillingDetails");

            migrationBuilder.RenameTable(
                name: "BillingDetails",
                newName: "BillingDetail");

            migrationBuilder.RenameIndex(
                name: "IX_BillingDetails_Id",
                table: "BillingDetail",
                newName: "IX_BillingDetail_Id");

            migrationBuilder.RenameIndex(
                name: "IX_BillingDetails_BillingId",
                table: "BillingDetail",
                newName: "IX_BillingDetail_BillingId");

            migrationBuilder.AlterColumn<int>(
                name: "petTypeId",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observations",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillingDetail",
                table: "BillingDetail",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PetPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetPhotos_Pets_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetPhotos_VehicleId",
                table: "PetPhotos",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingDetail_Billings_BillingId",
                table: "BillingDetail",
                column: "BillingId",
                principalTable: "Billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_UserId",
                table: "Pets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetTypes_petTypeId",
                table: "Pets",
                column: "petTypeId",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingDetail_Billings_BillingId",
                table: "BillingDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_AspNetUsers_UserId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_PetTypes_petTypeId",
                table: "Pets");

            migrationBuilder.DropTable(
                name: "PetPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillingDetail",
                table: "BillingDetail");

            migrationBuilder.RenameTable(
                name: "BillingDetail",
                newName: "BillingDetails");

            migrationBuilder.RenameIndex(
                name: "IX_BillingDetail_Id",
                table: "BillingDetails",
                newName: "IX_BillingDetails_Id");

            migrationBuilder.RenameIndex(
                name: "IX_BillingDetail_BillingId",
                table: "BillingDetails",
                newName: "IX_BillingDetails_BillingId");

            migrationBuilder.AlterColumn<int>(
                name: "petTypeId",
                table: "Pets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Observations",
                table: "Pets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillingDetails",
                table: "BillingDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingDetails_Billings_BillingId",
                table: "BillingDetails",
                column: "BillingId",
                principalTable: "Billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_AspNetUsers_UserId",
                table: "Pets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_PetTypes_petTypeId",
                table: "Pets",
                column: "petTypeId",
                principalTable: "PetTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
