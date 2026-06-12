using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.BaseModel
{
    public class GenericComplexResult<TSearchModel,TListItem>
    {
        public TSearchModel sm { get; set; }
        public List<TListItem> ListItems { get; set; }
    }
}
