﻿using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPresentationLayer
{
    /// <summary>
    /// Interaction logic for frmProductOrderDetails.xaml
    /// </summary>
    public partial class frmProductOrderDetails : Window
    {   
        /// <summary>
        /// Victor Algarin
        /// 
        /// Alissa don't remove my comments and putting YOUR name from code that you did not work on!!!
        /// Updated: 2017/04/30
        /// 
        /// Initialize Product Order Details Window.
        /// Standaridized method.
        /// </summary>
        public frmProductOrderDetails()
        {
            InitializeComponent();
        }

        int orderID;
        /// <summary>
        /// Victor Algarin
        /// 
        /// Alissa don't remove my comments and putting YOUR name from code that you did not work on!!!
        /// Updated: 2017/04/30
        /// 
        /// Initialize Product Order Details Window.
        /// Standaridized method.
        /// </summary>
        /// <param name="orderID"></param>
        public frmProductOrderDetails(int orderID)
        {
            InitializeComponent();
            this.orderID = orderID;
            displayDetails();
        }

        ProductOrder _prodOrd = new ProductOrder();
        IProductOrderManager _ordMgr = new ProductOrderManager();

        /// <summary>
        /// Victor Algarin
        /// 
        /// Alissa don't remove my comments and putting YOUR name from code that you did not work on!!!
        /// Updated: 2017/04/30
        /// 
        /// 
        /// Displays Product Order Details.
        /// Standaridized method.
        /// </summary>
        public void displayDetails()
        {
            try
            {
                _prodOrd = _ordMgr.retrieveProductOrderDetails(orderID);
                txtOrderId.Text = _prodOrd.OrderId.ToString();
                txtCustomerId.Text = _prodOrd.CustomerId.ToString();
                txtOrderType.Text = _prodOrd.OrderTypeId;
                txtAddressType.Text = _prodOrd.AddressType;
                txtDeliveryType.Text = _prodOrd.DeliveryTypeId;
                txtAmount.Text = _prodOrd.Amount.ToString();
                txtOrderDate.Text = _prodOrd.OrderDate.ToString();
                txtExpectedDate.Text = _prodOrd.DateExpected.ToString();
                txtDiscount.Text = _prodOrd.Discount.ToString();
                txtOrderStatus.Text = _prodOrd.OrderStatusId;
                txtUserAddress.Text = _prodOrd.UserAddressId.ToString();
                txtHasArrived.Text = _prodOrd.HasArrived.ToString();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Victor Algarin
        /// 
        /// Alissa don't remove my comments and putting YOUR name from code that you did not work on!!!
        /// Updated: 2017/04/30
        /// 
        /// 
        /// Save Packaging Changes.
        /// Standaridized method. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPackaging_Click(object sender, RoutedEventArgs e)
        {
            frmProductOrderPackages pakMgr = new frmProductOrderPackages(this.orderID);
            pakMgr.ShowDialog();
        }

        /// <summary>
        /// Victor Algarin
        /// 
        /// Alissa don't remove my comments and putting YOUR name from code that you did not work on!!!
        /// Updated: 2017/04/30
        /// 
        /// 
        /// Closes Add Product Lot Window.
        /// Standaridized method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
