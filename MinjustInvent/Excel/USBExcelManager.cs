using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinjustInvent.Excel
{
    public class UsbExcelManager : ExcelManager
    {
        public UsbExcelManager() : base()
        {

        }

        public string FileName { get => FilePath + $"/USB {DateTime.Now:dd-MM-yyyy HH.mm}.xlsx"; }

        public override async Task<bool> SaveExcel(IEnumerable data)
        {
            try
            {
                var currentTypeData = data as List<USBOrder>;
                if (currentTypeData == null)
                    throw new Exception("Не подходящий тип для создания excel файла");

                var file = new FileInfo(FileName);
                DeleteIfExists(file);

                //убираем Id т.к. в экселе не нужен
                var excelTypeData = currentTypeData.Select(_ => new
                {
                    _.Name,
                    _.SerialNumber,
                    _.Size
                }).ToList();

                using (var package = new ExcelPackage(file))
                {
                    var ws = package.Workbook.Worksheets.Add("USB");

                    //заполняем данные
                    var range = ws.Cells["A1"].LoadFromCollection(excelTypeData, false);
              //      range.AutoFitColumns();

                    //Заголовки в экселе
                    ws.Cells["A1"].Value = "ФИО";
                    ws.Cells["B1"].Value = "Серийный номер";
                    ws.Cells["C1"].Value = "Объем накопителя";

                    //стили для экселя
                    ws.Column(1).Width = 6;
                    ws.Column(2).Width = 35;
                    ws.Column(3).Width = 20;

                    await package.SaveAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
