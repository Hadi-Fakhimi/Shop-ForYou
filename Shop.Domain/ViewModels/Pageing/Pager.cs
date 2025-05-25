using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Pageing
{
    public class Pager
    {
        public static BasePageing Build(int pageId, int allEntityCount, int take, int countForShowBeforAndAfter)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntityCount / (double)take));

            return new BasePageing
            {
                PageId = pageId,
                AllEntityCount = allEntityCount,
                CountForShowBeforeAndAfter = countForShowBeforAndAfter,
                SkipEntity = (pageId - 1) * take,
                TakeEntity = take,
                StartPage = pageId - countForShowBeforAndAfter <= 0 ? 1 : pageId - countForShowBeforAndAfter,
                EndPage = pageId + countForShowBeforAndAfter > pageCount ? pageCount : pageId + countForShowBeforAndAfter,
                PageCount = pageCount
            };
        }
    }
}
