using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicHealthcareSystem.Models
{
    public class LabTest
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }

        public LabTest() { }

        public LabTest(string code, string name)
        {
            this.Code = code;
            this.Name = name;
        }
    }
}
