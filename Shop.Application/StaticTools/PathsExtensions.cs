using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.StaticTools
{
    public static class PathsExtensions
    {
        #region Default Avatar
        public static string UserDefaultOrigin = "/img/userDefault/origin/";
        #endregion

        #region UserAvatar
        public static string UserAvatarOrigin = "/img/userAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/userAvatar/origin/");

        public static string UserAvatarThumb = "/img/userAvatar/thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/userAvatar/thumb/");
        #endregion

        #region ImageCategory

        public static string ImageCategoryOrigin = "/img/imageCategory/origin/";
        public static string ImageCategoryOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/imageCategory/origin/");

        public static string ImageCategoryThumb = "/img/imageCategory/thumb/";
        public static string ImageCategoryThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/imageCategory/thumb/");
        #endregion

        #region Image Slider


        public static string ImageSliderOrigin = "/img/imageSlider/origin/";
        public static string ImageSliderOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/imageSlider/origin/");

        public static string ImageSliderThumb = "/img/imageSlider/thumb/";
        public static string ImageSliderThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/imageSlider/thumb/");


        #endregion

        #region Products

        public static string ImageProductOrigin = "/img/imageProduct/origin/";
        public static string ImageProductOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/imageProduct/origin/");

        public static string ImageProductThumb = "/img/imageProduct/thumb/";
        public static string ImageProductThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/imageProduct/thumb/");
        #endregion
    }
}
