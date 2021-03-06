using System;
﻿using DataObjects;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LogicLayer
{
    /// <summary>
    /// Ethan Jorgensen
    /// Created on 2017/04/13
    /// 
    /// Manages the logic regarding supplier lots
    /// </summary>
    public class SupplierProductLotManager : ISupplierProductLotManager
    {
        public bool CreateSupplierProductLot(SupplierProductLot SupplierProductLot)
        {
            bool result = false;
            try
            {
                result = (1 == SupplierProductLotAccessor.CreateSupplierProductLot(SupplierProductLot));
            }
            catch
            {
                throw;
            }
            return result;
        }


        public bool AddProduct(SupplierProductLot p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ethan Jorgensen
        /// 2017/03/23
        /// 
        /// Sets a product lot inactive.
        /// </summary>
        /// <returns></returns>
        public bool DeleteSupplierProductLot(SupplierProductLot lot)
        {
            bool result = false;

            result = 1 == SupplierProductLotAccessor.DeleteSupplierProductLot(lot);

            return result;
        }

        /// <summary>
        /// Laura Simmonds
        /// Created: 2017/05/05
        /// 
        /// Gets a list of product lots from the database
        /// </summary>
        /// <returns></returns>
        public List<SupplierProductLot> RetrieveSupplierProducts(int supplierId)
        {
            List<SupplierProductLot> lots = null;
            try
            {
                lots = SupplierProductLotAccessor.RetrieveSupplierProducts(supplierId);
                IProductManager productManager = new ProductManager();
                foreach (var lot in lots)
                {
                    var productInLot = productManager.RetrieveProductById((int)lot.ProductId);
                }
            }
            catch (SqlException ex)
            {

                throw new ApplicationException("There was a database error.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was an unknown error.", ex);
            }

            return lots;
        }


        /// <summary>
        /// Ethan Jorgensen
        /// Created on 2017/04/13
        /// 
        /// Retrieves a list of product lots from a given supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public List<SupplierProductLot> RetrieveSupplierProductLotsBySupplier(Supplier supplier)
        {
            List<SupplierProductLot> lots = null;
            try
            {
                lots = SupplierProductLotAccessor.RetrieveSupplierProductLotsBySupplier(supplier);
            }
            catch (SqlException ex)
            {

                throw new ApplicationException("There was a database error.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was an unknown error.", ex);
            }
            return lots;
        }

        /// <summary>
        /// Ethan Jorgensen
        /// Created on 2017/04/13
        /// 
        /// Returns a product lot associated with that id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SupplierProductLot RetrieveSupplierProductLotById(int id)
        {
            try
            {
                SupplierProductLot lot = SupplierProductLotAccessor.RetrieveSupplierProductLot(id);
                return lot;
            }
            catch (SqlException ex)
            {

                throw new ApplicationException("There was a database error.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("There was an unknown error.", ex);
            }

        }

        /// <summary>
        ///  Ethan Jorgensen
        ///  
        /// 05/09/2017
        /// </summary>
        /// <param name="oldLot"></param>
        /// <param name="newLot"></param>
        public bool UpdateSupplierProductLot(SupplierProductLot oldLot, SupplierProductLot newLot)
        {
            return SupplierProductLotAccessor.UpdateSupplierProductLot(oldLot, newLot);
        }
    }
}
