using EIPMonitor.DomainServices.SecurityServices.FunctionServices;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model;
using EIPMonitor.Model.SecurityModel;
using EIPMonitor.Model.Widget;
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

namespace EIPMonitor.Views.UserControlViews
{
    /// <summary>
    /// Interaction logic for PageConfiguration.xaml
    /// </summary>
    public partial class PageConfiguration : UserControl
    {
        public PageConfiguration()
        {
            InitializeComponent();

        }
        private EIP_MONITOR_PAGEDeleteService eIP_MONITOR_PAGEDeleteService = new EIP_MONITOR_PAGEDeleteService();
        private EIP_MONITOR_PAGECreateService eIP_MONITOR_PAGECreateService = new EIP_MONITOR_PAGECreateService();
        private EIP_MONITOR_PAGESearchService eIP_MONITOR_PAGESearchService = new EIP_MONITOR_PAGESearchService();
        private async void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("确定要删除此数据？", "删除确认", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No) return;
            var datagrid = sender as DataGrid;
            if (datagrid == null) return;
            var selectedItem = datagrid.SelectedItem as EIP_MONITOR_PAGE;
            var result = await eIP_MONITOR_PAGEDeleteService.Delete(selectedItem, IocKernel.Get<IUserStamp>()).ConfigureAwait(true);
            if (result == null)
            {
                MessageBox.Show("删除失败");
                return;
            }
            ReloadTheSource().Wait();
            MessageBox.Show("删除成功");

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var ifOrderWeightIsInteger = Int32.TryParse(this.orderWeightTextbox.Text, out Int32 orderWeight);
            if (!ifOrderWeightIsInteger)
            {
                MessageBox.Show("请输入正确的数字顺序。");
                return;
            }
            EIP_MONITOR_PAGE eIP_MONITOR_PAGE = new EIP_MONITOR_PAGE()
            {
                PAGE_NAME = this.functionTextbox.Text,
                PAGE_FUNCTION_NAME = this.functionPageTextbox.Text,
                OrderWeight = orderWeight,
                STATUS_NAME = (this.statusCombobox.SelectedItem as ComboBoxItem).Content.ToString(),
                STATUS = (Status)Enum.Parse(typeof(Status), (this.statusCombobox.SelectedItem as ComboBoxItem).Content.ToString())
            };
            var result = await eIP_MONITOR_PAGECreateService.CreateOrUpdate(eIP_MONITOR_PAGE, IocKernel.Get<IUserStamp>()).ConfigureAwait(true);
            if (result == null)
            {
                MessageBox.Show("创建失败");
                return;
            }
            ReloadTheSource().Wait();
            MessageBox.Show("创建成功");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadTheSource().Wait();
        }

        private async Task ReloadTheSource()
        {
            var result = await eIP_MONITOR_PAGESearchService.GetFunctions(new EIP_MONITOR_PAGE() { STATUS = Status.Active }).ConfigureAwait(true);
            this.dataGrid.ItemsSource = null;
            this.dataGrid.ItemsSource = result;
        }

        private void dataGrid_CopyingRowClipboardContent(object sender, DataGridRowClipboardEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            var currentCell = e.ClipboardRowContent[dataGrid.CurrentCell.Column.DisplayIndex];
            e.ClipboardRowContent.Clear();
            e.ClipboardRowContent.Add(currentCell);
        }
    }
}
