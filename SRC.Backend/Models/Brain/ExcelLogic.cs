using Castle.Core.Internal;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using SRC.Backend.Models.Config;
using SRC.DB.Interfaces.Settings;
using SRC.DB.Interfaces.Users;
using SRC.DB.Models.Complex;
using SRC.DB.Models.EFMSSQL;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SRC.Backend.Models.Brain
{
    public class SRCExcelAttribute : Attribute
    {
        public string RowName { get; set; }
        public string ColumnName { get; set; }
    }


    public class ExcelLogic
    {
        public string InnerMessage { get; protected set; }

        private Serilog.ILogger Logger = null;

        public ExcelLogic(Serilog.ILogger logger)
        {
            Logger = logger;
        }

        protected void CreateExcelComponent(string sheetName, MemoryStream stream, out SpreadsheetDocument xml, out SheetData sheetData)
        {
            //xml = SpreadsheetDocument.Create(@"C:\Users\src_2\Downloads\123.xlsx", SpreadsheetDocumentType.Workbook);
            xml = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, false);

            WorkbookPart bookPart = xml.AddWorkbookPart();
            bookPart.Workbook = new Workbook();


            sheetData = new SheetData();
            WorksheetPart sheetPart = bookPart.AddNewPart<WorksheetPart>();
            sheetPart.Worksheet = new Worksheet(sheetData);

            //sheetData = new SheetData();
            //sheetPart.Worksheet.AddChild(sheetData);

            //Sheets sheets = bookPart.Workbook.AppendChild<Sheets>(new Sheets());
            Sheets sheets = xml.WorkbookPart.Workbook.AppendChild(new Sheets());

            Sheet sheet = new Sheet()
            {
                Id = bookPart.GetIdOfPart(sheetPart),
                SheetId = 1U,
                Name = sheetName
            };

            sheets.Append(sheet);

        }

        protected byte[] SaveStream(SpreadsheetDocument xml, MemoryStream memory)
        {
            xml.Save();
            xml.Close();
            memory.Seek(0, SeekOrigin.Begin);
            return memory.ToArray();
        }

        protected void DisposeExcelComponent(SpreadsheetDocument xml)
        {
            xml.Close();
        }

        protected Cell StringCell(string data)
        {
            return new Cell()
            {
                CellValue = new CellValue(data),
                DataType = new EnumValue<CellValues>(CellValues.String)
            };
        }

        private Column GetColumn(Columns columns, uint index)
        {
            // 根據列索引查找對應的列定義
            foreach (Column col in columns.Elements<Column>())
            {
                if (col.Min <= index && col.Max >= index)
                {
                    return col;
                }
            }

            return null;
        }

        public List<T> ReadExcel<T>(IFormFile file, int[] takeSheet = null, int startRow = 1)
        {
            List<T> resultObj = Activator.CreateInstance<List<T>>();

            List<PropertyInfo> propertys =
                typeof(T)
                .GetProperties()
                .Where(m => m.GetAttribute<SRCExcelAttribute>() != null)
                .ToList();


            Dictionary<string, List<string>> mapObject = new Dictionary<string, List<string>>();

            foreach (var each in propertys)
            {
                string colName = string.Empty;

                if (!mapObject.ContainsKey(
                    colName = each.GetAttribute<SRCExcelAttribute>().ColumnName))
                {
                    mapObject.Add(colName, new List<string>());
                }

                mapObject[colName].Add(each.Name);
            }

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(file.OpenReadStream(), false))
            {
                if (doc.WorkbookPart == null
                    || doc.WorkbookPart.Workbook == null
                    || doc.WorkbookPart.WorksheetParts == null
                    || doc.WorkbookPart.WorksheetParts.Count() < 1)
                {
                    return default;
                }

                int takeSheetCount = takeSheet == null ? doc.WorkbookPart.Workbook.Sheets.Count() : takeSheet.Length;

                //IEnumerable<Sheet> processSheets = 

                List<WorksheetPart> sheets = doc.WorkbookPart.WorksheetParts.ToList();

                for (int i = 0; i < takeSheetCount; i++)
                {
                    WorksheetPart currentSheet = null;
                    if (takeSheet == null)
                    {
                        currentSheet = sheets[i];
                    }
                    else
                    {
                        if (takeSheet[i] > 0 && takeSheet[i] - 1 < sheets.Count) currentSheet = sheets[takeSheet[i] - 1];
                        else throw new ArgumentException("TakeSheet參數值超出檔案提供的數量");
                    }


                    int rowIdx = 0;
                    foreach (var eachRow in currentSheet.Worksheet.Descendants<Row>())
                    {
                        if (startRow > 1)
                        {
                            startRow--;
                            continue;
                        }

                        rowIdx++;

                        IEnumerable<Cell> cells =
                            eachRow.Descendants<Cell>()
                            .Where(m => mapObject.Keys.Contains(ColumnName(m.CellReference?.Value)));

                        T item = Activator.CreateInstance<T>();

                        foreach (var eachMap in mapObject)
                        {
                            Cell cell = cells.Where(m => ColumnName(m.CellReference?.Value) == eachMap.Key).FirstOrDefault();

                            if (cell == null) continue;

                            foreach (var eachPropertyName in eachMap.Value)
                            {
                                item.GetType().GetProperty(eachPropertyName).SetValue(item, GetCellValue(cell, doc));
                            }
                        }

                        resultObj.Add(item);
                    }


                }
            }

            return resultObj;
        }

        protected string ColumnName(string? cellName)
        {
            if (cellName == null) return string.Empty;

            Regex gex = new Regex("[A-Za-z]+");
            Match match = gex.Match(cellName);

            return match.Value;
        }

        protected string? GetCellValue(Cell cell, SpreadsheetDocument doc)
        {
            if (cell.DataType is not null
                && cell.DataType.Value == CellValues.SharedString
                && int.TryParse(cell.CellValue?.Text, out int idx))
            {
                SharedStringTablePart shareString = doc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                SharedStringItem[] items = shareString.SharedStringTable.Elements<SharedStringItem>().ToArray();

                return items[idx].InnerText;
            }
            else
            {
                return cell.CellValue?.Text;
            }
        }

        private WorksheetPart GetWorksheetPartByName(SpreadsheetDocument document, string sheetName)
        {
            IEnumerable<Sheet> sheets =
               document.WorkbookPart.Workbook.GetFirstChild<Sheets>().
               Elements<Sheet>().Where(s => s.Name == sheetName);

            if (sheets.Count() == 0)
            {
                // The specified worksheet does not exist.

                return null;
            }

            string relationshipId = sheets.First().Id.Value;
            WorksheetPart worksheetPart = (WorksheetPart)
                 document.WorkbookPart.GetPartById(relationshipId);
            return worksheetPart;
        }

        private void InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart,string CellString, CellValues values = CellValues.String)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                var newCell = row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();


                newCell.CellValue = new CellValue(CellString);


                newCell.DataType = new EnumValue<CellValues>(values);

                worksheet.Save();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };

                    newCell.CellValue = new CellValue(CellString);
                
                newCell.DataType = new EnumValue<CellValues>(values);
                
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
            }
        }

        protected string TransIndexToName(int Index)
        {
            if (Index < 0)
            {
                return "";
            }

            List<string> ColumnName = new List<string>();


            do
            {
                if (ColumnName.Count() > 0)
                {
                    Index--;
                }

                ColumnName.Insert(0, ((char)(Index % 26 + 'A')).ToString());

                Index = (Index - Index % 26) / 26;

            } while (Index > 0);

            return string.Join(string.Empty, ColumnName.ToArray());
        }

        protected uint GetColumnIndex(string columnName)
        {
            uint columnIndex = 0;
            foreach (char c in columnName)
            {
                columnIndex = columnIndex * 26 + (uint)(c - 'A' + 1);
            }
            return columnIndex;
        }

        public byte[] MaterialReceipt(List<UnitApplyComplex> Data)
        {
            try
            {
                SpreadsheetDocument xml = null;
                SheetData dataPart = null;

                string[] title = new string[] {
                   "器材/裝備名稱", "申請人", "申請時間", "申請單位"
                };

                using (MemoryStream ms = new MemoryStream())
                {

                    CreateExcelComponent("取料單", ms, out xml, out dataPart);

                    Row row = new Row();
                    dataPart.Append(row);
                    int headIdx = 0;
                    foreach (var each in title)
                    {
                        row.InsertAt(new Cell()
                        {
                            CellValue = new CellValue(each),
                            DataType = new EnumValue<CellValues>(CellValues.String),

                        }, headIdx++);
                    }

                    int cellIdx = 0;
                    if (Data != null)
                    {
                        if (Data.Count() > 0)
                        {
                            for (int i = 0; i < Data.Count(); i++)
                            {
                                cellIdx = 0;
                                dataPart.Append(row = new Row());
                                row.InsertAt(StringCell(Data[i].sub_name), cellIdx++);
                                row.InsertAt(StringCell(Data[i].Apply_Name), cellIdx++);
                                row.InsertAt(StringCell(Data[i].create_time.ToString("yyyy年MM月dd日 HH時mm分")), cellIdx++);
                                row.InsertAt(StringCell(Data[i].unit), cellIdx++);
                            }
                        }
                    }

                    byte[] excelFile = SaveStream(xml, ms);
                    return excelFile;
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"產生取料單發生異常,{ex.Message}");
            }

            return null;
        }

        public byte[] EquipmentConsumptionList(List<PayTreasuryComplex> Data)
        {
            try
            {
                SpreadsheetDocument xml = null;
                SheetData dataPart = null;

                string[] title = new string[] {
                   "器材/裝備名稱", "申請人", "申請單位", "申請數量","繳回數量"
                };

                using (MemoryStream ms = new MemoryStream())
                {

                    CreateExcelComponent("器材消耗清冊", ms, out xml, out dataPart);

                    Row row = new Row();
                    dataPart.Append(row);
                    int headIdx = 0;
                    foreach (var each in title)
                    {
                        row.InsertAt(new Cell()
                        {
                            CellValue = new CellValue(each),
                            DataType = new EnumValue<CellValues>(CellValues.String),

                        }, headIdx++);
                    }

                    int cellIdx = 0;
                    if (Data != null)
                    {
                        if (Data.Count() > 0)
                        {
                            for (int i = 0; i < Data.Count(); i++)
                            {
                                cellIdx = 0;
                                dataPart.Append(row = new Row());
                                row.InsertAt(StringCell(Data[i].name), cellIdx++);
                                row.InsertAt(StringCell(Data[i].apply_name), cellIdx++);
                                row.InsertAt(StringCell(Data[i].unit), cellIdx++);
                                row.InsertAt(StringCell(Data[i].apply_amount.ToString()), cellIdx++);
                                row.InsertAt(StringCell(Data[i].already_pay_amount.ToString()), cellIdx++);
                            }
                        }
                    }

                    byte[] excelFile = SaveStream(xml, ms);
                    return excelFile;
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"產生器材消耗清冊發生異常,{ex.Message}");
            }

            return null;
        }

        public byte[] UnitUseReport(List<UnitApplyComplex> Data,IDF_SystemCode DF_SystemCode,IDF_BackendUser DF_BackendUser)
        {
            try
            {
                SpreadsheetDocument xml = null;
                SheetData dataPart = null;

                var AccountList = Data.Select(x => x.Apply_Name).ToList();

                var Users = DF_BackendUser.ListBackUserByAccount(AccountList);
                var UnitCodes = DF_SystemCode.List_SystemCode("UNIT");

                string[] title = new string[] {
                   "器材/裝備名稱", "申請人", "申請點", "申請單位", "申請數量","申請時間"
                };

                using (MemoryStream ms = new MemoryStream())
                {

                    CreateExcelComponent("使用單位年報表", ms, out xml, out dataPart);

                    Row row = new Row();
                    dataPart.Append(row);
                    int headIdx = 0;
                    foreach (var each in title)
                    {
                        row.InsertAt(new Cell()
                        {
                            CellValue = new CellValue(each),
                            DataType = new EnumValue<CellValues>(CellValues.String),

                        }, headIdx++);
                    }

                    int cellIdx = 0;
                    if (Data != null)
                    {
                        if (Data.Count() > 0)
                        {
                            for (int i = 0; i < Data.Count(); i++)
                            {
                                var User = Users.Where(x => x.account == Data[i].Apply_Name).FirstOrDefault();
                                var UnitCode = UnitCodes.Where(x => x.data == Data[i].unit).FirstOrDefault();

                                cellIdx = 0;
                                dataPart.Append(row = new Row());
                                row.InsertAt(StringCell(Data[i].sub_name), cellIdx++);
                                row.InsertAt(StringCell(User?.name_ch), cellIdx++);
                                row.InsertAt(StringCell(Data[i].subscribepoint), cellIdx++);
                                row.InsertAt(StringCell(UnitCode?.description), cellIdx++);
                                row.InsertAt(StringCell(Data[i].apply_amount.ToString()), cellIdx++);
                                row.InsertAt(StringCell(Data[i].create_time.ToString("yyyy年MM月dd日")), cellIdx++);
                            }
                        }
                    }

                    byte[] excelFile = SaveStream(xml, ms);
                    return excelFile;
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"產生使用單位年報表發生異常,{ex.Message}");
            }

            return null;
        }

        public byte[] ExistingStockReport(List<equipment_maintain> Data)
        {
            try
            {
                SpreadsheetDocument xml = null;
                SheetData dataPart = null;

                string[] title = new string[] {
                   "器材/裝備名稱", "剩餘庫存","建立時間"
                };

                using (MemoryStream ms = new MemoryStream())
                {

                    CreateExcelComponent("現有存量清冊", ms, out xml, out dataPart);

                    Row row = new Row();
                    dataPart.Append(row);
                    int headIdx = 0;
                    foreach (var each in title)
                    {
                        row.InsertAt(new Cell()
                        {
                            CellValue = new CellValue(each),
                            DataType = new EnumValue<CellValues>(CellValues.String),

                        }, headIdx++);
                    }

                    int cellIdx = 0;
                    if (Data != null)
                    {
                        if (Data.Count() > 0)
                        {
                            for (int i = 0; i < Data.Count(); i++)
                            {
                                cellIdx = 0;
                                dataPart.Append(row = new Row());
                                row.InsertAt(StringCell(Data[i].name), cellIdx++);
                                row.InsertAt(StringCell(Data[i].stock.ToString()), cellIdx++);
                                row.InsertAt(StringCell(Data[i].create_time.ToString("yyyy年MM月dd日")), cellIdx++);
                            }
                        }
                    }

                    byte[] excelFile = SaveStream(xml, ms);
                    return excelFile;
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, $"產生現有存量清冊發生異常,{ex.Message}");
            }

            return null;
        }
    }
}
