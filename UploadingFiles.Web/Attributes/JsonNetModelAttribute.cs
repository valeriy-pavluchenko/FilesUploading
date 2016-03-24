using UploadingFiles.Web.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UploadingFiles.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface |
                    AttributeTargets.Struct | AttributeTargets.Property,
                    AllowMultiple = false, Inherited = true)]
    public class JsonNetModelAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new JsonNetModelBinder();
        }
    }
}
