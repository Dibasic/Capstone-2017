﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    /// <summary>
    /// Created 2017-03-29 by William Flood
    /// </summary>
    public class SupplierInventory
    {
        public int? SupplierInventoryID { get; set; }
        public int? AgreementID { get; set; }
        public int? Quantity { get; set; }
        public DateTime? DateAdded { get; set; }
    }
}