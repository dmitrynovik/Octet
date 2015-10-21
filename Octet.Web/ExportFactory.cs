using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Octet.Web
{
    public class ExportFactory
    {
        public static FileResult GetFile<T>(string format, ICollection<T> items)
        {
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            switch (format.ToLower())
            {
                case "csv":
                    return new CsvActionResult<T>(items, "items.csv");
                default:
                    throw new ArgumentException(string.Format("Format {0} not supported", format), "format");
            }
        }
    }
}