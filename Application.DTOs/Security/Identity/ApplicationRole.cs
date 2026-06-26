using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Identity
{
    public class ApplicationRole : IdentityRole<int>
    {
        public int ApplicationUserRoleID { get; set; }
        public string Description { get; set; }

        public ICollection<AppliactionUserRole> appliactionUserRoles { get; set; }
    }
}
