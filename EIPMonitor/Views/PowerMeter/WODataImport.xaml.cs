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
    /// Interaction logic for WODataImport.xaml
    /// </summary>
    public partial class WODataImport : UserControl
    {
        public WODataImport()
        {
            InitializeComponent();
        }

        private async void WorkOrderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            //this.WorkOrderTextbox.IsEnabled = false;
            //try
            //{
            //    if (e.Key == Key.Enter)
            //    {
            //        var result = await VerifyTheMOName(this.WorkOrderTextbox.Text).ConfigureAwait(true);
            //        if (result == null) return;
            //        this.MOQtyRequirement.Text = result.AMOUNT.ToString("###,###");
            //        this.materialCodeTextbox.Text = result.MATERIALSCODE;
            //        this.materialNameTextbox.Text = result.MATERIALSNAME;
            //        CleanAllTheCommentTextBlock();
            //        EnableAllTheButton();
            //    }
            //}
            //catch (Exception e1)
            //{
            //    LocalConstant.Logger.Debug("AppDomainUnhandledExceptionHandler", e1);
            //    this.materialNameTextbox.Text = "该工单不是一个电能表工单";
            //}
            //finally
            //{
            //    this.WorkOrderTextbox.IsEnabled = true;

            //}
        }

        public async void SynchronousButton_ClickEventHandler(object sender, RoutedEventArgs e)
        {
            
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
