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
    /// Interaction logic for PromoteTheScoreAutomation.xaml
    /// </summary>
    public partial class PromoteTheScoreAutomation : UserControl
    {
        private PromoteTheScoreAutomationViewModel promoteTheScorePowerMeterViewModel;
        private IRequestLimitControlService requestLimitControlService;
        private readonly string className;
        public PromoteTheScoreAutomation()
        {
            InitializeComponent();
            promoteTheScorePowerMeterViewModel = new PromoteTheScoreAutomationViewModel();
            this.DataContext = promoteTheScorePowerMeterViewModel;
            requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
            className = this.GetType().FullName;
        }
        private async void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            promoteTheScorePowerMeterViewModel.OrderTextBox = this.orderTextBox.Text;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, "TextBox");
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }

            await Task.Run(() => promoteTheScorePowerMeterViewModel.GetMoInfo().Wait()).ContinueWith(t =>
            {
                requestLimitControlService.ReleaseClickPermission(className, "TextBox");
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
            });
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var btnName = btn.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            await Task.Run(() => promoteTheScorePowerMeterViewModel.GetMoInfo().Wait()).ContinueWith(t =>
            {
                requestLimitControlService.ReleaseClickPermission(className, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
            });
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var btnName = btn.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }

            await Task.Run(() => promoteTheScorePowerMeterViewModel.PromoteTheScoreForcely().Wait()).ContinueWith(t =>
            {
                requestLimitControlService.ReleaseClickPermission(className, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
            });

        }
        private void DataGrid_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var datagrid = sender as DataGrid;
            var currentCell = e.ClipboardRowContent[datagrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }
    }
}
