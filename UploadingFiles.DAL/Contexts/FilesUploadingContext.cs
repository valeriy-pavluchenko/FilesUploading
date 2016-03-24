using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadingFiles.DAL.Entities;

namespace UploadingFiles.DAL.Contexts
{
    public class FilesUploadingContext : DbContext
    {
        public FilesUploadingContext() : base("FilesUploading")
        { }

        public DbSet<UploadedFile> Files { get; set; }
    }
}
