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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                log.Debug($"FileName is {model.UploadFile.FileName}");
                string tempFileName = System.IO.Path.GetTempFileName();
                model.UploadFile.SaveAs(tempFileName);

                FileReader reader = new FileReader(ConfigurationManager.AppSettings["Delimiter"]);
                var csvCustomers = reader.ReadCsvFile(tempFileName);
                var customers = csvCustomers.Where(c => c.Email == "claudine_nadeau@hotmail.com");
                log.Debug($"Number of CSV Records is {customers.Count()}");

                foreach (var customer in customers)
                {
                    CustomerModel maxCustomer = new CustomerModel()
                    {
                        Lang = (customer.Language.ToLower() == "french")
                         ? (PreferredLangEnum.FRENCH)
                         : (PreferredLangEnum.ENGLISH)
                    };

                    maxCustomer.FirstName = customer.FirstName.Trim();
                    maxCustomer.LastName = customer.LastName.Trim();
                    maxCustomer.HomePhone = Utils.GetFormattedPhoneNumber(customer.PhoneNumber.Trim());
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
                                log.Debug($"CSV RowNumber: {customer.RowNumber}");
                                log.Debug($"Customer record found with matching email and Explicit Consent: Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                                maxCustomerResult.Lang = maxCustomer.Lang;
                                log.Debug($"Customer language updated to {maxCustomer.Lang} for customer with Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                            }
                            else
                            {
                                log.Debug($"CSV RowNumber: {customer.RowNumber}");
                                log.Debug($"Customer record found with matching email and No Explicit Consent: Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                                maxCustomerResult.Lang = maxCustomer.Lang;
                                maxCustomerResult.EmailOptOut = false;
                                log.Debug($"Customer language and explicit consent updated to {maxCustomer.Lang} for customer with Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                            }

                        }

                        if (maxCustomerResult != null && maxCustomerResult.Email != maxCustomer.Email)
                        {

                            if (string.Equals(maxCustomerResult.GroupId,
                                ConfigurationManager.AppSettings["SilverSecuredGroupId"]))
                            {
                                log.Debug($"CSV RowNumber: {customer.RowNumber}");
                                log.Debug($"Customer record found with no matching email and is silver secured: Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                                continue;
                            }
                            else
                            {
                                log.Debug($"CSV RowNumber: {customer.RowNumber}");
                                log.Debug($"Customer record found with no matching email and is not silver secured: Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                                maxCustomerResult.Lang = maxCustomer.Lang;
                                maxCustomerResult.EmailOptOut = false;
                                maxCustomerResult.Email = maxCustomer.Email;
                                log.Debug($"Customer record updated with Language, Explicit Consent, Email for customer with no matching email and is not silver secured: Email: {customer.Email} , PhoneNumber: {customer.PhoneNumber}");
                            }

                        }

                        maxCustomerResult.ReceiveCardUponRegistering = false;

                        //bool result = client.EditMaxCustomer(maxCustomerResult);

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

                        log.Debug($"CSV RowNumber: {customer.RowNumber}");

                        //client.AddMaxCustomer(maxCustomer);

                    }
                }

                return View(model);
            }
            return View(model);
        }
    }
}