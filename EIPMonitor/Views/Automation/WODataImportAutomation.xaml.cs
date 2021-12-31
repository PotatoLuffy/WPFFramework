using EIPMonitor.ViewModel.Functions;
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
        public WODataImportAutomation()
        {
            InitializeComponent();
            wOImportViewModel = new WODataImportAutomationViewModel();
            this.DataContext = wOImportViewModel;
        }

        private async void WorkOrderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await wOImportViewModel.GetMOInformation().ConfigureAwait(true);
            }
        }

        public async void SynchronousButton_ClickEventHandler(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;
            await wOImportViewModel.TriggerTheButtonRelativeAction(button.Name).ContinueWith(t => { button.IsEnabled = true; }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(true);
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.IsEnabled = false;
            await wOImportViewModel.CalculateTheScore().ContinueWith(t => { button.IsEnabled = true; }, TaskScheduler.FromCurrentSynchronizationContext()).ConfigureAwait(true);
        }
    }
}
