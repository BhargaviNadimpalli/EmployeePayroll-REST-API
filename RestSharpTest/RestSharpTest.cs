using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace RestSharpTest
{
    public class Employee
    {
        public int id { get; set; }
        public string name { get; set; }
        public string salary { get; set; }
    }
    [TestClass]
    public class RestSharpTest
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000");
        }

        private IRestResponse GetEmployeeList()
        {
            RestRequest request = new RestRequest("/employees", Method.GET);

            IRestResponse response = client.Execute(request);
            return response;
        } 

        [TestMethod]
        public void onCallingGetReturnEmployeeList()
        {
            IRestResponse response = GetEmployeeList();

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> data = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(9, data.Count);

            foreach(Employee e in data)
            {
                Console.WriteLine("id: " + e.id + "name: " + e.name + "salary: " + e.salary);
            }
        }
        [TestMethod]
        public void givenOnPostReturnEmployeeDetails()
        {

            RestRequest request = new RestRequest("/employees", Method.POST);
            JObject obj = new JObject();
            obj.Add("name", "Maa");
            obj.Add("salary", "50000");

            request.AddParameter("application/json", obj, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee data = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Maa", data.name);
            Assert.AreEqual("50000", data.salary);
            Console.WriteLine(response.Content);
        }
    }
}
