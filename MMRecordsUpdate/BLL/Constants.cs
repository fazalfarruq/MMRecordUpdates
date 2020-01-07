using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMRecordsUpdate.BLL
{
    public static class Constants
    {
        public static string CustomerByEmailUrl => "/api/Sync/Customer/GetMaxCustomerByEmail?email=";
        public static string UserExistsByPhoneNumberUrl => "/api/Sync/Customer/GetMaxCustomerByEmail?email=";
        public static string GetMaxCustomerUrl => "/api/Sync/Customer/GetMaxCustomer?maxNumber=";
        public static string EditMaxCustomerUrl => "/api/Sync/Customer/EditMaxCustomer";
        public static string AddMaxCustomerUrl => "/api/Sync/Customer/AddMaxCustomer";


    }
}