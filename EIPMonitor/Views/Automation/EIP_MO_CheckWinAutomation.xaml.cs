using EIPMonitor.DomainService;
using EIPMonitor.LocalInfrastructure;
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
    /// Interaction logic for EIP_MO_CheckWinAutomation.xaml
    /// </summary>
    public partial class EIP_MO_CheckWinAutomation : UserControl
    {
        private IRequestLimitControlService requestLimitControlService;
        private EIP_MO_CheckWinAutomationViewModel eIP_MO_CheckWinViewModel;
        private readonly string className;
        public EIP_MO_CheckWinAutomation()
        {
            InitializeComponent();
            requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
            eIP_MO_CheckWinViewModel = new EIP_MO_CheckWinAutomationViewModel();
            this.DataContext = eIP_MO_CheckWinViewModel;
            className = this.GetType().FullName;
        }
        private async void productIndexQueryButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string btnName = button.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            await Task.Run(() => eIP_MO_CheckWinViewModel.MOListQuery().Wait()).ContinueWith(t =>
            {
                requestLimitControlService.ReleaseClickPermission(className, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
            });

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

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
            else
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }
        private void pastedMultipleOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string btnName = button.Name;
            try
            {
                var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
                if (!getThePermission)
                {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                }
                string workorders = System.Windows.Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(workorders)) return;
                eIP_MO_CheckWinViewModel.CopiedOrders = workorders.Split(new String[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Distinct().OrderBy(o => o).ToList();
                eIP_MO_CheckWinViewModel.WorkOrderFromTextBox = eIP_MO_CheckWinViewModel.CopiedOrders.DefaultIfEmpty().FirstOrDefault();
                eIP_MO_CheckWinViewModel.WorkOrderToTextBox = eIP_MO_CheckWinViewModel.CopiedOrders.DefaultIfEmpty().LastOrDefault();
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(className, btnName);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("确定要提交这些数据吗？", "提交确认", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No) return;
            var button = sender as Button;
            string btnName = button.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            await Task.Run(() => eIP_MO_CheckWinViewModel.CheckSelectedRows().Wait()).ContinueWith(t =>
            {
                requestLimitControlService.ReleaseClickPermission(className, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
            });



            //checkMODataWindow.Show();
        }

        private void ProductionIndexMain_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var currentCell = e.ClipboardRowContent[dataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }
    }
}
