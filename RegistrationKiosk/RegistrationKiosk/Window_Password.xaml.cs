﻿using System;
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
using System.IO;

namespace RegistrationKiosk {
    /// <summary>
    /// Interaction logic for Window_Password.xaml
    /// </summary>
    public partial class Window_Password : Window {
        
        private Window_Main main = null;
        SecurityMeans security = new SecurityMeans();

        //===========================================================================
        #region Initialize Window
        //===========================================================================
        public Window_Password(Window_Main main) {
            this.main = main;
            InitializeComponent();
            pass_PassOld.Focus();
        }
        #endregion
        //===========================================================================
        #region Window Events
        //===========================================================================

        /// <summary>
        /// Click event for Cancel button.
        /// </summary>
        private void btn_PassCancel_Click(object sender, RoutedEventArgs e) {
            main.IsEnabled = true;
            this.Close();
        }

        /// <summary>
        /// Click event for Change Password button.
        /// </summary>
        private void btn_PassOk_Click(object sender, RoutedEventArgs e) {
            try {
                // Open file and read password
                string[] oldLines = File.ReadAllLines("../../security.txt");
                string hash = oldLines[0].Substring(12);
                string passOld = pass_PassOld.Password;
                string passNew = pass_PassNew.Password;
                string passVer = pass_PassVerify.Password;
                // Verify Old Password
                if (!security.VerifyMd5Hash(passOld, hash)) {
                    MessageBox.Show("Old password invalid!");
                    pass_PassOld.Focus();
                    return;
                }
                // Verify New Password
                if (!passNew.Equals(passVer)) {
                    MessageBox.Show("New passwords don't match!");
                    pass_PassVerify.Focus();
                    return;
                }
                // Set password
                string[] newLines = {
                                         "Admin Pass: " + security.GetMd5Hash(passNew),
                                         oldLines[1], oldLines[2], oldLines[3], oldLines[4], oldLines[5]
                                     };
                File.WriteAllLines("../../security.txt", newLines);
                MessageBox.Show("Password changed!");
                btn_PassCancel_Click(sender, e);
            } catch { }
        }

        /// <summary>
        /// KeyDown event for password boxes.
        /// </summary>
        private void pass_Pass_PressEnter(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                btn_PassOk_Click(sender, e);
            }
        }
        
        #endregion
        //===========================================================================
    }
}
