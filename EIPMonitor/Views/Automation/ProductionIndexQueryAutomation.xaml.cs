using EIPMonitor.DomainService;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.MasterData;
using EIPMonitor.ViewModel.Functions;
using GalaSoft.MvvmLight.Messaging;
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

namespace EIPMonitor.Views.Automation
{
    /// <summary>
    /// Interaction logic for ProductionIndexQueryAutomation.xaml
    /// </summary>
    public partial class ProductionIndexQueryAutomation : UserControl
    {
        private ProductionIndexQueryAutomationViewModel productionIndexQueryAutomationViewModel;
        private IRequestLimitControlService requestLimitControlService;
        public ProductionIndexQueryAutomation()
        {
            InitializeComponent();
            productionIndexQueryAutomationViewModel = new ProductionIndexQueryAutomationViewModel();
            DataContext = productionIndexQueryAutomationViewModel;
            requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
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
            try
            {
                var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, "ProductionIndexMain_SelectionChanged");
                if (!getThePermission)
                {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                }
                var datagrid = e.Source as DataGrid;
                if (datagrid == null) return;
                var mapperSource = datagrid.SelectedItem as MES_MO_TO_EIP_POOL;
                if (mapperSource == null) return;
                await productionIndexQueryAutomationViewModel.GetSelectedEntryDetails(mapperSource).ConfigureAwait(true);
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, "ProductionIndexMain_SelectionChanged");
            }
        }
        private async void workOrderFromTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, "workOrderFromTextBox");
                if (!getThePermission)
                {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                }
                await productionIndexQueryAutomationViewModel.MOListQuery().ConfigureAwait(true);
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, "workOrderFromTextBox");
            }
        }
        private async void productIndexQueryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, button.Name);
                if (!getThePermission)
                {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                }
                await productionIndexQueryAutomationViewModel.MOListQuery().ConfigureAwait(true);
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, button.Name);
            }
        }
        private void pastedMultipleOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            try
            {
                var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, button.Name);
                if (!getThePermission)
                {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                }
                string workorders = System.Windows.Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(workorders)) return;
                productionIndexQueryAutomationViewModel.CopiedOrders = workorders.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(o => o).ToList();
                productionIndexQueryAutomationViewModel.WorkOrderFromTextBox = productionIndexQueryAutomationViewModel.CopiedOrders.DefaultIfEmpty().FirstOrDefault();
                productionIndexQueryAutomationViewModel.WorkOrderToTextBox = productionIndexQueryAutomationViewModel.CopiedOrders.DefaultIfEmpty().LastOrDefault();
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, button.Name);
            }
        }
    }
}
