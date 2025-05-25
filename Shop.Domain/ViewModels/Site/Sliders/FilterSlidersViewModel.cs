using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Pageing;

namespace Shop.Domain.ViewModels.Site.Sliders
{
    public class FilterSlidersViewModel:BasePageing
    {

        #region Properties
        public string SliderTitle { get; set; }
        public List<Slider> Sliders { get; set; }
        #endregion


        #region Methods
        public FilterSlidersViewModel SetSliders(List<Slider> sliders)
        {
            Sliders = sliders;
            return this;
        }
        public FilterSlidersViewModel SetPageing(BasePageing pageing)
        {
            PageId = pageing.PageId;
            EndPage = pageing.EndPage;
            StartPage = pageing.StartPage;
            AllEntityCount = pageing.AllEntityCount;
            CountForShowBeforeAndAfter = pageing.CountForShowBeforeAndAfter;
            TakeEntity = pageing.TakeEntity;
            SkipEntity = pageing.SkipEntity;
            PageCount = pageing.PageCount;
            return this;
        }
        #endregion
    }
}
