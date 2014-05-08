using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Recursos_Humanos_wpf.Clases
{
    class AutoComplete
    {
        public int param { get; set; } 
        List<string> listAutocomplet = new Clases.Personal().findAll(0);
        ListBox lAutoComplete = new ListBox();
        public AutoComplete() { }
        public AutoComplete(int param) {
            this.param = param;
            listAutocomplet = new Clases.Personal().findAll(this.param);
        }

        public ListBox llenaList(string value){
            List<string> autoList = new List<string>();
            autoList.Clear();
            foreach (string item in listAutocomplet)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (item.StartsWith(value))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                this.lAutoComplete.ItemsSource = autoList;
                this.lAutoComplete.Visibility = Visibility.Visible;
            }
            else if (value.Equals(""))
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.lAutoComplete.ItemsSource = null;
            }
            else
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.lAutoComplete.ItemsSource = null;
            }
            return lAutoComplete;
        }
    }
}
