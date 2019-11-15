using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthApi.Migrations
{
    public partial class RemoveSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("64093606-828a-4b91-bfa2-0b977ad50f38"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "EmailAddress", "Password" },
                values: new object[] { new Guid("64093606-828a-4b91-bfa2-0b977ad50f38"), "duplicateuser@test.com", "Yvdksn0nDhJ+ladRQxDyNQ==.Y5fFvB+PZpLmq7MFbMJYGaSgznPYqbosGLPg6uSNyPU=" });
        }
    }
}
