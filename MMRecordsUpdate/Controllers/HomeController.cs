using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using MMRecordsUpdate.BLL;
using MMRecordsUpdate.Models;
using MMRecordsUpdate.ViewModels;
using Renci.SshNet;

namespace MMRecordsUpdate.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            var model = new UploadFileViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UploadFileViewModel model)
        {

            if (model.UploadFile == null || model.UploadFile.ContentLength == 0)
            {
                ModelState.AddModelError("UploadFile", "Please upload file.");
            }

            if (ModelState.IsValid)
            {
                string tempFileName = System.IO.Path.GetTempFileName();
                model.UploadFile.SaveAs(tempFileName);

                FileReader reader = new FileReader(ConfigurationManager.AppSettings["Delimiter"]);
                var csvCustomers = reader.ReadCsvFile(tempFileName);
                var customers = csvCustomers.Where(c => c.Email == "claudine_nadeau@hotmail.com");
                
                foreach (var customer in customers)
                {
                    CustomerModel maxCustomer = new CustomerModel()
                    {
                        Lang = (customer.Language.ToLower() == "french")
                         ? (PreferredLangEnum.FRENCH)
                         : (PreferredLangEnum.ENGLISH)
                    };

                    maxCustomer.FirstName = customer.Firstname.Trim();
                    maxCustomer.LastName = customer.LastName.Trim();
                    maxCustomer.HomePhone = Utils.GetFormattedPhoneNumber(customer.Telephone.Trim());
                    maxCustomer.Email = customer.Email.ToLowerInvariant();

                    maxCustomer.OverrideBucketNumber = 5;
                    
                    SyncApiClient client = new SyncApiClient(ConfigurationManager.AppSettings["WebsiteSyncApiUrl"]);
                    string matchedMaxNumber = client.CheckUserExistsByPhoneNumber(maxCustomer);

                    if (!string.IsNullOrEmpty(matchedMaxNumber))
                    {
                        CustomerModel maxCustomerResult =
                            client.GetMaxCustomer(matchedMaxNumber, maxCustomer.HomePhone).FirstOrDefault();

                        if (maxCustomerResult != null && maxCustomerResult.Email == maxCustomer.Email)
                        {

                            if (!maxCustomerResult.EmailOptOut.Value)
                            {
                                maxCustomerResult.Lang = maxCustomer.Lang;
                            }
                            else
                            {
                                maxCustomerResult.Lang = maxCustomer.Lang;
                                maxCustomerResult.EmailOptOut = false;

                            }

                        }

                        if (maxCustomerResult != null && maxCustomerResult.Email != maxCustomer.Email)
                        {

                            if (string.Equals(maxCustomerResult.GroupId,
                                ConfigurationManager.AppSettings["SilverSecuredGroupId"]))
                            {

                            }
                            else
                            {
                                maxCustomerResult.Lang = maxCustomer.Lang;
                                maxCustomerResult.EmailOptOut = false;
                                maxCustomerResult.Email = maxCustomer.Email;

                            }

                        }

                        maxCustomerResult.ReceiveCardUponRegistering = false;

                        bool result = client.EditMaxCustomer(maxCustomerResult);
                    }
                    else
                    {
                        // Create new Erply account
                        if (!string.IsNullOrEmpty(customer.PostalCode))
                        {
                            maxCustomer.PostalCode = Utils.GetFormattedPostalCode(customer.PostalCode);
                        }

                        maxCustomer.ReceiveCardUponRegistering = false;
                        maxCustomer.EmailOptOut = false;
                        maxCustomer.BirthdayDay = "0";
                        maxCustomer.BirthdayMonth = "0";
                        maxCustomer.BirthdayYear = "0";

                        maxCustomer.Gender = GenderEnum.UNKNOWN;

                        client.AddMaxCustomer(maxCustomer);

                    }
                }
                

                return View(model);
            }
        }
    }
}