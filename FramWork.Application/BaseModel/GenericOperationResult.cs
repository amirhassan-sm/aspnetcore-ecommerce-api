using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.BaseModel
{
        public class GenericOperationResult<TItems>
        {
            public int? RecordId { get; private set; }
       

            public bool Success { get; private set; } = false;

            public string Message { get; private set; }
            public  List<string> Errors { get;private set; } = new List<string>();

            public List<TItems> Items { get; private set; }


            public DateTime OperationDate { get; private set; } = DateTime.UtcNow;

            private GenericOperationResult( int? RecordId, string messege,bool success,List<string> Errors, List<TItems> item)
            {
                this.RecordId = RecordId;
                this.Message = messege;
                this.Success = success;
                this.Errors = Errors;
                this.Items =item;

            }
         

            public static GenericOperationResult<TItems> ToSuccess(int RecordId, string messege , List<TItems> items)
            {
                return new GenericOperationResult<TItems>(RecordId, messege, true, new List<string>(), items );

            }

            public static GenericOperationResult<TItems> ToFail(int RecordId, string messege, List<string> Errors)
            {

                return new GenericOperationResult<TItems>(RecordId,messege,false, Errors,new List<TItems>());

            }
        public static GenericOperationResult<TItems> ToFail( string messege, List<string> Errors)
        {

            return new GenericOperationResult<TItems>(null, messege, false, Errors, new List<TItems>());

        }




    }
}
