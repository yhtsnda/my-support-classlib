using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projects.Tool.Attributes;

namespace WebTester.Models
{
    [ExcelCaption("考勤报表", TitleStyle = ExcelHeaderSuffix.DateTime, TitleFormat = "yyyyMMddHHmmss")]
    public class StaffModel
    {
        [ExcelCaption("用户ID")]
        public int StaffId { get; set; }

        [ExcelCaption("用户名")]
        public string StaffName { get; set; }

        [ExcelCaption("考勤日期")]
        public string AttendDate { get; set; }

        [ExcelCaption("备注")]
        public string Remark { get; set; }
    }
}