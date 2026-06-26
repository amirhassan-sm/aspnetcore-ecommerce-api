using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Identity
{
    public  class AppliactionUserRole
    {
        public int AppliactionUserRoleId { get; set; }
        public int ApplicationUserId { get; set; }

        public int ApplicationRoleId { get; set; }
        public ApplicationRole applicationRole { get; set; }
        public ApplicationUser applicationUser { get; set; }


    }
    
}
