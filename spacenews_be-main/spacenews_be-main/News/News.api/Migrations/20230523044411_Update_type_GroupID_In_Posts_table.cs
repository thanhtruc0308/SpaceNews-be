using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.api.Migrations
{
    /// <inheritdoc />
    public partial class Update_type_GroupID_In_Posts_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = "ALTER TABLE [dbo].[Posts] ALTER COLUMN [GroupID] nvarchar(450);";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
