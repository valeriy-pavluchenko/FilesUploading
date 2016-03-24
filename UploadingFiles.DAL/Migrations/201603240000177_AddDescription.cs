namespace UploadingFiles.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadedFiles", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadedFiles", "Description");
        }
    }
}
