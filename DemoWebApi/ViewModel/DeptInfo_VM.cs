using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoWebApi.ViewModel
{
    public class DeptInfo_VM
    { 
        [Key]
        public string Name { get; set; }
        public int? Count { get; set; }
        public int? TotalSalary { get; set; }
    }
}
