using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CustomersTask.Models
{
    [Serializable]
    [XmlRoot("Customers"), XmlType("Customers")]
    public class CustomerMetaData
    {
        public CustomerMetaData()
        {
            this.addressMetaData = new HashSet<AddressMetaData>();
        }

        public int ID { get; set; }
        public string CustomerNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Tel { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }

        public virtual ICollection<AddressMetaData> addressMetaData { get; set; }
    }
}