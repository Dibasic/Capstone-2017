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
    /// Christian Lopez
    /// Created on 2017/01/31
    /// 
    /// Interaction logic for AddSupplier.xaml
    /// </summary>
    public partial class frmAddSupplier : Window
    {

        User _currentUser;
        Supplier _supplierToEdit;
        IUserManager _userManager;
        ISupplierManager _supplierManager;
        IProductManager _productManager;
        IAgreementManager _agreementManager;
        bool supplierFound = false;
        string _type;
        List<Product> _notAgreedProducts;
        List<Product> _agreedProducts = new List<Product>();
        List<Agreement> _agreements;
        List<string> _usernames;

        /// <summary>
        /// Christian Lopez
        /// Created on 2017/01/31
        /// 
        /// Constructor for the form
        /// 
        /// Update
        /// Bobby Thorne
        /// 5/11/2017
        /// 
        /// Hide the submit button when in view mode
        /// </summary>
        /// <param name="currentEmp">The current User</param>
        /// <param name="userManager">Something implementing the IUserManager interface</param>
        /// <param name="supplierManager">Something implementing the ISupplierManager interface</param>
        /// <remarks>Last modified by Christian Lopez 2017/03/08</remarks>
        public frmAddSupplier(User currentUser, IUserManager userManager, ISupplierManager supplierManager,
                IProductManager productManager, IAgreementManager agreementManager, string type = "Adding", Supplier supplierToEdit = null)
        {
            _currentUser = currentUser;
            _supplierToEdit = supplierToEdit;
            _userManager = userManager;
            _supplierManager = supplierManager;
            _productManager = productManager;
            _agreementManager = agreementManager;
            InitializeComponent();
            if (null == _supplierToEdit)
            {
                _type = type;
            }
            else if (type == "Editing")
            {
                _type = type;
                supplierFound = true;
            }
            else if (type == "Viewing")
            {

                _type = type;
                supplierFound = true;
                btnSubmit.Visibility = Visibility.Collapsed;
                btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
                
            }
            
            _usernames = _userManager.RetrieveFullUserList().Select(u => u.UserName).ToList();
            cboUsername.ItemsSource = _usernames;
        }

        /// <summary>
        /// Christian Lopez
        /// Created: 2017/02/22
        /// 
        /// Looks up a Supplier Username.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLookup_Click(object sender, RoutedEventArgs e)
        {
            // Set up variables to use
            User supplierUser = null;
            resetForm();


            try
            {
                supplierUser = _userManager.RetrieveUserByUserName((string)cboUsername.SelectedItem);
            }
            catch (Exception ex)
            {

                if (null != ex.InnerException)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
                return;
            }

            // Check whether or not we actually got a user
            if (supplierUser == null)
            {
                MessageBox.Show("Unable to find user: " + (string)cboUsername.SelectedItem);
            }
            else
            {
                supplierFound = true;

                // Fill in tables with the suppliers information that will carry over from user
                // (name, phone for double checking) and assume the user's address
                // will be the same as the farm address, but that can be changed
                txtName.Text = supplierUser.FirstName + " " + (supplierUser.LastName ?? "");
                txtFarmCity.Text = supplierUser.City ?? "";
                // Need to get index to populate the correct drop down with
                // the user's state
                if (getDropdown(supplierUser.State) != -1)
                {
                    cboFarmState.SelectedIndex = getDropdown(supplierUser.State);
                }
                else
                {
                    MessageBox.Show("Cannot find user's state!");
                    cboFarmState.SelectedIndex = 0;
                }
                txtFarmAddress.Text = supplierUser.AddressLineOne + " " + (supplierUser.AddressLineTwo ?? "");

                // Let the _employee modify the form
                txtPhone.Text = supplierUser.Phone;
                txtFarmAddress.IsEnabled = true;
                txtFarmCity.IsEnabled = true;
                txtFarmName.IsEnabled = true;
                txtFarmTaxId.IsEnabled = true;
                cboFarmState.IsEnabled = true;
                productSection.IsEnabled = true;
                chkActive.IsEnabled = true;
                foreach (Product p in _agreedProducts)
                {
                    _notAgreedProducts.Remove(p);
                }
                dgAvailableProducts.ItemsSource = _notAgreedProducts;
                dgApprovedProducts.ItemsSource = _agreedProducts;

            }

        }

        /// <summary>
        /// Alissa Duffy
        /// Updated: 2017/04/24
        /// 
        /// Resets Add Supplier form.
        /// Standardized method.
        /// </summary>
        private void resetForm()
        {
            txtName.Text = "";
            txtFarmAddress.Text = "";
            txtFarmAddress.IsEnabled = false;
            txtFarmCity.Text = "";
            txtFarmCity.IsEnabled = false;
            txtPhone.Text = "";
            txtFarmName.Text = "";
            txtFarmName.IsEnabled = false;
            txtFarmTaxId.Text = "";
            txtFarmTaxId.IsEnabled = false;
            cboFarmState.SelectedIndex = 0;
            cboFarmState.IsEnabled = false;
            chkActive.IsChecked = true;
            chkActive.IsEnabled = false;
            supplierFound = false;
            productSection.IsEnabled = false;
            dgAvailableProducts.ItemsSource = null;
        }


        private static readonly string[] States = {"AK", "AL", "AR", "AZ", "CA", "CO", "CT", "DE", "FL", "GA", "HI", "IA", "ID", "IL",
                              "IN", "KS", "KY", "LA", "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND",
                              "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "RI", "SC", "SD", "TN",
                              "TX", "UT", "VA", "VT", "WA", "WI", "WV", "WY"};

        /// <summary>
        /// Christian Lopez
        /// Created: 2017/02/22
        /// 
        /// Create an array of state abbreviations
        /// </summary>
        /// 
        /// <remarks>
        /// Aaron Usher
        /// Updated: 2017/04/27
        /// 
        /// Moved constant string array outside of method and made it effectively a constant, 
        /// so it wouldn't be recreated every time the method was called.
        /// </remarks>
        /// 
        /// <param name="stateAbr"></param>
        /// <returns></returns>
        private int getDropdown(string stateAbr)
        {

            // The way to get the index for the drop down combo box
            return binarySearchStates(stateAbr, 0, States.Length, States);
        }

        /// <summary>
        /// Chrsitian Lopez
        /// Created: 2017/02/22
        /// 
        /// Recursion binary search
        /// </summary>
        /// <param name="wantedState"></param>
        /// <param name="first"></param>
        /// <param name="arrayLength"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        private int binarySearchStates(string wantedState, int first, int arrayLength, string[] states)
        {
            int mid = (arrayLength / 2) + first;
            if (arrayLength == 0)
            {
                return -1;
            }
            else
            {
                if (states[mid].CompareTo(wantedState) < 0)
                {
                    mid = binarySearchStates(wantedState, mid + 1, (arrayLength - 1) / 2, states);
                }
                else if (states[mid].CompareTo(wantedState) > 0)
                {
                    mid = binarySearchStates(wantedState, first, arrayLength / 2, states);
                }
                return mid;
            }
        }

        /// <summary>
        /// Christian Lopez
        /// Created: 2017/01/31
        /// 
        /// Handles logic of sending data to manager
        /// </summary>
        /// 
        /// <remarks>
        /// Aaron Usher
        /// Updated: 2017/04/27
        /// 
        /// Changed method call of _supplierManager.CreateSupplier to just send a supplier,
        /// instead of the information piece by piece.
        /// </remarks>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Last modified 2017/03/09 by Skyler Hiscock</remarks>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // The process of submitting information to make a supplier in the DB

            // See if we even have a user found for the supplier
            if (!supplierFound && null == _supplierToEdit)
            {
                MessageBox.Show("Please select a supplier.");
            }
            else
            {
                if (_type.Equals("Adding"))
                {
                    try
                    {
                        validateInputs();
                        User supplierUser = _userManager.RetrieveUserByUserName((string)cboUsername.SelectedItem);
                        Supplier supplier = new Supplier()
                        {
                            UserId = supplierUser.UserId,
                            Active = chkActive.IsChecked.Value,
                            ApprovedBy = _currentUser.UserId,
                            FarmName = txtFarmName.Text,
                            FarmAddress = txtFarmAddress.Text,
                            FarmCity = txtFarmCity.Text,
                            FarmState = cboFarmState.Text,
                            FarmTaxID = txtFarmTaxId.Text
                        };
                        // Actually try to create the supplier
                        if (_supplierManager.CreateNewSupplier(supplier))
                        {
                            //this.DialogResult = true;
                            try
                            {
                                addAgreedProducts(supplierUser);
                                this.DialogResult = true;
                            }
                            catch (Exception ex)
                            {

                                if (null != ex.InnerException)
                                {
                                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            // If an error was thrown, should go to catch block. This would occur if the number of rows
                            // affected were not one
                            MessageBox.Show("There was an error, where more than one row was affected! Please " +
                                "contact your Database Admin.");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (null != ex.InnerException)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else if (_type.Equals("Applying"))
                {
                    try
                    {
                        validateInputs();
                        User supplierUser = _userManager.RetrieveUserByUserName((string)cboUsername.SelectedItem);

                        // Actually try to create the supplier
                        if (_supplierManager.ApplyForSupplierAccount(new Supplier()
                        {
                            UserId = supplierUser.UserId,
                            FarmName = txtFarmName.Text,
                            FarmAddress = txtFarmAddress.Text,
                            FarmCity = txtFarmCity.Text,
                            FarmState = cboFarmState.Text,
                            FarmTaxID = txtFarmTaxId.Text
                        }))

                        //supplierUser.UserId, txtFarmName.Text, txtFarmAddress.Text, txtFarmCity.Text,
                        //cboFarmState.Text, txtFarmTaxId.Text
                        {
                            //this.DialogResult = true;
                            try
                            {
                                addAgreedProducts(supplierUser);
                                this.DialogResult = true;
                            }
                            catch (Exception ex)
                            {

                                if (null != ex.InnerException)
                                {
                                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            // If an error was thrown, should go to catch block. This would occur if the number of rows
                            // affected were not one
                            MessageBox.Show("There was an error, where more than one row was affected! Please " +
                                "contact your Database Admin.");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (null != ex.InnerException)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
                else if (_type.Equals("Editing"))
                {
                    // Update suppliers approved products: check if there are agreements that have products
                    // not listed in the approved products list. If so, mark those aggrements as not approved & inactive.
                    // Then see what products in the approved list is not in the agreements, and make agreements for those products.
                    try
                    {

                        Supplier newSupplier = _supplierToEdit.Clone();
                        newSupplier.Active = (bool)chkActive.IsChecked;
                        if (!_supplierManager.UpdateSupplierAccount(_supplierToEdit, newSupplier))
                        {
                            MessageBox.Show("Unable to update supplier information.");
                        }

                        foreach (Agreement a in _agreements)
                        {
                            if (!_agreedProducts.Any(p => p.ProductId == a.ProductId)) // if we cannot find any products in the approved list with matching id
                            {
                                Agreement notApprovedAgreement =
                                    _agreementManager.MakeAgreement(a.AgreementId, a.ProductId, a.SupplierId, DateTime.Now, false, false, _currentUser.UserId);
                                if (!_agreementManager.UpdateAgreement(a, notApprovedAgreement))
                                {
                                    MessageBox.Show("Unable to update agreements");
                                }

                            }
                        }

                        getAgreements();

                        foreach (Product p in _agreedProducts)
                        {
                            if (!_agreements.Any(a => a.ProductId == p.ProductId)) // if we cannot find an agreed product in the list of agreements
                            {
                                _agreementManager.CreateAgreementsForSupplier(_supplierToEdit, p, _currentUser.UserId, true);

                            }
                        }

                        getAgreements();
                        // Update any active agreements that are not approved yet in the agreement list
                        foreach (Agreement a in _agreements)
                        {
                            if (!a.IsApproved)
                            {
                                Agreement update =
                                    _agreementManager.MakeAgreement(a.AgreementId, a.ProductId, a.SupplierId, DateTime.Now, true, a.Active, _currentUser.UserId);
                                if (!_agreementManager.UpdateAgreement(a, update))
                                {
                                    MessageBox.Show("Unable to update agreement for " + _agreedProducts.First(p => p.ProductId == a.ProductId).Name);
                                }
                            }
                        }

                        this.DialogResult = true;
                    }
                    catch (Exception ex)
                    {
                        if (null != ex.InnerException)
                        {
                            MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                        }
                        else
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Christian Lopez
        /// Created: 2017/03/08
        /// 
        /// Adds Agreed Products.
        /// </summary>
        /// <param name="supplierUser"></param>
        private void addAgreedProducts(User supplierUser)
        {
            foreach (Product p in _agreedProducts)
            {

                try
                {
                    if (_type.Equals("Adding"))
                    {
                        _agreementManager.CreateAgreementsForSupplier(_supplierManager.RetrieveSupplierByUserId(supplierUser.UserId), p, _currentUser.UserId);
                    }
                    else if (_type.Equals("Applying"))
                    {
                        _agreementManager.CreateAgreementsForSupplier(_supplierManager.RetrieveSupplierByUserId(supplierUser.UserId), p);
                    }

                }
                catch (Exception ex)
                {

                    throw new ApplicationException("Could not store " + p.Name + " as an agreement. Error: " + ex.Message, ex.InnerException);
                }
            }
        }
        /// <summary>
        /// Ariel Sigo
        /// Updated: 2017/04/07
        /// 
        /// </summary>
        /// <summary>
        /// Alissa Duffy
        /// Updated: 2017/04/24
        /// 
        /// Validates User Input.
        /// Standardized method.
        /// </summary>
        private void validateInputs()
        {
            if (txtFarmName.Text.Length == 0)
            {
                throw new ApplicationException("Please enter a farm name.");
            }
            if (txtFarmAddress.Text.Length == 0)
            {
                throw new ApplicationException("Please enter a farm address.");
            }
            if (txtFarmCity.Text.Length == 0)
            {
                throw new ApplicationException("Please enter a farm city.");
            }
            try
            {
                Int32.Parse(txtFarmTaxId.Text);
            }
            catch (Exception)
            {

                throw new ApplicationException("Please enter a valid farm tax ID");
            }
            if (txtFarmTaxId.Text.Length != 9)
            {
                throw new ApplicationException("Farm Tax ID Must Be 9 Digits");

            }
        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the username is changed, cannot guarantee that the information is
            // correct. Reset by clicking the lookup button.
            supplierFound = false;
        }

        /// <summary>
        /// Christian Lopez
        /// 
        /// Laura Simmonds
        /// Updated: 2017/04/14
        /// 
        /// Sets the screen depending on the type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_type.Equals("Adding"))
            {
                btnSubmit.Content = "Submit";
                this.Title = "Create Supplier";
            }
            else if (_type.Equals("Applying"))
            {
                btnSubmit.Content = "Apply";
                this.Title = "Apply for Account";
            }
            else if (_type.Equals("Viewing"))
            {
                try
                {
                    this.Title = "View Application Details";
                    cboUsername.IsEnabled = false;
                    btnSubmit.IsEnabled = false;
                    User supplierUser = _userManager.RetrieveUser(_supplierToEdit.UserId);
                    cboUsername.Text = supplierUser.UserName;
                    txtName.Text = supplierUser.FirstName + " " + supplierUser.LastName;
                    txtPhone.Text = supplierUser.Phone;
                    txtFarmName.Text = _supplierToEdit.FarmName;
                    txtFarmAddress.Text = _supplierToEdit.FarmAddress;
                    txtFarmCity.Text = _supplierToEdit.FarmCity;
                    txtFarmTaxId.Text = _supplierToEdit.FarmTaxID;
                    cboFarmState.SelectedIndex = getDropdown(_supplierToEdit.FarmState);
                    productSection.IsEnabled = false;
                    chkActive.IsChecked = _supplierToEdit.Active;
                    chkActive.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
            }
            else if (_type.Equals("Editing"))
            {
                try
                {
                    cboUsername.IsEnabled = false;
                    btnSubmit.Content = "Update";
                    User supplierUser = _userManager.RetrieveUser(_supplierToEdit.UserId);
                    cboUsername.Text = supplierUser.UserName;
                    txtName.Text = supplierUser.FirstName + " " + supplierUser.LastName;
                    txtPhone.Text = supplierUser.Phone;
                    txtFarmName.Text = _supplierToEdit.FarmName;
                    txtFarmAddress.Text = _supplierToEdit.FarmAddress;
                    txtFarmCity.Text = _supplierToEdit.FarmCity;
                    txtFarmTaxId.Text = _supplierToEdit.FarmTaxID;
                    cboFarmState.SelectedIndex = getDropdown(_supplierToEdit.FarmState);
                    productSection.IsEnabled = true;
                    chkActive.IsChecked = _supplierToEdit.Active;
                    chkActive.IsEnabled = true;
                }
                catch (Exception ex)
                {

                    if (null != ex.InnerException)
                    {
                        MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            try
            {
                _notAgreedProducts = _productManager.RetrieveProducts();
                if (_type.Equals("Adding") || _type.Equals("Applying"))
                {
                    //If adding or applying for an account, then there cannot be any already approved products.
                    _agreedProducts = new List<Product>();
                }
                else if (null != _supplierToEdit)
                {
                    getAgreements();
                    foreach (Agreement a in _agreements)
                    {
                        _agreedProducts.Add(_productManager.RetrieveProductById(a.ProductId));
                        _notAgreedProducts.RemoveAll(p => p.Name == _productManager.RetrieveProductById(a.ProductId).Name);
                    }
                    refreshTables();
                }
            }
            catch (Exception ex)
            {

                if (null != ex.InnerException)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }

        /// <summary>
        /// Christian Lopez
        /// Created: 2017/03/09
        /// 
        /// Retrieves Supplier Agreements.
        /// </summary>
        private void getAgreements()
        {
            try
            {
                _agreements = _agreementManager.RetrieveAgreementsBySupplierId(_supplierToEdit.SupplierID);
            }
            catch (Exception ex)
            {

                if (null != ex.InnerException)
                {
                    MessageBox.Show(ex.Message + "\n\n" + ex.InnerException.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        /// <summary>
        /// Christian Lopez
        /// Created: 2017/03/08
        /// 
        /// Logic to take an product from the not approved list to the approved list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddToApproved_Click(object sender, RoutedEventArgs e)
        {
            if (0 > dgAvailableProducts.SelectedIndex)
            {
                MessageBox.Show("Please select a Product to add!");
            }
            else
            {
                _agreedProducts.Add((Product)(dgAvailableProducts.SelectedItem));
                _notAgreedProducts.Remove((Product)(dgAvailableProducts.SelectedItem));
                refreshTables();
                dgAvailableProducts.SelectedIndex = -1;
            }
        }


        /// <summary>
        /// Alissa Duffy
        /// Updated: 2017/04/24
        /// 
        /// Removes Products from Approved.
        /// Standardized method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveFromApproved_Click(object sender, RoutedEventArgs e)
        {
            if (0 > dgApprovedProducts.SelectedIndex)
            {
                MessageBox.Show("Please select a Product to remove!");
            }
            else
            {
                _agreedProducts.Remove((Product)(dgApprovedProducts.SelectedItem));
                _notAgreedProducts.Add((Product)(dgApprovedProducts.SelectedItem));
                refreshTables();
                dgApprovedProducts.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Alissa Duffy
        /// Updated: 2017/04/24
        /// 
        /// Refreshed Available and Approved Tables.
        /// Standardized method.
        /// </summary>
        private void refreshTables()
        {
            dgAvailableProducts.ItemsSource = null;
            dgApprovedProducts.ItemsSource = null;
            dgAvailableProducts.ItemsSource = _notAgreedProducts;
            dgApprovedProducts.ItemsSource = _agreedProducts;
        }

        /// <summary>
        /// Ryan Spurgetis
        /// Created: 2017/04/17
        /// 
        /// The submit agreement button is only visible when approving suppliers, 
        /// it sends new agreement request to AgreementManager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitAgreement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Product p in _agreedProducts)
                {
                    _agreementManager.CreateAgreementsForSupplier(_supplierToEdit, p, _currentUser.UserId, true);
                    Close();
                }
                MessageBox.Show("Agreement created.");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Unable to create agreement: " + ex.Message + ex.StackTrace); ;
            }
        }

        private void cboUsername_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnLookup_Click(sender, e);
        }
    } // End of class
} // End of namespace
