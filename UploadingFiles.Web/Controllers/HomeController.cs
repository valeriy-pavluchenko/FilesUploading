using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UploadingFiles.BL.Providers;
using UploadingFiles.DAL.Entities;
using UploadingFiles.Web.Models;

namespace UploadingFiles.Web.Controllers
{
    public class HomeController : Controller
    {
        UploadingFilesProvider _provider;

        public HomeController()
        {
            _provider = new UploadingFilesProvider();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FilesList()
        {
            var files = _provider.GetAllFiles();

            var filesViewModel = new List<UploadedFileViewModel>();
            foreach (var file in files)
            {
                filesViewModel.Add(new UploadedFileViewModel
                {
                    Id = file.Id,
                    Name = file.Name,
                    Description = file.Description
                });
            }

            return PartialView(filesViewModel);
        }

        [HttpGet]
        public FileResult Download(int id)
        {
            var file = _provider.GetFile(id);

            return File(file.Data, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
        }

        [HttpPost]
        public ActionResult AddFile(UploadedFileViewModel fileViewModel, HttpPostedFileBase file)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }

            var uploadedFile = new UploadedFile
            {
                Name = file.FileName,
                Description = fileViewModel.Description,
                Data = fileData
            };

            _provider.AddFile(uploadedFile);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RemoveFile(UploadedFileViewModel fileViewModel)
        {
            var file = new UploadedFile
            {
                Id = fileViewModel.Id
            };

            _provider.RemoveFile(file);
            return RedirectToAction("Index");
        }
    }
}