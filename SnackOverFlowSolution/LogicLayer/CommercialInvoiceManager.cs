﻿using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class CommercialInvoiceManager : ICommercialInvoiceManager
    {
        public List<CommercialInvoice> RetrieveCommercialInvoiceByUserName(string userName)
        {
            try
            {
                return CommercialInvoiceAccessor.RetrieveCommercialInvoicesByUserName(userName);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {

                throw new ApplicationException("There was a database error.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was an unknown error.", ex);
            }
        }
    }
}
