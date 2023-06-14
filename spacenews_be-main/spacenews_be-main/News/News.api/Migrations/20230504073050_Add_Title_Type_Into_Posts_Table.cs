using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.api.Migrations
{
    /// <inheritdoc />
    public partial class Add_Title_Type_Into_Posts_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.AddColumn<int>(
           name: "Title",
           table: "Posts",
           type: "nvarchar(450)",
           nullable: true);
          migrationBuilder.AddColumn<int>(
           name: "Type",
           table: "Posts",
           type: "nvarchar(450)",
           nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
