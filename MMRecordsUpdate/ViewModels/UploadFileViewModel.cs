using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MMRecordsUpdate.BLL;
using MMRecordsUpdate.BLL.Attributes;

namespace MMRecordsUpdate.ViewModels
{
    public class UploadFileViewModel
    {

        [Display(Name = "File Upload")]
        [UploadValidator(".csv,.xls,.xlsx", ErrorMessage = "Invalid file type.")]
        public HttpPostedFileBase UploadFile { get; set; }
    }
}