using MinjustInvent.Model;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinjustInvent.Excel
{
    public class CardsExcelManager : ExcelManager
    {
        public CardsExcelManager() : base()
        {

        }

        public string FileName { get => FilePath + $"/Карточки {DateTime.Now:dd-MM-yyyy HH.mm}.xlsx"; }

        public override async Task<bool> SaveExcel(IEnumerable data)
        {
            try
            {
                var currentTypeData = data as List<CardModel>;
                if (currentTypeData == null)
                    throw new Exception("Не подходящий тип для создания excel файла");

                var file = new FileInfo(FileName);
                DeleteIfExists(file);

                //убираем Id т.к. в экселе не нужен
                var excelTypeData = currentTypeData.Select(_ => new
                {
                    _.Name,   
                    _.DepartmentName,
                    _.Card,
                    _.IssuedSignature,
                    _.ReceivedSignature
                }).ToList();

                using (var package = new ExcelPackage(file))
                {
                    var ws = package.Workbook.Worksheets.Add("Карточки");

                    //заполняем данные
                    var range = ws.Cells["A2"].LoadFromCollection(excelTypeData, false);
                    range.AutoFitColumns();

                    //Заголовки в экселе
                    ws.Cells["A1"].Value = "ФИО";
                    ws.Cells["B1"].Value = "Отдел";
                    ws.Cells["C1"].Value = "Номер карты";
                    ws.Cells["D1"].Value = "Подпись(кто получил)";
                    ws.Cells["E1"].Value = "Подпись(кто выдавал)";

                    //стили для экселя
                    ws.Column(1).Width = 30;
                    ws.Column(2).Width = 70;
                    ws.Column(3).Width = 20;
                    ws.Column(4).Width = 20;
                    ws.Column(5).Width = 25;

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
