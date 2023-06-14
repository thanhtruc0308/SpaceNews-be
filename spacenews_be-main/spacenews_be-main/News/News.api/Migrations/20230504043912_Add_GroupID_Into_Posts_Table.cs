using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.api.Migrations
{
    /// <inheritdoc />
    public partial class Add_GroupID_Into_Posts_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "GroupID",
               table: "Posts",
               type: "int",
               nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
