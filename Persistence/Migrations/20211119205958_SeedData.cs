using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("9527649d-aa37-4d91-8fb1-c984eea6c25f"), "Channel for AngularJS and Angular 11", "Angular" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("b452369b-51cf-4288-ba3d-0a4d781afdc1"), "Channel for ReactJS", "React" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("b1753190-21cf-4c88-9df8-e1da1a3210dd"), "Channel for NetCore, Net 5 and Net Framework", "Angular" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("9527649d-aa37-4d91-8fb1-c984eea6c25f"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("b1753190-21cf-4c88-9df8-e1da1a3210dd"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("b452369b-51cf-4288-ba3d-0a4d781afdc1"));
        }
    }
}
