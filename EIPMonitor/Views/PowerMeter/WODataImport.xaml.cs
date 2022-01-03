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

        private void WorkOrderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                    var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, "WorkOrderTextbox");
                    if (!getThePermission)
                    {
                    Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                    return;
                    }
                    var textBox = sender as TextBox;
                    wOImportViewModel.moName = textBox.Text;
                    Task.Run(()=>wOImportViewModel.GetMOInformation().Wait()).ContinueWith(t=> {
                        //release the click count
                        requestLimitControlService.ReleaseClickPermission(className, "WorkOrderTextbox");
                        if (t.IsFaulted)
                        {
                            Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
                        }
                    });

                
            }
        }

        public async void SynchronousButton_ClickEventHandler(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string btnName = button.Name;
                var getThePermission = requestLimitControlService.RequestClickPermission(this.GetType().FullName, button.Name);
                if (!getThePermission)
                {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
                }
                await Task.Run(() => wOImportViewModel.TriggerTheButtonRelativeAction(btnName).Wait()).ContinueWith(t=> {
                    requestLimitControlService.ReleaseClickPermission(className, btnName);
                    if(t.IsFaulted)
                        Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
                });
                //await wOImportViewModel.TriggerTheButtonRelativeAction(button.Name).ConfigureAwait(true);
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string btnName = button.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            await Task.Run(() => wOImportViewModel.CalculateTheScore().ContinueWith(t => { 
                requestLimitControlService.ReleaseClickPermission(className, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
            }).Wait());
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
