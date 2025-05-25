using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Site
{
    public class Slider:BaseEntity
    {
        public string SliderImageName { get; set; }
        public string SliderTitle { get; set; }
        public string SliderText { get; set; }
        public string SliderPrice { get; set; }
        public string SliderHref { get; set; }
        public string SliderTextButton { get; set; }


    }
}
