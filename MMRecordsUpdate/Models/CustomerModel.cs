using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMRecordsUpdate.Models
{
    public class CustomerModel
    {
        public string OverrideCustomerRegistryToken { get; set; }
        public int? OverrideBucketNumber { get; set; }

        public string MAXNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string SuiteType { get; set; }
        public string SuiteNumber { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string NumberInHousehold { get; set; }
        public GenderEnum Gender { get; set; }
        public PreferredLangEnum Lang { get; set; }
        public string BirthdayDay { get; set; }
        public string BirthdayMonth { get; set; }
        public string BirthdayYear { get; set; }
        public bool Suspended { get; set; }
        public bool IsEmployee { get; set; }
        public string CCCCKEY { get; set; }
        public string StoreNumber { get; set; }
        public AdditionalInfoPortableWrapper AdditionalInfo { get; set; }
        public string Notes { get; set; }
        public string GroupId { get; set; }

        public bool? EmailOptOut { get; set; }

        // Registration only
        public bool ReceiveCardUponRegistering { get; set; }
    }

    public enum GenderEnum
    {
        UNKNOWN,
        MALE,
        FEMALE
    }

    public enum PreferredLangEnum
    {
        UNKNOWN,
        ENGLISH,
        FRENCH
    }

    public class AdditionalInfoPortableWrapper
    {
        public AdditionalInfoPortable Info { get; set; }
    }

    public class AdditionalInfoPortable
    {
        public string EmailPromotionOptOut { get; set; }
        public int PointOfSaleID { get; set; }
    }
}