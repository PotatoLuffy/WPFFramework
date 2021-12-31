﻿using EIPMonitor.Model.MasterData;
using EIPMonitor.ViewModel.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EIPMonitor.Views.PowerMeter
{
    /// <summary>
    /// Interaction logic for ProductionIndexQueryUserControl.xaml
    /// </summary>
    public partial class ProductionIndexQueryUserControl : UserControl
    {
        private ProductionIndexQueryUserControlViewModel productionIndexQueryUserControlViewModel;
        public ProductionIndexQueryUserControl()
        {
            InitializeComponent();
            productionIndexQueryUserControlViewModel = new ProductionIndexQueryUserControlViewModel();
            DataContext = productionIndexQueryUserControlViewModel;
        }
        private void ProductionIndexDatagrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName) && !displayName.Equals("选择"))
            {
                e.Column.Header = displayName;
            }
            else
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }
        private static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default) return displayName.DisplayName;
                return null;
            }
            else
            {
                var pi = descriptor as PropertyInfo;
                if (pi == null) return null;
                var attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                for (int ix = 0; ix < attributes.Length; ix++)
                {
                    var displayName = attributes[ix] as DisplayNameAttribute;
                    if (displayName != null && displayName != DisplayNameAttribute.Default)
                    {
                        return displayName.DisplayName;
                    }
                }
                return null;
            }
        }
        private void ProductionIndexMain_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName) && !displayName.Equals("选择"))
            {
                e.Column.Header = displayName;
            }
            else
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }
        private async void ProductionIndexMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var datagrid = e.Source as DataGrid;
            if (datagrid == null) return;
            var mapperSource = datagrid.SelectedItem as MES_MO_TO_EIP_POOL;
            if (mapperSource == null) return;
            await productionIndexQueryUserControlViewModel.GetSelectedEntryDetails(mapperSource).ConfigureAwait(true);
        }
        private async void workOrderFromTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            await productionIndexQueryUserControlViewModel.MOListQuery().ConfigureAwait(true);
        }
        private async void productIndexQueryButton_Click(object sender, RoutedEventArgs e)
        {
            await productionIndexQueryUserControlViewModel.MOListQuery().ConfigureAwait(true);
        }
        private void pastedMultipleOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string workorders = System.Windows.Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(workorders)) return;
            productionIndexQueryUserControlViewModel.CopiedOrders = workorders.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(o=>o).ToList();
            productionIndexQueryUserControlViewModel.WorkOrderFromTextBox = productionIndexQueryUserControlViewModel.CopiedOrders.DefaultIfEmpty().FirstOrDefault();
            productionIndexQueryUserControlViewModel.WorkOrderToTextBox = productionIndexQueryUserControlViewModel.CopiedOrders.DefaultIfEmpty().LastOrDefault();
        }
    }
}
