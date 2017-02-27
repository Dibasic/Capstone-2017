﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface ICustomerManager
    {
        /// <summary>
        /// Eric Walton
        /// 2017/26/02
        /// Create Commerial Account Method
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        bool CreateCommercialAccount(CommercialCustomer cc);
        /// <summary>
        /// Eric Walton
        /// 2017/26/02
        /// RetrieveCommercialCustomer method
        /// </summary>
        /// <returns></returns>
        List<CommercialCustomer> RetrieveCommercialCustomers();
    }
}
