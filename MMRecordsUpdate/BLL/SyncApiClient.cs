using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using MMRecordsUpdate.Models;
using Newtonsoft.Json;

namespace MMRecordsUpdate.BLL
{
    public class SyncApiClient
    {
        private readonly string Url;

        public SyncApiClient(string url)
        {
            Url = url;
        }

        public CustomerByEmailResult GetMaxCustomerByEmail(string email)
        {
            string urlSuffix = "/api/Sync/Customer/GetMaxCustomerByEmail?email=" + email;

            using (WebClient client = new WebClient())
            {
                CredentialCache cc = new CredentialCache
                {
                    {
                        new Uri(Url),
                        "NTLM",
                        new NetworkCredential("srvWebsiteSync", "LpgZ2KQxnhOjKYG6", "MMM")
                    }
                };

                client.Credentials = cc;
                client.Encoding = Encoding.UTF8;

                #region GetMaxCustomerByEmail
                try
                {
                    string getMaxCustomerByEmail = Regex.Unescape(client.DownloadString(Url + urlSuffix));
                    getMaxCustomerByEmail = getMaxCustomerByEmail.Substring(1, getMaxCustomerByEmail.Length - 2);

                    CustomerByEmailResult deserialized = JsonConvert.DeserializeObject<CustomerByEmailResult>(Regex.Unescape(getMaxCustomerByEmail));
                    return deserialized;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"SyncApiClient's GetMaxCustomerByEmail threw exception {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";


                }
                #endregion
            }
            return null;
        }

        public string CheckUserExistsByPhoneNumber(CustomerModel customerModel)
        {
            string urlSuffix = "/api/Sync/Customer/CheckUserExistsByPhoneNumber?firstName=" + customerModel.FirstName + "&lastName=" + customerModel.LastName + "&phoneNumber=" + customerModel.HomePhone;

            using (WebClient client = new WebClient())
            {
                CredentialCache cc = new CredentialCache
                {
                    {
                        new Uri(Url),
                        "NTLM",
                        new NetworkCredential("srvWebsiteSync", "LpgZ2KQxnhOjKYG6", "MMM")
                    }
                };

                client.Credentials = cc;
                client.Encoding = Encoding.UTF8;

                #region CheckUserExistsByPhoneNumber
                try
                {
                    string maxNumber = client.DownloadString(Url + urlSuffix);

                    if (!string.IsNullOrEmpty(maxNumber))
                    {
                        maxNumber = maxNumber.Substring(1, maxNumber.Length - 2);
                        return maxNumber;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"SyncApiClient's CheckUserExistsByPhoneNumber threw exception {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";

                }
                #endregion
            }
            return null;
        }

        public List<CustomerModel> GetMaxCustomer(string maxNumber, string phoneNumber)
        {
            string urlSuffix = "/api/Sync/Customer/GetMaxCustomer?maxNumber=" + maxNumber + "&phoneNumber=" + phoneNumber;

            using (WebClient client = new WebClient())
            {
                CredentialCache cc = new CredentialCache
                {
                    {
                        new Uri(Url),
                        "NTLM",
                        new NetworkCredential("srvWebsiteSync", "LpgZ2KQxnhOjKYG6", "MMM")
                    }
                };

                client.Credentials = cc;
                client.Encoding = Encoding.UTF8;

                #region GetMaxCustomer
                try
                {
                    string getMaxCustomer = Regex.Unescape(client.DownloadString(Url + urlSuffix));
                    getMaxCustomer = getMaxCustomer.Substring(1, getMaxCustomer.Length - 2);

                    List<CustomerModel> deserialized = JsonConvert.DeserializeObject<List<CustomerModel>>(Regex.Unescape(getMaxCustomer));
                    return deserialized;
                }
                catch (Exception ex)
                {
                    string errorMessage = $"SyncApiClient's GetMaxCustomer threw exception {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";

                }
                #endregion
            }
            return null;
        }

        public bool EditMaxCustomer(CustomerModel customer)
        {
            string urlSuffix = "/api/Sync/Customer/EditMaxCustomer";
            try
            {
                WebRequest wrequest = WebRequest.Create(Url + urlSuffix);

                CredentialCache cc = new CredentialCache
                {
                    {
                        new Uri(Url),
                        "NTLM",
                        new NetworkCredential("srvWebsiteSync", "LpgZ2KQxnhOjKYG6", "MMM")
                    }
                };

                wrequest.Credentials = cc;
                wrequest.Method = "POST";
                wrequest.ContentType = "application/json";

                string jsonData = string.Empty;

                // Write JSON to the WebRequest
                using (StreamWriter streamWriter = new StreamWriter(wrequest.GetRequestStream()))
                {
                    jsonData = JsonConvert.SerializeObject(customer, Formatting.Indented);

                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                WebResponse response = wrequest.GetResponse();
                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                bool result = Convert.ToBoolean(responseFromServer);

                reader.Close();
                dataStream.Close();
                response.Close();

                return result;
            }
            catch (Exception ex)
            {
                string errorMessage = $"SyncApiClient's EditMaxCustomer threw exception {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
            }

            return false;
        }

        public string AddMaxCustomer(CustomerModel customer)
        {
            string urlSuffix = "/api/Sync/Customer/AddMaxCustomer";

            using (WebClient client = new WebClient())
            {
                CredentialCache cc = new CredentialCache
                {
                    {
                        new Uri(Url),
                        "NTLM",
                        new NetworkCredential("srvWebsiteSync", "LpgZ2KQxnhOjKYG6", "MMM")
                    }
                };

                client.Credentials = cc;
                client.Encoding = Encoding.UTF8;

                #region AddMaxCustomer
                try
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    //Logger.Write($"About to call Sync API AddMaxCustomer with parameters: {Newtonsoft.Json.JsonConvert.SerializeObject(customer)}{Environment.NewLine}Query string: {GetQueryString(customer)}");


                    string maxNumber = client.UploadString(Url + urlSuffix, Utils.GetQueryString(customer));

                    if (!string.IsNullOrEmpty(maxNumber))
                    {
                        maxNumber = maxNumber.Substring(1, maxNumber.Length - 2);

                        return maxNumber;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = $"SyncApiClient's AddMaxCustomer threw exception {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";

                    //Logger.Write(errorMessage);

                    //ErrorMessages += errorMessage;
                }
                #endregion
            }

            return string.Empty;
        }

    }
}