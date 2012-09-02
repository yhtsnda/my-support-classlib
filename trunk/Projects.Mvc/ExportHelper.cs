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

namespace BuildingSiteCheck
{
    /// <summary>
    /// 导出数据的助手
    /// </summary>
    public class ExportHelper
    {
        public static void Export<T>(IList<T> source, string header, string file, string[] columns)
        {
            using (var ms = Export(source, header, columns))
            {
                using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        public static MemoryStream Export<T>(IList<T> genSource, string header, string[] columns)
        {
            //先将泛型转为Datatable
            DataTable source = GenericToDatatable(genSource);

            if (columns != null && columns.Length != source.Columns.Count)
                throw new ArgumentException("参数不正确:columnNames,数组元素的个数需要和数据源列的数量相同!");

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = workbook.CreateSheet("导出的数据") as HSSFSheet;
            //设置文件的属性
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "福建万乘科技有限公司";
            workbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = "考勤系统";
            si.Title = header;
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;

            //设置窗格的格式
            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

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
                        sheet = workbook.CreateSheet() as HSSFSheet;

                    //表头及样式
                    HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                    headerRow.HeightInPoints = 25;
                    headerRow.CreateCell(0).SetCellValue(header);

                    HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                    headStyle.Alignment = HorizontalAlignment.CENTER;
                    HSSFFont font = workbook.CreateFont() as HSSFFont;
                    font.FontHeightInPoints = 20;
                    font.Boldweight = 700;
                    headStyle.SetFont(font);
                    headerRow.GetCell(0).CellStyle = headStyle;
                    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, source.Columns.Count - 1));

                    //列头及样式
                    headerRow = sheet.CreateRow(1) as HSSFRow;
                    headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                    headStyle.Alignment = HorizontalAlignment.CENTER;
                    font = workbook.CreateFont() as HSSFFont;
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    headStyle.SetFont(font);
                    if (columns == null)
                    {
                        foreach (DataColumn column in source.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }
                    else
                    {
                        foreach (DataColumn column in source.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(columns[column.Ordinal]);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

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

                            newCell.CellStyle = dateStyle;//格式化显示
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
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }

        public static void ExportByWeb<T>(IList<T> source, string header, string fileName, string[] columns)
        {
            HttpContext curContext = HttpContext.Current;

            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
            curContext.Response.BinaryWrite(Export(source, header, columns).GetBuffer());
            curContext.Response.End();
        }

        /// <summary>
        /// 将泛型转为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable GenericToDatatable<T>(IList<T> list)
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
        public static DataTable GenericToDatatable<T>(IList<T> list, params  string[] properties)
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
    }
}