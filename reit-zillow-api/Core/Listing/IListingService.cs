﻿using Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Listing
{
    public interface IListingService
    {
        public Task<ListingDetail> GetListingDetail(string address);
    }
}
