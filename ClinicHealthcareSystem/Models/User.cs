using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
   public class User
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName => $"{this.FirstName} {this.LastName}";

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>
        /// The SSN.
        /// </value>
        public string SSN { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime DOB { get; set; }

        /// <summary>
        /// Gets the short dob.
        /// </summary>
        /// <value>
        /// The short dob.
        /// </value>
        public string ShortDOB => this.DOB.ToShortDateString();

        /// <summary>
        /// Gets or sets the sex.
        /// </summary>
        /// <value>
        /// The sex.
        /// </value>
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [logged in].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [logged in]; otherwise, <c>false</c>.
        /// </value>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the user role i.e. Doctor, Admin, Nurse.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="ssn">The SSN.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="sex">The sex.</param>
        /// <param name="address">The address.</param>
        /// <param name="phone">The phone.</param>
        /// <exception cref="ArgumentNullException">
        /// Invalid SSN!
        /// or
        /// DOB cannot be null
        /// or
        /// id
        /// or
        /// firstName
        /// or
        /// lastName
        /// or
        /// phone
        /// or
        /// address
        /// or
        /// sex
        /// </exception>
        public User(string id, string firstName, string lastName, string ssn, DateTime dob, string sex, string address, string phone)
        {
            if (ssn.Length != 9)
            {
                throw new ArgumentException("Invalid SSN!");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("The First name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("The last name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("The address cannot be empty");
            }

            if (dob == null)
            {
                throw new ArgumentException("DOB cannot be null");
            }
            if (phone.Length != 10)
            {
                throw new ArgumentException("Phone Number Must Be 10 Digits.");
            }
            this.Id = id ?? throw new ArgumentNullException(nameof(id));
            this.FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            this.LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            this.SSN = ssn;
            this.Phone = phone ?? throw new ArgumentNullException(nameof(phone));
            this.Address = address ?? throw new ArgumentNullException(nameof(address));
            this.Sex = sex ?? throw new ArgumentNullException(nameof(sex));
            this.DOB = dob;
            this.LoggedIn = false;

        }
    }
}
