using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Security
{
    public class UpdateMeModel
    {
        //public int Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CurrentPassword{ get; set; }
        public string NewPassword { get; set; }


       
    }
}
