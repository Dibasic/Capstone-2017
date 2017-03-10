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
    /// Interaction logic for ResetPassword.xaml
    /// </summary>
    public partial class ResetPassword : Window
    {
        List<User> _users;
        IUserManager _userManager;
        public ResetPassword(IUserManager _userManager, List<User> _users)
        {
            this._userManager = _userManager;
            InitializeComponent();
            this._users = _users;
            cbxUsers.ItemsSource = this._users;
        }

        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (1 == _userManager.ResetPassword((String)_users[cbxUsers.SelectedIndex].UserName, txtPassword.Text))
                {
                    MessageBox.Show("User Password Changed");
                }
                else
                {
                    MessageBox.Show("Change failed.  Was the user deleted?");
                }
            }
            catch
            {
                ErrorAlert.ShowDatabaseError();
            }
        }

        private void btnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = _userManager.NewPassword();
        }
    }
}
