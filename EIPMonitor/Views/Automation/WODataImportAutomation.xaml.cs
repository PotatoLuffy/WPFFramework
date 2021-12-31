using EIPMonitor.DomainService;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.ViewModel.Functions;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EIPMonitor.Views.Automation
{
    /// <summary>
    /// Interaction logic for WODataImportAutomation.xaml
    /// </summary>
    public partial class WODataImportAutomation : UserControl
    {
        private WODataImportAutomationViewModel wOImportViewModel;
        private IRequestLimitControlService requestLimitControlService;
        public WODataImportAutomation()
        {
            InitializeComponent();
            wOImportViewModel = new WODataImportAutomationViewModel();
            this.DataContext = wOImportViewModel;
            requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
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
                    await wOImportViewModel.GetMOInformation().ConfigureAwait(true);
                }
                finally
                {
                    requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, "WorkOrderTextbox");
                }
            }
        }

        public async void SynchronousButton_ClickEventHandler(object sender, RoutedEventArgs e)
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
                await wOImportViewModel.TriggerTheButtonRelativeAction(button.Name).ConfigureAwait(true);
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, button.Name);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
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
                await wOImportViewModel.CalculateTheScore().ConfigureAwait(true);
            }
            finally
            {
                requestLimitControlService.ReleaseClickPermission(this.GetType().FullName, button.Name);
            }
        }
    }
}
