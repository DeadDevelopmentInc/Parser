using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    internal class BufferProject
    {
        //For projects and users
        public string _id { get; set; } = Convert.ToString(Guid.NewGuid());
        //For projects and users
        public string name { get; set; }
        //For projects
        public string customer { get; set; }
        //For projects
        public string activity { get; set; }
        //For projects and users
        public string result { get; set; }
        //For users
        public string role { get; set; }
        //For users
        public string responsibility { get; set; }
        //For users
        public string startProjectDate { get; set; }
        //For users
        public string endProjectDate { get; set; }

        public DateTime startDate { get; set; }
        //For users
        public DateTime endDate { get; set; }

        public string Environment { get; set; }

        public string sourceCompany { get; set; }

        public BufferProject(
            string name, string customer, string activity, 
            string result, string role, string responsibility,
            string startProjectDate, string endProjectDate,
            string Env)
        {
            this.name = name;
            this.customer = customer;
            this.activity = activity;
            this.responsibility = responsibility;
            this.result = result;
            this.role = role;
            this.startProjectDate = startProjectDate;
            this.endProjectDate = endProjectDate;
            Environment = Env;
            GenerateDateTime();
        }

        private void GenerateDateTime()
        {
            string[] start = startProjectDate.Split(' ');
            string[] end = endProjectDate.Split(' ');
            startDate = new DateTime(int.Parse(start[1]), Month(start[0]), 01, 12, 00, 00, DateTimeKind.Utc);
            if (endProjectDate.Contains("Present") || endProjectDate.Contains("present")) { endDate = DateTime.UtcNow; }
            else { endDate = new DateTime(int.Parse(end[1]), Month(end[0]), 01, 12, 00, 00, DateTimeKind.Utc); }
            
        }

        public int Month(string month)
        {
            switch (month)
            {
                case "January": { return 01; }
                case "February": { return 02; } 
                case "March": { return 03; }  
                case "April": { return 04; }  
                case "May": { return 05; }  
                case "June": { return 06; }  
                case "July": { return 07; }  
                case "August": { return 08; }  
                case "September": { return 09; }  
                case "October": { return 10; }  
                case "November": { return 11; }  
                case "December": { return 12; }  
            }
            return 00;
        }

    }
}
