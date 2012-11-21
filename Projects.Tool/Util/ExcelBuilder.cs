using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Projects.Tool.Util
{
    /// <summary>
    /// 导出数据的助手
    /// </summary>
    public class ExcelBuilder
    {
        HSSFWorkbook mWorkbook;
        HSSFCellStyle mDateStyle, mTableHeadStyle, mColumnHeadStyle;
        int mSheetCounter = 0;

        /// <summary>
        /// Excel构建器
        /// </summary>
        /// <param name="title">Excel的标题,出现在摘要中,默认为空</param>
        public ExcelBuilder(string title = "")
        {
            mWorkbook = new HSSFWorkbook();
            //设置文档的属性
            SetDocumentAttributes(title);
            //设置数据单元格的属性:日期类型,表头类型,列头
            SetDataCellStyle();
        }

        public ExcelBuilder AddExportSet<T>(IList<T> genSource, string header, string[] columns)
        {
            //先将泛型转为Datatable
            DataTable source = GenericToDatatable(genSource);

            if (columns != null && columns.Length != source.Columns.Count)
                throw new ArgumentException("参数不正确:columnNames,数组元素的个数需要和数据源列的数量相同!");
            //设置一个Sheet的数据
            SetSheetData(source, header, columns);
            this.mSheetCounter++;
            return this;
        }

        public MemoryStream ExportAll()
        {
            if (mSheetCounter == 0)
                throw new NullReferenceException("没有添加任何导出数据....请至少添加一个导出数据!");

            using (MemoryStream ms = new MemoryStream())
            {
                mWorkbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        /// <summary>
        /// 导出数据,只能导出一个Sheet的数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="source">要导出的数据</param>
        /// <param name="header">数据表的标题</param>
        /// <param name="fileName">文件名</param>
        /// <param name="columns">数据列字段</param>
        public void Export<T>(IList<T> source, string header, string file, string[] columns)
        {
            using (var ms = AddExportSet(source, header, columns).ExportAll())
            {
                using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// 通过Web导出,只能导出一个Sheet的数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="source">要导出的数据</param>
        /// <param name="header">数据表的标题</param>
        /// <param name="fileName">文件名</param>
        /// <param name="columns">数据列字段</param>
        public void ExportByWeb<T>(IList<T> source, string header, string fileName, string[] columns)
        {
            HttpContext curContext = HttpContext.Current;

            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(AddExportSet(source, header, columns).ExportAll().GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        /// 将泛型转为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        internal static DataTable GenericToDatatable<T>(IList<T> list)
        {
            return GenericToDatatable(list, null);
        }

        /// <summary>
        /// 将泛型转为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        internal static DataTable GenericToDatatable<T>(IList<T> list, params  string[] properties)
        {
            List<string> propertyList = properties != null ? properties.ToList() : new List<string>();
            DataTable result = new DataTable();

            if (list.Any())
            {
                PropertyInfo[] property = list[0].GetType().GetProperties();
                property.ForEach(o =>
                                     {
                                         if (propertyList.Any() && propertyList.Contains(o.Name))
                                             result.Columns.Add(o.Name);
                                         else if (propertyList.Count == 0)
                                             result.Columns.Add(o.Name);
                                     });
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (var pi in property)
                    {
                        object tempObj = pi.GetValue(list[i], null);
                        if (propertyList.Any() && propertyList.Contains(pi.Name))
                        {
                            tempList.Add(tempObj);
                        }
                        else if (propertyList.Count == 0)
                        {
                            tempList.Add(tempObj);
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 设置数据表的数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="header"></param>
        /// <param name="columns"></param>
        internal void SetSheetData(DataTable source, string header, string[] columns)
        {
            HSSFSheet sheet = mWorkbook.CreateSheet(header) as HSSFSheet;
            //取得列宽
            int[] arrColWidth = new int[source.Columns.Count];
            foreach (DataColumn item in source.Columns)
                arrColWidth[item.Ordinal] = Encoding.UTF8.GetBytes(item.ColumnName).Length;
            //设置列宽
            for (var i = 0; i < source.Rows.Count; i++)
            {
                for (var j = 0; j < source.Columns.Count; j++)
                {
                    var intTemp = Encoding.UTF8.GetBytes(source.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                        arrColWidth[j] = intTemp;
                }
            }

            int rowIndex = 0;
            foreach (DataRow row in source.Rows)
            {
                //新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                        sheet = mWorkbook.CreateSheet() as HSSFSheet;

                    //表头及样式
                    HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                    headerRow.HeightInPoints = 25;
                    headerRow.CreateCell(0).SetCellValue(header);
                    headerRow.GetCell(0).CellStyle = mTableHeadStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, source.Columns.Count - 1));

                    //列头及样式
                    headerRow = sheet.CreateRow(1) as HSSFRow;
                    if (columns == null)
                    {
                        foreach (DataColumn column in source.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = mColumnHeadStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    else
                    {
                        foreach (DataColumn column in source.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(columns[column.Ordinal]);
                            headerRow.GetCell(column.Ordinal).CellStyle = mColumnHeadStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    rowIndex = 2;
                }

                //填充内容
                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in source.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);
                            newCell.CellStyle = mDateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                rowIndex++;
            }
        }

        /// <summary>
        /// 设置文档属性
        /// </summary>
        /// <param name="title">文档的标题</param>
        protected virtual void SetDocumentAttributes(string title)
        {
            //设置文件的属性
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "福建万乘科技有限公司";
            mWorkbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = "自动导出组件";
            si.Title = title;
            si.CreateDateTime = DateTime.Now;
            mWorkbook.SummaryInformation = si;
        }

        /// <summary>
        ///  设置数据格式
        /// </summary>
        protected virtual void SetDataCellStyle()
        {
            //设置窗格的格式
            mDateStyle = mWorkbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = mWorkbook.CreateDataFormat() as HSSFDataFormat;
            mDateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //表头及样式
            mTableHeadStyle = mWorkbook.CreateCellStyle() as HSSFCellStyle;
            mTableHeadStyle.Alignment = HorizontalAlignment.CENTER;
            HSSFFont font = mWorkbook.CreateFont() as HSSFFont;
            font.FontHeightInPoints = 20;
            font.Boldweight = 700;
            mTableHeadStyle.SetFont(font);
            //列头及样式
            mColumnHeadStyle = mWorkbook.CreateCellStyle() as HSSFCellStyle;
            mColumnHeadStyle.Alignment = HorizontalAlignment.CENTER;
            font = mWorkbook.CreateFont() as HSSFFont;
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            mColumnHeadStyle.SetFont(font);
        }
    }
}