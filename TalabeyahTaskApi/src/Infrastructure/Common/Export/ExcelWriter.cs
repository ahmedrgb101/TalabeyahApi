using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using TalabeyahTaskApi.Application.Common.Exporters;
using TalabeyahTaskApi.Application.Common.Helper;
using System.ComponentModel;
using System.Data;

namespace TalabeyahTaskApi.Infrastructure.Common.Export;
public class ExcelWriter : IExcelWriter
{
    public Stream WriteToStream<T>(IList<T> data)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable("table", "table");
        foreach (PropertyDescriptor prop in properties)
            table.Columns.Add(prop.Name?.ToDescription(), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        foreach (T item in data)
        {
            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
            table.Rows.Add(row);
        }

        using XLWorkbook wb = new XLWorkbook();
        wb.Worksheets.Add(table);
        Stream stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public Stream WriteToStreamWithImage<T>(IList<T> data)
    {
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
        using XLWorkbook wb = new XLWorkbook();

        foreach (T item in data)
        {
            DataTable table = new DataTable($"Container {properties["ContainerNo"]?.GetValue(item)}", $"Container {properties["ContainerNo"]?.GetValue(item)}");
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
            {
                if (!prop.Name.Contains("Path"))
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                else
                    row[prop.Name] = DBNull.Value;
            }

            table.Rows.Add(row);

            var ws = wb.Worksheets.Add(table);
            //ws.Columns().AdjustToContents();
            ws.Columns("A").AdjustToContents();
            ws.Columns("B").AdjustToContents();
            ws.ColumnWidth = 27;
            int cnumber = 3;
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.Name.Contains("Path") && prop.GetValue(item) != null)
                {
                    row[prop.Name] = prop.Name + ws.AddPicture($"{prop.GetValue(item) ?? DBNull.Value}")
                                            .MoveTo(ws.Cell(2, cnumber))
                                            .WithSize(180, 250);
                    cnumber++;
                }
            }
        }

        Stream stream = new MemoryStream();
        wb.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

}
