using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Zillow
{
    public interface IZillowClient
    {
        Task<string> GetListingHtmlPage(string address);
        Task<string> GetPriceMyRentalHtmlPage(string address);
    }
}
