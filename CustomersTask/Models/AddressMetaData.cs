using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CustomersTask.Models
{

    [Serializable]
    [XmlRoot("Customers"), XmlType("Customers")]
    public class AddressMetaData
    {

        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string Title { get; set; }
        public string Street { get; set; }
        public string PLZ { get; set; }
        public int CityID { get; set; }

        public virtual CityMetaData City { get; set; }
        public virtual CustomerMetaData Customer { get; set; }
    }
}