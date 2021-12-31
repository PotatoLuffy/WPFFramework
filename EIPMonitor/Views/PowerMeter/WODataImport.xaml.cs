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
using System.Threading;
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
    /// Interaction logic for WODataImport.xaml
    /// </summary>
    public partial class WODataImport : UserControl
    {
        private WOImportViewModel wOImportViewModel;
        private IRequestLimitControlService requestLimitControlService;
        private readonly string className;
        public WODataImport()
        {
            InitializeComponent();
            wOImportViewModel = new WOImportViewModel();
            this.DataContext = wOImportViewModel;
            requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
            className = this.GetType().FullName;
        }

        private async void WorkOrderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {

                    var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, "WorkOrderTextbox");
                    if (!getThePermission)
                    {
                        Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                        return;
                    }
                    var textBox = sender as TextBox;
                    wOImportViewModel.moName = textBox.Text;
                    await wOImportViewModel.GetMOInformation().ConfigureAwait(true);
                }
                finally
                {
                    requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, "WorkOrderTextbox");
                }
            }
        }

        public void SynchronousButton_ClickEventHandler(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string btnName = button.Name;
            try
            {
                var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, button.Name);
                if (!getThePermission)
                {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                }
                var result = Task.Run(() => wOImportViewModel.TriggerTheButtonRelativeAction(btnName).Wait());
                //await wOImportViewModel.TriggerTheButtonRelativeAction(button.Name).ConfigureAwait(true);
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, button.Name);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string btnName = button.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            var result = Task.Run(() => wOImportViewModel.CalculateTheScore().ContinueWith(t => requestLimitControlService.ReleaseClickPermission(className, btnName)).Wait());
            //await wOImportViewModel.CalculateTheScore().ConfigureAwait(false);
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
    }
}
