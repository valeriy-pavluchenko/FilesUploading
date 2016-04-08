using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UploadingFiles.DAL.Contexts;
using UploadingFiles.DAL.Entities;

namespace UploadingFiles.BL.Providers
{
    public class UploadingFilesProvider
    {
        FilesUploadingContext _context;

        public UploadingFilesProvider()
        {
            _context = new FilesUploadingContext();
        }

        public List<UploadedFile> GetAllFiles()
        {
            return _context.Files.OrderByDescending(p => p.Id).ToList();
        }

        public UploadedFile GetFile(int id)
        {
            return _context.Files.Where(f => f.Id == id).SingleOrDefault();
        }

        public UploadedFile AddFile(UploadedFile file)
        {
            _context.Files.Add(file);
            _context.SaveChanges();

            return file;
        }

        public void RemoveFile(UploadedFile file)
        {
            var entry = _context.Entry(file);
            entry.State = EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
