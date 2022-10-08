using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Dto;

namespace Core.Listing
{
    public interface IListingParser
    {
        ListingDetail Parse(string html);
    }
}
