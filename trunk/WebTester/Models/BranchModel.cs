using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projects.Tool.Attributes;

namespace WebTester.Models
{
    [ExcelCaption("部门报表", TitleStyle = ExcelHeaderSuffix.DateTime, TitleFormat = "yyyyMMddHHmmss")]
    public class BranchModel
    {
        [ExcelCaption("部门ID")]
        public int BranchId { get; set; }

        [ExcelCaption("部门名称")]
        public string BranchName { get; set; }

        [ExcelCaption("考勤日期")]
        public string AttendDate { get; set; }

        [ExcelCaption("备注")]
        public string Remark { get; set; }
    }
}