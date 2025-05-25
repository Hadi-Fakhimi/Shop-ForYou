using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Pageing
{
    public static class PageingExtensions
    {
        public static IQueryable<T> Pageing<T>(this IQueryable<T> query, BasePageing basePageing)
        {
            return query.Skip(basePageing.SkipEntity).Take(basePageing.TakeEntity);
        }
    }
}
