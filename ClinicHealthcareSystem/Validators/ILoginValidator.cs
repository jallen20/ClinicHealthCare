using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicHealthcareSystem.Models;

namespace ClinicHealthcareSystem.Validators
{
    public interface ILoginValidator
    {

        Task<bool> IsValid();

        Task<User> GetUser();

    }
}
