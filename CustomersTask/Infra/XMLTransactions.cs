using CustomersTask.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace CustomersTask.Infra
{
    public class XMLTransactions
    {
        string xmlData = HttpContext.Current.Server.MapPath("~/Infra/Data.xml");
        string xmlCitiesData = HttpContext.Current.Server.MapPath("~/Infra/Cities.xml");

        

        public List<CustomerMetaData> GetCustomers()
        {
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);


            var addresses = new List<AddressMetaData>();
            addresses = (from addressRows in ds.Tables[2].AsEnumerable()//Load addresses Table
                         select new AddressMetaData
                         {
                             ID = Convert.ToInt32(addressRows[0].ToString()), //Convert row to int  
                             CustomerID = Convert.ToInt32(addressRows[1].ToString()),
                             Title = addressRows[2].ToString(),
                             Street = addressRows[3].ToString(),
                             PLZ = addressRows[4].ToString(),
                             CityID = Convert.ToInt32(addressRows[5].ToString())


                         }).ToList();

            var customers = new List<CustomerMetaData>();
            customers = (from rows in ds.Tables[0].AsEnumerable()//Load custoomers Table
                         select new CustomerMetaData
                         {
                             ID = Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                             CustomerNo = rows[1].ToString(),
                             FirstName = rows[2].ToString(),
                             LastName = rows[3].ToString(),
                             Tel = rows[4].ToString(),
                             Cell = rows[5].ToString(),
                             Email = rows[6].ToString(),
                             //assign each customer addresses List
                             addressMetaData = addresses.Where(p => p.CustomerID == Convert.ToInt32(rows[0].ToString())).ToList()
                         }).ToList();


            var cities = new List<CityMetaData>();
            cities = GetCities(addresses);


            return customers;
        }
        /// <summary>
        /// Load Cities from XML file and assein address to model
        /// </summary>
        /// <param name="addresses">object of IEnumerable<AddressMetaData> </param>
        /// <returns>List of CityMetaData</returns>
        public List<CityMetaData> GetCities(IEnumerable<AddressMetaData> addresses)
        {
            //string xmlCityData = HttpContext.Current.Server.MapPath("~/Infra/Cities.xml");
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlCitiesData);
            
            var cities = new List<CityMetaData>();
            cities = (from rows in ds.Tables[0].AsEnumerable()//Load Cities  
                      select new CityMetaData
                      {
                          ID = Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          CityName = rows[1].ToString(),
                          Addresses = addresses.Where(p => p.CityID == Convert.ToInt32(rows[0].ToString())).ToList()
                             
                         }).ToList();
            

            return cities;
        }
        public List<CityMetaData> GetCities()
        {
            //string xmlCityData = HttpContext.Current.Server.MapPath("~/Infra/Cities.xml");
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlCitiesData);

            var cities = new List<CityMetaData>();
            cities = (from rows in ds.Tables[0].AsEnumerable()
                      select new CityMetaData
                      {
                          ID = Convert.ToInt32(rows[0].ToString()), //Convert row to int  
                          CityName = rows[1].ToString()

                      }).ToList();


            return cities;
        }
        public CityMetaData GetCity(int ID)
        {
            CityMetaData city = new CityMetaData();
            XDocument xmlDoc = XDocument.Load(xmlCitiesData);
            //var root = (from item in xmlDoc.Descendants("Customers")

            // select item).ToList();

            var items = (from item in xmlDoc.Descendants("City")
                         where item.Element("ID").Value == ID.ToString()
                         select item).ToList();


            foreach (XElement itemElement in items)
            {
                city.ID = int.Parse(itemElement.Element("ID").Value);
                city.CityName = itemElement.Element("CityName").Value;
                
            }

            return city;

        }

        
        public CustomerMetaData GetCustomer(string ID)
        {
            CustomerMetaData customer = new CustomerMetaData();
            XDocument xmlDoc = XDocument.Load(xmlData);


            var items = (from item in xmlDoc.Descendants("customer")
                         where item.Element("ID").Value == ID
                         select item).ToList();


            foreach (XElement item in items)
            {
                customer = new CustomerMetaData();
                customer.ID = int.Parse(item.Element("ID").Value);
                customer.CustomerNo = item.Element("CustomerNo").Value;
                customer.FirstName = item.Element("FirstName").Value;
                customer.LastName = item.Element("LastName").Value;
                customer.Tel = item.Element("Tel").Value;
                customer.Cell = item.Element("Cell").Value;
                customer.Email = item.Element("Email").Value;
            }



            return customer;
        }


        public void RemoveCustomer(string ID)
        {
            XDocument xmlDoc = XDocument.Load(xmlData);


            var items = (from item in xmlDoc.Descendants("customer")
                         where item.Element("ID").Value == ID
                         select item).ToList();

            items.Remove();


            xmlDoc.Save(xmlData);

        }

        /// <summary>
        /// Edit Customers Info
        /// </summary>
        /// <param name="customer">get an object of CustomerMetaData</param>
        public void EditXML(CustomerMetaData customer)
        {
            XDocument xmlDoc = XDocument.Load(xmlData);

            var items = (from item2 in xmlDoc.Descendants("customer")
                         where item2.Element("ID").Value == customer.ID.ToString()
                         select item2).ToList();

            foreach (XElement itemElement in items)
            {                
                itemElement.SetElementValue("ID", customer.ID);
                itemElement.SetElementValue("CustomerNo", customer.CustomerNo);
                itemElement.SetElementValue("FirstName", customer.FirstName);
                itemElement.SetElementValue("LastName", customer.LastName);
                itemElement.SetElementValue("Tel", customer.Tel);
                itemElement.SetElementValue("Cell", customer.Cell);
                itemElement.SetElementValue("Email", customer.Email);
            }

            xmlDoc.Save(xmlData);
        }



        public void AddCustomer(CustomerMetaData customer)
        {
            XDocument xmlDoc = XDocument.Load(xmlData);

            
            XElement NewCustomer =new XElement("customer");
            XElement newNode;

            newNode = new XElement("ID");
            newNode.Value = (GetCustomers().Max(p => p.ID) + 1).ToString();
            NewCustomer.Add(newNode);

            newNode = new XElement("CustomerNo");
            newNode.Value = customer.CustomerNo.ToString();
            NewCustomer.Add(newNode);

            newNode = new XElement("FirstName");
            newNode.Value = customer.FirstName.ToString();
            NewCustomer.Add(newNode);
            

            newNode = new XElement("LastName");
            newNode.Value = customer.LastName.ToString();
            NewCustomer.Add(newNode);

            newNode = new XElement("Tel");
            newNode.Value = customer.Tel.ToString();
            NewCustomer.Add(newNode);

            newNode = new XElement("Cell");
            newNode.Value = customer.Cell.ToString();
            NewCustomer.Add(newNode);
            
            newNode = new XElement("Email");
            newNode.Value = customer.Email.ToString();
            NewCustomer.Add(newNode);

          
            xmlDoc.Root.Add(NewCustomer);

            
            xmlDoc.Save(xmlData);
        }

        public List<AddressMetaData> GetAddresses(int CustomerID)
        {
            CustomerMetaData customer = new CustomerMetaData();
            XDocument xmlDoc = XDocument.Load(xmlData);

            var XMLAddresses = (from item in xmlDoc.Descendants("address")
                                where item.Element("customerID").Value == CustomerID.ToString()
                                select item).ToList();

            List<AddressMetaData> lstAddresses = new List<AddressMetaData>();
            AddressMetaData address;
            foreach (var item in XMLAddresses)
            {
                address = new AddressMetaData();
                address.ID = int.Parse(item.Element("ID").Value);
                address.CityID = int.Parse(item.Element("CityID").Value);
                address.City = GetCity(int.Parse(item.Element("CityID").Value));
                address.CustomerID = CustomerID;
                address.Title = item.Element("Title").Value;
                address.Street = item.Element("Street").Value;
                address.PLZ = item.Element("PLZ").Value;
                lstAddresses.Add(address);
            }
            return lstAddresses;
        }

        public AddressMetaData GetAddress(string ID)
        {
            AddressMetaData address = new AddressMetaData();
            XDocument xmlDoc = XDocument.Load(xmlData);


            var items = (from item in xmlDoc.Descendants("address")
                         where item.Element("ID").Value == ID
                         select item).ToList();


            foreach (XElement item in items)
            {
                address = new AddressMetaData();
                address.ID = int.Parse(item.Element("ID").Value);
                address.CityID = int.Parse(item.Element("CityID").Value);
                address.City = GetCity(int.Parse(item.Element("CityID").Value));
                address.CustomerID = int.Parse(item.Element("customerID").Value);
                address.Title = item.Element("Title").Value;
                address.Street = item.Element("Street").Value;
                address.PLZ = item.Element("PLZ").Value;
            }



            return address;
        }

        private int GetNextAddressID()
        {
            DataSet ds = new DataSet();//Using dataset to read xml file  
            ds.ReadXml(xmlData);

            var lstAddress = new List<AddressMetaData>();
            lstAddress = (from addressRows in ds.Tables[2].AsEnumerable()
                         select new AddressMetaData
                         {
                             ID = Convert.ToInt32(addressRows[0].ToString()), //Convert row to int  
                             CustomerID = Convert.ToInt32(addressRows[1].ToString()),
                             Title = addressRows[2].ToString(),
                             Street = addressRows[3].ToString(),
                             PLZ = addressRows[4].ToString(),
                             CityID = Convert.ToInt32(addressRows[5].ToString())


                         }).ToList();
            if (lstAddress.Count == 0)
            {
                return  1;
            }
            else
            {
                return lstAddress.Max(p => p.ID) + 1;
            }

        }
        public void AddAddress(AddressMetaData _address)
        {
            XDocument xmlDoc = XDocument.Load(xmlData);
            AddressMetaData address = new AddressMetaData();


            var items = (from item in xmlDoc.Descendants("customer")
                         where item.Element("ID").Value == _address.CustomerID.ToString()
                         select item).ToList();



            XElement NewAddresses = new XElement("Addresses");
            XElement innerNode = new XElement("address");
            XElement newNode;
            

            NewAddresses.Add(innerNode);

            newNode = new XElement("ID");
            newNode.Value = GetNextAddressID().ToString();
            innerNode.Add(newNode);

            newNode = new XElement("customerID");
            newNode.Value = _address.CustomerID.ToString();
            innerNode.Add(newNode);

            newNode = new XElement("Title");
            newNode.Value = _address.Title;
            innerNode.Add(newNode);


            newNode = new XElement("Street");
            newNode.Value = _address.Street;
            innerNode.Add(newNode);

            newNode = new XElement("PLZ");
            newNode.Value = _address.PLZ;
            innerNode.Add(newNode);

            newNode = new XElement("CityID");
            newNode.Value = _address.CityID.ToString();
            innerNode.Add(newNode);


            items[0].Add(NewAddresses);
            


            xmlDoc.Save(xmlData);
        }


        public void EditAddress(AddressMetaData address)
        {
            XDocument xmlDoc = XDocument.Load(xmlData);
            //var root = (from item in xmlDoc.Descendants("Customers")

            //            select item).ToList();

            var items = (from item in xmlDoc.Descendants("address")
                         where item.Element("ID").Value == address.ID.ToString()
                         select item).ToList();

            foreach (XElement itemElement in items)
            {
                itemElement.SetElementValue("ID", address.ID);
                itemElement.SetElementValue("CustomerID", address.CustomerID);
                itemElement.SetElementValue("Title", address.Title);
                itemElement.SetElementValue("Street", address.Street);
                itemElement.SetElementValue("PLZ", address.PLZ);
                itemElement.SetElementValue("CityID", address.CityID);
            }

            xmlDoc.Save(xmlData);
        }

        public void RemoveAddress(string ID)
        {

            XDocument xmlDoc = XDocument.Load(xmlData);


            var items = (from item in xmlDoc.Descendants("address")
                         where item.Element("ID").Value == ID
                         select item).ToList();

            items.Remove();
            xmlDoc.Save(xmlData);

        }



    }
}

