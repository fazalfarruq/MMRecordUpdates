using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsvHelper;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Configuration;
using CsvHelper.Configuration.Attributes;
using MMRecordsUpdate.Models;
using Newtonsoft.Json;

namespace MMRecordsUpdate.BLL
{
    public class FileReader
    {
        private readonly string _delimiter;

        public FileReader(string delimiter)
        {
            _delimiter = delimiter;
        }

        public List<CsvCustomer> ReadCsvFile(string fileName)
        {

            List<CsvCustomer> customerRecords = new List<CsvCustomer>();
            using (StreamReader str = File.OpenText(fileName))
            {
                using (CsvReader csvReader = new CsvReader(str))
                {
                    csvReader.Configuration.Delimiter = _delimiter;
                    csvReader.Configuration.RegisterClassMap<CsvCustomerMap>();
                    csvReader.Read();
                    csvReader.ReadHeader();
                    customerRecords = csvReader.GetRecords<CsvCustomer>().ToList();
                }
            }

            return customerRecords;
        }
    }
}