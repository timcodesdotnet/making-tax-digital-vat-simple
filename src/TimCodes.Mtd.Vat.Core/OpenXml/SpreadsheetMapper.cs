using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using TimCodes.Mtd.Vat.Core.Models;

namespace TimCodes.Mtd.Vat.Core.OpenXml
{
    public class SpreadsheetMapper
    {
        public static AccountingRow[]? MapRows(string filename)
        {
            using var spreadsheetDocument = SpreadsheetDocument.Open(filename, false);
            var workbookPart = spreadsheetDocument.WorkbookPart;

            //Get sheet from excel
            var sheet = workbookPart?.WorksheetParts.First().Worksheet;
            if (workbookPart is null || sheet is null)
            {
                return null;
            }

            var rows = sheet.Descendants<Row>();
            var rowCells = rows
                .Select((q, index) => FromRow(workbookPart, q.Descendants<Cell>().ToArray(), index)).ToArray()
                .Where(q => q.InvoiceDate?.StartsWith("VAT") == true)
                .OrderByDescending(q => q.RowNumber)
                .ToArray();

            return rowCells;
        }

        public static AccountingRow FromRow(WorkbookPart workbookPart, Cell[] values, int index) => new()
        {
            RowNumber = index + 1,
            InvoiceDate = GetDateValue(workbookPart, values, 0),
            DatePaid = GetDateValue(workbookPart, values, 1),
            Company = GetValue(workbookPart, values, 2),
            Description = GetValue(workbookPart, values, 3),
            AmountPaidExVat = GetValue(workbookPart, values, 4),
            AmountSoldExVat = GetValue(workbookPart, values, 5),
            VatToReclaim = GetValue(workbookPart, values, 6),
            VatDue = GetValue(workbookPart, values, 7),
            VatRate = GetValue(workbookPart, values, 8),
            TotalExpenses = GetValue(workbookPart, values, 9),
            TotalRevenue = GetValue(workbookPart, values, 10)
        };

        private static string? GetDateValue(WorkbookPart workbookPart, Cell[] values, int index)
        {
            var date = GetValue(workbookPart, values, index);
            if (int.TryParse(date, out var number) && number > 9999)
            {
                return DateTime.FromOADate(number).ToString("yyyy-MM-dd");
            }
            return date;
        }

        private static string? GetValue(WorkbookPart workbookPart, Cell[] values, int index) => 
            index < values.Length ? GetValue(workbookPart, values[index]) : null;

        public static string GetValue(WorkbookPart workbookPart, Cell cell)
        {
            if (cell.DataType != null)
            {
                if (cell.CellFormula is not null)
                {
                    return cell.CellValue?.InnerText ?? String.Empty;
                }
                if (cell.DataType == CellValues.SharedString)
                {
                    if (int.TryParse(cell.InnerText, out int id))
                    {
                        var item = GetSharedStringItemById(workbookPart, id);

                        if (item?.Text != null)
                        {
                            return item.Text.Text;
                        }
                        else if (item?.InnerText != null)
                        {
                            return item.InnerText;
                        }
                        else if (item?.InnerXml != null)
                        {
                            return item.InnerXml;
                        }
                    }
                }
            }
            return cell.InnerText;
        }

        public static SharedStringItem? GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
    }
}
