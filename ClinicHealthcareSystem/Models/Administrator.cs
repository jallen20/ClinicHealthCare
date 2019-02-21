using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClinicHealthcareSystem.Models.User" />
    public sealed class Administrator : User
    {
        /// <summary>
        /// Gets or sets the administrator identifier.
        /// </summary>
        /// <value>
        /// The administrator identifier.
        /// </value>
        public int AdministratorId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Administrator"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="ssn">The SSN.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="sex">The sex.</param>
        /// <param name="address">The address.</param>
        /// <param name="phone">The phone.</param>
        public Administrator(string id, string firstName, string lastName, string ssn, DateTime dob, string sex,
            string address, string phone) :
            base(id, firstName, lastName, ssn, dob, sex, address, phone)
        {
            this.Role = "Administrator";
        }
    }
}
