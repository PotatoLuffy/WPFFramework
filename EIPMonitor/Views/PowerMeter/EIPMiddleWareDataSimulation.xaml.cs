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

namespace EIPMonitor.Views.PowerMeter
{
    /// <summary>
    /// Interaction logic for EIPMiddleWareDataSimulation.xaml
    /// </summary>
    public partial class EIPMiddleWareDataSimulation : UserControl
    {
        private Dictionary<string, DatePicker> buttonDateMapper;
        private Dictionary<String, Func<DateTime?>> GrabedDateMapper;
        private readonly EIPMiddleWareDataSimulationViewModel eIPMiddleWareDataSimulationViewModel;
        private IRequestLimitControlService requestLimitControlService;
        private readonly string className;
        public EIPMiddleWareDataSimulation()
        {
            InitializeComponent();

            buttonDateMapper = new Dictionary<string, DatePicker>()
            {
                { "PCBAOIBtn", this.PCBAOIDatePickerName},
                { "FCTBtn",this.FCTDatePickerName },
                { "BatteryCurrrentBtn",this.BatteryCurrrentDatePickerName},
                { "AgingBtn",this.AgingDatePickerName},
                { "ReflowBtn",this.ReflowDatePickerName},
                { "WaveBtn",this.WaveDatePickerName},
                { "HighVoltageBtn",this.HighVoltageDatePickerName},
                { "IntrinsicErrorBtn",this.IntrinsicErrorDatePickerName},
                { "IntrinsicErrorDetailBtn",this.IntrinsicErrorDetailDatePickerName},
                { "DayTimingBtn", this.DayTimingDatePickerName},
            };
            GrabedDateMapper = new Dictionary<string, Func<DateTime?>>()
            {
                { "PCBAOIBtn", ()=>eIPMiddleWareDataSimulationViewModel.PCBAOIDatePicker},
                { "FCTBtn",()=>eIPMiddleWareDataSimulationViewModel.FCTDatePicker },
                { "BatteryCurrrentBtn",()=>eIPMiddleWareDataSimulationViewModel.BatteryCurrrentDatePicker},
                { "AgingBtn",()=>eIPMiddleWareDataSimulationViewModel.AgingDatePicker},
                { "ReflowBtn",()=>eIPMiddleWareDataSimulationViewModel.ReflowDatePicker},
                { "WaveBtn",()=>eIPMiddleWareDataSimulationViewModel.WaveDatePicker},
                { "HighVoltageBtn",()=>eIPMiddleWareDataSimulationViewModel.HighVoltageDatePicker},
                { "IntrinsicErrorBtn",()=>eIPMiddleWareDataSimulationViewModel.IntrinsicErrorDatePicker},
                { "IntrinsicErrorDetailBtn",()=>eIPMiddleWareDataSimulationViewModel.IntrinsicErrorDetailDatePicker},
                { "DayTimingBtn", ()=>eIPMiddleWareDataSimulationViewModel.DayTimingDatePicker},
            };
            eIPMiddleWareDataSimulationViewModel = new EIPMiddleWareDataSimulationViewModel();
            this.DataContext = eIPMiddleWareDataSimulationViewModel;
            this.className = this.GetType().FullName;
            this.requestLimitControlService = IocKernel.Get<IRequestLimitControlService>();
        }

        private async void Buttton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var btnName = btn.Name;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, btnName);
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            await Task.Run(() => eIPMiddleWareDataSimulationViewModel.SimulateTheScore(btnName).Wait()).ContinueWith(t => {
                requestLimitControlService.ReleaseClickPermission(className, btnName);
                if (t.IsFaulted)
                    Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
                EnableOrDisableTheSelectedDatePicker();
            });
        }
        private async void WorkOrderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var getThePermission = requestLimitControlService.RequestClickPermission(className, "TextBox");
            if (!getThePermission)
            {
                Messenger.Default.Send("请求已经再处理中，请勿重复点击。", "SendMessageToMainWin");
                return;
            }
            eIPMiddleWareDataSimulationViewModel.ClearAllThePicker();
            EnableOrDisableTheSelectedDatePicker();
           await Task.Run(()=> eIPMiddleWareDataSimulationViewModel.VerifyMOAndGetSimulateDate().Wait()).ContinueWith(t=> {
               requestLimitControlService.ReleaseClickPermission(className, "TextBox");
               if (t.IsFaulted)
                   Messenger.Default.Send(t.Exception.Flatten().InnerException.Message, "SendMessageToMainWin");
               EnableOrDisableTheSelectedDatePicker();
           },TaskScheduler.FromCurrentSynchronizationContext());
            
        }
        private void EnableOrDisableTheSelectedDatePicker()
        {
            foreach (var pickerDic in buttonDateMapper)
                if (!GrabedDateMapper[pickerDic.Key]().HasValue)
                    pickerDic.Value.IsEnabled = true;
                else
                    pickerDic.Value.IsEnabled = false;
        }
    }
}
