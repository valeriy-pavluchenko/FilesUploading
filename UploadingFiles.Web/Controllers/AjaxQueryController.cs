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
    public class AjaxQueryController : Controller
    {
        UploadingFilesProvider _provider;

        public AjaxQueryController()
        {
            _provider = new UploadingFilesProvider();
        }

        [HttpGet]
        public JsonResult GetAllFiles()
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

            return Json(filesViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFile(int id)
        {
            var file = _provider.GetFile(id);
            var fileViewModel = new UploadedFileViewModel
            {
                Id = file.Id,
                Name = file.Name,
                Description = file.Description
            };

            return Json(fileViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddFile(UploadedFileViewModel fileViewModel, HttpPostedFileBase file)
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

            var addedFile = _provider.AddFile(uploadedFile);
            var addedFileViewModel = new UploadedFileViewModel
            {
                Id = addedFile.Id,
                Name = addedFile.Name,
                Description = addedFile.Description
            };

            return Json(addedFileViewModel);
        }

        [HttpPost]
        public void RemoveFile(UploadedFileViewModel fileViewModel)
        {
            var file = new UploadedFile
            {
                Id = fileViewModel.Id
            };

            _provider.RemoveFile(file);
        }
    }
}