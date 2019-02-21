using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using ClinicHealthcareSystem.DAL;
using ClinicHealthcareSystem.Models;

namespace ClinicHealthcareSystem.Validators
{
    /// <summary>
    ///     Validates login credentials
    /// </summary>
    public class AdminLoginValidator : ILoginValidator
    {
        private readonly string id;
        private readonly string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="NurseLoginValidator"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="ArgumentNullException">
        /// id
        /// or
        /// password
        /// </exception>
        public AdminLoginValidator(string id, string password)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
        }

        /// <summary>
        /// Returns true if provided is and password is valid.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsValid()
        {
            var loginDal = new LoginDAL();
            return await loginDal.IsValidLogin(this.id, this.password);
        }

        /// <summary>
        /// Gets the admin with the provided id.
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetUser()
        {
            var adminDal = new AdminDAL();
            var admin = await adminDal.GetAdminByUserId(this.id);
            return admin;
        }
    }
}