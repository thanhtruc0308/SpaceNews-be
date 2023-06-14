using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Groups_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
           name: "Groups",
           columns: table => new
           {
               Id = table.Column<int>(type: "int", nullable: false)
                   .Annotation("SqlServer:Identity", "1, 1"),
               Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
               Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
           },
           constraints: table =>
           {
               table.PrimaryKey("PK_Groups", x => x.Id);

           });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

        }
    }
}
