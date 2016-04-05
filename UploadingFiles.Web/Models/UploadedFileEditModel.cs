using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UploadingFiles.Web.Attributes;

namespace UploadingFiles.Web.Models
{
    [JsonObject]
    [JsonNetModel]
    public class UploadedFileEditModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("file")]
        public HttpPostedFileBase File { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}