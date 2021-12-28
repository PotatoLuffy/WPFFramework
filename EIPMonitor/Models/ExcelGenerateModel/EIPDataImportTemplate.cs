using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EIPMonitor.Model.ExcelGenerateModel
{
    public class EIPDataImportTemplate
    {
        //public static readonly Dictionary<String, String> excelHeaderMapper = new Dictionary<String, String>() 
        //{
        //    { "生产订单号","PRODUCTION_ORDER_ID"},
        //    { "设备编码","DEVICE_NO"},
        //    { "规格型号","MODEL_CODE"},
        //    { "PCB编号","PCB_CODE"},
        //    { "检验时间","CHECK_TIME"},
        //    { "软件版本号","SOFTWARE_VERSION"}
        //};
        //[ImporterHeader(Name = "生产订单号")]
        //[Required(ErrorMessage = "生产订单号不能为空")]
        public String PRODUCTION_ORDER_ID { get; set; }
        //[ImporterHeader(Name = "设备编码")]
        //[Required(ErrorMessage = "设备编码不能为空")]
        public string DEVICE_NO { get; set; }
        //[ImporterHeader(Name = "规格型号")]
        //[Required(ErrorMessage = "规格型号不能为空")]
        public string MODEL_CODE { get; set; }
        //[ImporterHeader(Name = "PCB编号")]
        //[Required(ErrorMessage = "PCB编号不能为空")]
        public string PCB_CODE { get; set; }
        //[ImporterHeader(Name = "检验时间")]
        //[Required(ErrorMessage = "检验时间不能为空")]
        public DateTime CHECK_TIME { get; set; }
        //[ImporterHeader(Name = "软件版本号")]
        //[Required(ErrorMessage = "软件版本号不能为空")]
        public string SOFTWARE_VERSION { get; set; }
    }

   
}
