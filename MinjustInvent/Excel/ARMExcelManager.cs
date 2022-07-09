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
    public class ARMExcelManager : ExcelManager
    {
        public ARMExcelManager() : base()
        {

        }

        public string FileName { get => FilePath + $"/ARM {DateTime.Now:dd-MM-yyyy HH.mm}.xlsx"; }

        public override async Task<bool> SaveExcel(IEnumerable data)
        {
            try
            {
                var currentTypeData = data as List<ARMOrder>;
                if (currentTypeData == null)
                    throw new Exception("Не подходящий тип для создания excel файла");

                var file = new FileInfo(FileName);
                DeleteIfExists(file);

                //убираем Id т.к. в экселе не нужен
                var excelTypeData = currentTypeData.Select(_ => new
                {
                    _.Num,
                    _.Name,
                    _.Segment,
                    _.IpAdress,
                    _.OperationSystem,
                    _.Memory,
                    _.InventNumber,
                    _.ComputerName,
                    _.Services,
                    _.AccountName
                }).ToList();

                using (var package = new ExcelPackage(file))
                {
                    var ws = package.Workbook.Worksheets.Add("АРМ");

                    //заполняем данные
                    var range = ws.Cells["A2"].LoadFromCollection(excelTypeData, false);
                    range.AutoFitColumns();

                    //Заголовки в экселе
                    ws.Cells["A1"].Value = "№";
                    ws.Cells["B1"].Value = "ФИО";
                    ws.Cells["C1"].Value = "Сегмент";
                    ws.Cells["D1"].Value = "Ip-адресс";
                    ws.Cells["E1"].Value = "Операционная система";
                    ws.Cells["F1"].Value = "Оперативная память";
                    ws.Cells["G1"].Value = "Инвентарный номер";
                    ws.Cells["H1"].Value = "Имя компьютера";
                    ws.Cells["I1"].Value = "Сервисы";
                    ws.Cells["J1"].Value = "Название учетной записи";

                    //стили для экселя
                    ws.Column(1).Width = 6;
                    ws.Column(2).Width = 35;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 25;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 20;
                    ws.Column(9).Width = 20;
                    ws.Column(10).Width = 25;

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
