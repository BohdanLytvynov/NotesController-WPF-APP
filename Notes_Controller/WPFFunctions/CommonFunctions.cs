using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notes_Controller.WPFFunctions
{
    public static class CommonFunctions
    {
        public static Window DispWindow { get; set; }

        public static MessageBoxResult ShowMessageBox(string message,
            MessageBoxButton buttons, MessageBoxImage image)
        {           
            return  MessageBox.Show(message, "Notes_Controller_Api", buttons,
                image);  
        }
    }
}
