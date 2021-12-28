using EIPMonitor.Model.ExcelGenerateModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.ExcelGeneratorService
{
    public static class TemplateGenerator
    {
        //public static async Task<String> GenerateExcelTemplateEIPDataImportTemplate()
        //{
        //    IImporter Importer = new ExcelImporter();
        //    var result = await Importer.GenerateTemplateBytes<EIPDataImportTemplate>();
        //    DateTime currentDate = DateTime.Now;
        //    var fileName = Path.Combine(currentDate.ToString("yyyy-MM-dd"), "EIP Parameter Set", ".xlsx");
        //    var filePath = Path.GetTempPath();
        //    var fullFilePath = $"{filePath}{fileName}";
        //    File.WriteAllBytes(fullFilePath, result);
        //    return fullFilePath;
        //}
    }
}
