
using Application.Common.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.DTOs.Category
{
    public class CategorySearchModel : PageModel
    {
        public int? ParentID { get; set; }
        public string? SeachPhrase { get; init; }
    }
}
