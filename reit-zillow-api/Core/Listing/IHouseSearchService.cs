using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Listing
{
    public interface IHouseSearchService
    {
        public Task<IList<string>> SearchListings(int zipCode);
    }
}
