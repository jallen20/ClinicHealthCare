using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class Patient
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public string SSN { get; set; }

        public DateTime DOB { get; set; }

        public string ShortDOB => this.DOB.ToShortDateString();

        public string Sex { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public string Phone { get; set; }

        public Patient() { }

        public Patient(string firstName, string lastName, string ssn, DateTime dob, string sex, string address, string city, string state, int zip, string phone)
        {
            if (ssn.Length != 9)
            {
                throw new ArgumentNullException("Invalid SSN!");
            }

            if (Math.Abs(Math.Floor(Math.Log10(zip) + 1) - 5) > 0.01)
            {
                throw new ArgumentException("Zip code must be 5 digits.");
            }

            if (phone.Length != 10)
            {
                throw new ArgumentException("Phone number must be 10 digits.");
            }
           
            this.FirstName = string.IsNullOrEmpty(firstName) ? throw new ArgumentException("Invalid First Name!") : firstName;
            this.LastName = string.IsNullOrEmpty(lastName) ? throw new ArgumentException("Invalid Last Name") : lastName;
            this.SSN = string.IsNullOrEmpty(ssn) ? throw new ArgumentException("Invalid SSN!") : ssn;
            this.Phone = string.IsNullOrEmpty(phone) ? throw new ArgumentException("Invalid Phone NUmber!") : phone;
            this.Address = string.IsNullOrEmpty(address) ? throw new ArgumentException("Invalid Address!") : address;
            this.City = string.IsNullOrEmpty(city) ? throw new ArgumentException("Invalid City!") : city;
            this.State = string.IsNullOrEmpty(state) ? throw new ArgumentException("Invalid State!") : state;
            this.Zip = zip;
            this.Sex = string.IsNullOrEmpty(sex) ? throw new ArgumentException("Invalid Sex!") : sex;
            this.DOB = dob.Date;

        }
    }
}
