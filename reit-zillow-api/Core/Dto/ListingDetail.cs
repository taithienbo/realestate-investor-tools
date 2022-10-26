﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ListingDetail
    {
        public double ListingPrice { get; set; }
        public int NumOfBedrooms { get; set; }
        public int NumOfBathrooms { get; set; }
        public int NumOfStories { get; set; }
        public int NumOfParkingSpaces { get; set; }
        public int LotSizeInSqrtFt { get; set; }
        public int NumOfGarageSpaces { get; set; }
        public string? HomeType { get; set; }
        public string? PropertyCondition { get; set; }
        public int YearBuilt { get; set; }
        public bool HasHOA { get; set; }
        public int PropertyAge
        {
            get
            {
                return DateTime.Now.Year - this.YearBuilt;
            }
        }
    }
}
