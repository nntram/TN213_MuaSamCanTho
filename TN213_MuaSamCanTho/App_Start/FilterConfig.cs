using System.Web;
using System.Web.Mvc;

namespace TN213_MuaSamCanTho
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
