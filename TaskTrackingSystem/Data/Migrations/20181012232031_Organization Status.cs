using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SampleWebApp.Data.Migrations
{
    public partial class OrganizationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationTypes_OrganizationTypeID",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationTypeID",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Organizations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrganizationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_StatusId",
                table: "Organizations",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationTypes_OrganizationTypeID",
                table: "Organizations",
                column: "OrganizationTypeID",
                principalTable: "OrganizationTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationStatuses_StatusId",
                table: "Organizations",
                column: "StatusId",
                principalTable: "OrganizationStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationTypes_OrganizationTypeID",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationStatuses_StatusId",
                table: "Organizations");

            migrationBuilder.DropTable(
                name: "OrganizationStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_StatusId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Organizations");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationTypeID",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationTypes_OrganizationTypeID",
                table: "Organizations",
                column: "OrganizationTypeID",
                principalTable: "OrganizationTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
