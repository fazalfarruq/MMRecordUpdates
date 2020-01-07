using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MMRecordsUpdate.BLL.Attributes
{

    /// <summary>
    /// Validates file extensions on a HttpPostedFileBase
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UploadValidatorAttribute : ValidationAttribute, IClientValidatable
    {
        private List<string> ValidExtensions { get; set; }

        /// <summary>
        /// Initialize without allowed extensions. Allowed extensions pulled from appSetting Media.AllowedExtensions 
        /// </summary>
        public UploadValidatorAttribute()
        {
            string fileExtensions = System.Configuration.ConfigurationManager.AppSettings["Uploads.AllowedExtensions"];
            SetExtensions(fileExtensions);
        }

        /// <summary>
        /// Initialize with a comma-delimited list of allowed extensions
        /// </summary>
        /// <param name="fileExtensions">Comma-delimited list of extensions.</param>
        public UploadValidatorAttribute(string fileExtensions)
        {
            if (String.IsNullOrEmpty(fileExtensions))
            {
                throw new ArgumentException("fileExtensions is required.");
            }

            SetExtensions(fileExtensions);
        }

        private void SetExtensions(string fileExtensions)
        {
            string extensions = fileExtensions.Trim().Replace(".", "").Replace(" ", "");
            extensions = extensions + "," + extensions.ToUpper(); // duplicate into uppercase

            ValidExtensions = extensions.Split(',').Distinct().ToList();
        }

        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file != null)
            {
                var fileName = file.FileName;
                var isValidExtension = ValidExtensions.Any(y => System.IO.Path.GetExtension(fileName).TrimStart('.') == y);
                return isValidExtension;
            }
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "uploadvalidator"
            };

            rule.ValidationParameters["extensions"] = String.Join("|", ValidExtensions.ToArray());
            yield return rule;
        }

    }

}