using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace MMRecordsUpdate.Models
{
    public class CsvCustomer
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        
        public string Email { get; set; }

        
        public string PostalCode { get; set; }

        
        public string Language { get; set; }

        
        public string PhoneNumber { get; set; }

        public int RowNumber { get; set; }
    }


    public sealed class CsvCustomerMap : ClassMap<CsvCustomer>
    {
        public CsvCustomerMap()
        {
            Map(m => m.FirstName);
            Map(m => m.LastName);
            Map(m => m.Email);
            Map(m => m.PostalCode);
            Map(m => m.Language);
            Map(m => m.PhoneNumber);
            Map(m => m.RowNumber).ConvertUsing(row => row.Context.Row);
        }
    }
}