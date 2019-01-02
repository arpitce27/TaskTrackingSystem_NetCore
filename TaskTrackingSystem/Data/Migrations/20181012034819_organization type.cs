using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SampleWebApp.Data.Migrations
{
    public partial class organizationtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationType_OrganizationTypeID",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationType",
                table: "OrganizationType");

            migrationBuilder.RenameTable(
                name: "OrganizationType",
                newName: "OrganizationTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationTypes",
                table: "OrganizationTypes",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationTypes_OrganizationTypeID",
                table: "Organizations",
                column: "OrganizationTypeID",
                principalTable: "OrganizationTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_OrganizationTypes_OrganizationTypeID",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationTypes",
                table: "OrganizationTypes");

            migrationBuilder.RenameTable(
                name: "OrganizationTypes",
                newName: "OrganizationType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationType",
                table: "OrganizationType",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationType_OrganizationTypeID",
                table: "Organizations",
                column: "OrganizationTypeID",
                principalTable: "OrganizationType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
