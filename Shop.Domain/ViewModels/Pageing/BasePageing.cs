using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Pageing
{
    public class BasePageing
    {
        public BasePageing()
        {
            PageId = 1;
            TakeEntity = 5;
            CountForShowBeforeAndAfter = 15;
        }
        public int PageId { get; set; }
        public int PageCount { get; set; }
        public int AllEntityCount { get; set; }
        public int CountForShowBeforeAndAfter { get; set; }
        public int SkipEntity { get; set; }
        public int TakeEntity { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }


        public BasePageing GetCurrentPaging()
        {
            return this;
        }
    }

}
