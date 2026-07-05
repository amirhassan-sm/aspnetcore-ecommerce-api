using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.BaseModel
{
    public class PageModel
    {
        public int PageIndex { get; set; }
        //private int _PageSize { get; set; }

        //public int pageSize
        //{
        //    get => _PageSize == 0 ? _PageSize : 10; 
        //    set => _PageSize = (value == 0 ? value : 10);  
           
        //}


        private int _pageSize;

        public int pageSize
        {
            get => _pageSize == 0 ? 10 : _pageSize; // if _pageSize is 0, return 10, otherwise return the value
            set => _pageSize = (value == 0 ? 10 : value); // if value is 0, store 10, otherwise store the value
        }





        private int _recordCount { get; set; }
        public int RecordCount { get => _recordCount; set => _recordCount = value; }

        
        public int PageCount
        {
            get
            {   
                
              
                if (RecordCount % pageSize == 0)
                {
                    return RecordCount / pageSize;
                }
                else { return (RecordCount / pageSize) + 1; }
            }
        }



    }
}
