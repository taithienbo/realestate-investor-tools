﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CashFlow
{
    public interface ICashFlowService
    {
        public Task<double> CalculateCashFlow(string address);
    }
}
