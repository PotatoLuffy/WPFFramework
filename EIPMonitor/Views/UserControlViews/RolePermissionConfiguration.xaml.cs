using EIPMonitor.DomainServices.SecurityServices.FunctionServices;
using EIPMonitor.DomainServices.SecurityServices.RightEnumServices;
using EIPMonitor.DomainServices.SecurityServices.RoleServices;
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
    /// Interaction logic for RolePermissionConfiguration.xaml
    /// </summary>
    public partial class RolePermissionConfiguration : UserControl
    {
        private EIP_MONITOR_PAGESearchService eIP_MONITOR_PAGESearchService;
        //private RightEnumCombobxService rightEnumCombobxService;
        private EIP_MONITOR_ROLESearchService eIP_MONITOR_ROLESearchService;
        private EIP_MONITOR_ROLE_PAGECreateService eIP_MONITOR_ROLE_PAGECreateService;
        private List<ComboboxChecked> jsonEnums;
        private List<EIP_MONITOR_PAGE> eIP_MONITOR_PAGEs;
        private List<EIP_MONITOR_ROLE> eIP_MONITOR_ROLEs;
        private List<String> departments;
        private List<String> roles;
        public RolePermissionConfiguration()
        {
            InitializeComponent();
            eIP_MONITOR_PAGESearchService = new EIP_MONITOR_PAGESearchService();
            eIP_MONITOR_ROLESearchService = new EIP_MONITOR_ROLESearchService();
            eIP_MONITOR_ROLE_PAGECreateService = new EIP_MONITOR_ROLE_PAGECreateService();
            InitializeCombobox();
        }

        private async void InitializeCombobox()
        {
            jsonEnums = RightEnumCombobxService.GetRights();
            this.rightCombobox.ItemsSource = jsonEnums;
            eIP_MONITOR_PAGEs = await eIP_MONITOR_PAGESearchService.GetFunctions(new EIP_MONITOR_PAGE() { STATUS = Status.Active }).ConfigureAwait(true);
            this.functionCombobox.ItemsSource = eIP_MONITOR_PAGEs.Select(S => S.PAGE_NAME).ToList();
            eIP_MONITOR_ROLEs = await eIP_MONITOR_ROLESearchService.GetRoles(new EIP_MONITOR_ROLE()).ConfigureAwait(true);
            departments = eIP_MONITOR_ROLEs.Select(s => s.DEPARTMENT).Distinct().ToList();
            this.departmentCombobox.ItemsSource = departments;

        }

        private void departmentCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.Source as ComboBox;
            roles = this.eIP_MONITOR_ROLEs.Where(w => w.DEPARTMENT == selected.SelectedItem.ToString()).Select(s => s.ROLE_NAME).Distinct().ToList();
            this.roleCombobox.ItemsSource = roles;
        }

        private void AllCheckbocx_CheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            var permissions = e.Source as CheckBox;
            if (permissions.IsChecked != null && permissions.IsChecked == true)
            {
                this.rightCombobox.Text += "," + permissions.Content;
            }
            else
            {
                this.rightCombobox.Text = this.rightCombobox.Text.Replace("," + permissions.Content, "");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            EIP_MONITOR_ROLE_PAGE eIP_MONITOR_ROLE_PAGE = new EIP_MONITOR_ROLE_PAGE()
            {
                DEPARTMENT = this.departmentCombobox.SelectedItem.ToString(),
                RIGHTNAME = String.Join(",", this.rightCombobox.ItemsSource.Cast<ComboboxChecked>().Where(w => w.Check_Status).Select(s => s.Name)),
                PAGE_NAME = this.functionCombobox.Text,
                ROLE_NAME = this.roleCombobox.Text
            };
            eIP_MONITOR_ROLE_PAGE.RIGHTCODE = (RightEnum)Enum.Parse(typeof(RightEnum), eIP_MONITOR_ROLE_PAGE.RIGHTNAME);
            var result = await eIP_MONITOR_ROLE_PAGECreateService.Create(eIP_MONITOR_ROLE_PAGE,IocKernel.Get<IUserStamp>()).ConfigureAwait(false);
            if (result == null)
            {
                MessageBox.Show("创建失败");
            }
            MessageBox.Show("创建成功");
        }
    }
}
