using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration.Attributes;

namespace MMRecordsUpdate.Models
{
    public class CsvCustomer
    {
        [Name("First Name")]
        public string Firstname { get; set; }
        [Name("Last Name")]
        public string LastName { get; set; }

        [Name("Email")]
        public string Email { get; set; }

        [Name("Postal Code")]
        public string PostalCode { get; set; }

        [Name("Language")]
        public string Language { get; set; }

        [Name("Phone Number")]
        public string Telephone { get; set; }
    }
}