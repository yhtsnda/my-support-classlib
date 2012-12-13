using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Projects.Tool.Util;
using WebTester.Models;
using Projects.Mvc;
using Warehouse.RecordProcessor;

namespace WebTester.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ExportExcel()
        //{
        //    IList<StaffModel> data = new List<StaffModel>();
        //    IList<BranchModel> data2 = new List<BranchModel>();

        //    for(int i=0;i<100;i++)
        //    {
        //        data.Add(new StaffModel
        //        {
        //            StaffId = i + 1,
        //            StaffName = "用户名" + (i + 1).ToString(),
        //            AttendDate = DateTime.Today.ToString("yyyy-MM-dd"),
        //            Remark = "这是一个备注"
        //        });
        //    }

        //    for (int i = 0; i < 100; i++)
        //    {
        //        data2.Add(new BranchModel
        //        {
        //            BranchId = i + 1,
        //            BranchName = "部门名" + (i + 1).ToString(),
        //            AttendDate = DateTime.Today.ToString("yyyy-MM-dd"),
        //            Remark = "这是一个备注"
        //        });
        //    }

        //    ExcelBuilder builder = new ExcelBuilder();
        //    var excelStream = builder.AddExportSet<StaffModel>(data)
        //        .AddExportSet<BranchModel>(data2)
        //        .ExportAll();

        //    //ExcelResult result = 
        //    //    new ExcelResult("导出测试_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", excelStream);
        //    //return result;
        //}
            //        result.AddSource<StaffModel>(data, "考勤报表" + DateTime.Now.ToString("yyyyMMddHHmmss"), new string[] { "用户ID", "用户名", "考勤日期", "备注" });
            //result.AddSource<BranchModel>(data2, "部门报表" + DateTime.Now.ToString("yyyyMMddHHmmss"), new string[] { "部门ID", "部门名", "考勤日期", "备注" });
    }
}
