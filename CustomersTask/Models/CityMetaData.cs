using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CustomersTask.Models
{

    [Serializable]
    [XmlRoot("Customers"), XmlType("Customers")]
    public class CityMetaData
    {
        public CityMetaData()
        {
            this.Addresses = new HashSet<AddressMetaData>();
        }

        public int ID { get; set; }
        public string CityName { get; set; }
        public virtual ICollection<AddressMetaData> Addresses { get; set; }
    }
}