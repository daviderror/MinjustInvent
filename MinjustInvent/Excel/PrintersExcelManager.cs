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
    public class PrintersExcelManager : ExcelManager
    {
        public PrintersExcelManager() : base()
        {

        }

        public string FileName { get => FilePath + $"/Принтеры {DateTime.Now:dd-MM-yyyy HH.mm}.xlsx"; }

        public override async Task<bool> SaveExcel(IEnumerable data)
        {
            try
            {
                var currentTypeData = data as List<PrinterOrder>;
                if (currentTypeData == null)
                    throw new Exception("Не подходящий тип для создания excel файла");

                var file = new FileInfo(FileName);
                DeleteIfExists(file);

                //убираем Id т.к. в экселе не нужен
                var excelTypeData = currentTypeData.Select(_ => new
                {
                    _.CabinetNum,
                    _.Name,
                    _.Cartridge,
                    _.InventNumber,
                    _.IP
                }).ToList();

                using (var package = new ExcelPackage(file))
                {
                    var ws = package.Workbook.Worksheets.Add("Принтеры");

                    //заполняем данные
                    var range = ws.Cells["A1"].LoadFromCollection(excelTypeData, false);
              //      range.AutoFitColumns();

                    //Заголовки в экселе
                    ws.Cells["A1"].Value = "№ кабинета";
                    ws.Cells["B1"].Value = "Модель принтера";
                    ws.Cells["C1"].Value = "Картридж";
                    ws.Cells["D1"].Value = "Инвентарный номер";
                    ws.Cells["E1"].Value = "IP";

                    //стили для экселя
                    ws.Column(1).Width = 15;
                    ws.Column(2).Width = 35;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 30;
                    ws.Column(5).Width = 20;

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
