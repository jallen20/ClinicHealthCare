using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class PatientLabTests
    {
        public int OrderId { get; set; }

        public DateTime Order_Date { get; set; }

        public DateTime Due_Date { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Result { get; set; }
    }
}
