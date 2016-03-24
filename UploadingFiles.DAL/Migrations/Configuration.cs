using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadingFiles.DAL.Contexts;

namespace UploadingFiles.DAL.Migrations
{
    public class Configuration : DbMigrationsConfiguration<FilesUploadingContext>
    {
        protected override void Seed(FilesUploadingContext context)
        {
            base.Seed(context);
        }
    }
}
