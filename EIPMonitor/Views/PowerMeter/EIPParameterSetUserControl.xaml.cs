using ClosedXML.Excel;
using Dapper;
using EIPMonitor.Databse;
using EIPMonitor.DomainServices.MasterData;
using EIPMonitor.LocalInfrastructure;
using EIPMonitor.Model.MasterData;
using Infrastructure.Standard.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

namespace EIPMonitor.Views.PowerMeter
{
    /// <summary>
    /// Interaction logic for EIPParameterSetUserControl.xaml
    /// </summary>
    public partial class EIPParameterSetUserControl : UserControl
    {
        private string fullFilePath;
        private string testName = null;
        private string info_type_code = null;
        private string standardversion = null;
        EM_EXP_PRO_PARAMSETCreateService eM_EXP_PRO_PARAMSETCreateService = new EM_EXP_PRO_PARAMSETCreateService();
        EIP_PRO_GlobalParamConfigureService eIP_PRO_GlobalParamConfigureService = new EIP_PRO_GlobalParamConfigureService();
        private List<EM_EXP_PRO_PARAMSET> insertedList = new List<EM_EXP_PRO_PARAMSET>();
        public EIPParameterSetUserControl()
        {
            InitializeComponent();
            this.ImportTemplateButtton.IsEnabled = false;
            this.ImportTemplateButtton.Background = new SolidColorBrush(Colors.Gray);
        }

        private async void DownloadTemplateButtton_Click(object sender, RoutedEventArgs e)
        {

            this.successDataGrid.ItemsSource = null;
            insertedList.Clear();
            this.DownloadTemplateButtton.IsEnabled = false;
            this.DownloadTemplateButtton.Background = new SolidColorBrush(Colors.Gray);
            //this.failuredDataGrid.ItemsSource = null;
            //ExcelOperator.SetWritterMapping<EIPDataImportTemplate>(EIPDataImportTemplate.excelHeaderMapper);
            DateTime currentDate = DateTime.Now;
            var fileName = System.IO.Path.Combine(currentDate.ToString("yyyy-MM-dd"), "EIP Parameter Set.xlsx");
            var filePath = System.IO.Path.GetTempPath();
            fullFilePath = $"{filePath}{fileName}";
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("EIP内部平台-参数配置导入");
                worksheet.Cell("A1").Value = "生产订单号";
                worksheet.Cell("B1").Value = "设备编码";
                worksheet.Cell("C1").Value = "规格型号";
                worksheet.Cell("D1").Value = "PCB编号";
                worksheet.Cell("E1").Value = "检验日期";
                worksheet.Cell("F1").Value = "软件版本号";
                worksheet.Cell("G1").Value = "国网资产编码";
                workbook.SaveAs(fullFilePath);
            }
            //ExcelOperator.WriteToFile(fullFilePath, new List<EIPDataImportTemplate>(){ new EIPDataImportTemplate()});
            Process.Start(fullFilePath);

            InitalizeTheGlobalParameter();
            //ExcelOperator.SetReaderMapping<Student>(dict);
            this.ImportTemplateButtton.IsEnabled = true;
            this.ImportTemplateButtton.Background = new SolidColorBrush(Color.FromArgb(255, 43, 87, 154));

        }

        public async void InitalizeTheGlobalParameter()
        {
            if (testName == null)
            {
                var testNameConfig = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.em_exp_pro_paramset_test_name).ConfigureAwait(true);
                testName = testNameConfig.Parameter;

            }
            if (info_type_code == null)
            {
                var info_type_codeConfig = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.em_exp_pro_paramset_Info_type_code).ConfigureAwait(true);
                info_type_code = info_type_codeConfig.Parameter;
            }
            if (standardversion == null)
            {
                var standardversionConfig = await eIP_PRO_GlobalParamConfigureService.ExtractConfiguration(Model.MasterData.EIP_PRO_GlobalParameter.EIP_StandardVersion).ConfigureAwait(true);
                standardversion = standardversionConfig.Parameter;
            }
        }
        private async void ImportTemplateButtton_Click(object sender, RoutedEventArgs e)
        {
            this.ImportTemplateButtton.IsEnabled = false;
            this.ImportTemplateButtton.Background = new SolidColorBrush(Colors.Gray);
            var workbook = new XLWorkbook(fullFilePath);

            var ws1 = workbook.Worksheet(1);
            DataTable dataTable = ws1.RangeUsed(XLCellsUsedOptions.AllContents).AsTable().AsNativeDataTable();
            int rowCounts = dataTable.Rows.Count;
            var testCodes = GenerateTestCode(rowCounts);
            //var check_time = GenerateCheckingTime(rowCounts, dataTable);
            var result = EM_EXP_PRO_PARAMSET.ReadDataFromDataTable(dataTable, testName, info_type_code, testCodes, standardversion);

            foreach (var el in result)
            {
                try
                {
                    var checkingTime = await GenerateCheckingTime(el).ConfigureAwait(true);
                    el.CHECK_TIME = checkingTime.ConvertToDateTime();
                    var createResult = await eM_EXP_PRO_PARAMSETCreateService.Create(el).ConfigureAwait(true);
                    if (createResult == null)
                    {
                        insertedList.Add(el);
                        el.InsertedResult = "操作失败";
                        el.FailureReason = "数据库执行了插入操作，但返回结果事插入0行";
                        continue;
                    }
                    el.InsertedResult = "操作成功";
                    el.FailureReason = "N/A";
                    insertedList.Add(el);
                }
                catch (Exception e1)
                {
                    LocalConstant.Logger.Debug("AppDomainUnhandledExceptionHandler", e1);
                    el.InsertedResult = "操作失败";
                    el.FailureReason = e1.Message;
                    insertedList.Add(el);
                }
            }
            this.successDataGrid.ItemsSource = this.insertedList;
            this.DownloadTemplateButtton.IsEnabled = true;
            this.DownloadTemplateButtton.Background = new SolidColorBrush(Color.FromArgb(255, 43, 87, 154));
            //this.failuredDataGrid.ItemsSource = this.failedList;
        }
        public List<String> GenerateTestCode(int rowCounts)
        {
            List<String> testCodes = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();
            FunctionExecutionService functionExecutionService = new FunctionExecutionService();
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("testcode", "PAR");
            for (int cnt = 0; cnt < rowCounts; cnt++)
                tasks.Add(functionExecutionService.ExecuteFunctionOrProcedure(" select fgetno(:testcode) as TestCode from dual ", dynamicParameters));
            tasks.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).ForAll(f => f.Wait());
            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks)
                testCodes.Add(task.Result);

            return testCodes;
        }

        public async Task<String> GenerateCheckingTime(EM_EXP_PRO_PARAMSET eM_EXP_PRO_PARAMSET)
        {

            FunctionExecutionService functionExecutionService = new FunctionExecutionService();
            DynamicParameters dynamicParameters = new DynamicParameters();

            dynamicParameters.Add("YMD", eM_EXP_PRO_PARAMSET.CHECK_TIME.ToString("yyyyMMdd"));
            var result = await functionExecutionService.ExecuteFunctionOrProcedure(" select fgetdate(:YMD) as checkingTime from dual ", dynamicParameters).ConfigureAwait(true);
            return result;

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

        private void successDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
    }
}
