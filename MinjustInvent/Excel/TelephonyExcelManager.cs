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
    public class TelephonyExcelManager : ExcelManager
    {
        public TelephonyExcelManager() : base()
        {

        }

        public string FileName { get => FilePath + $"/Телефония {DateTime.Now:dd-MM-yyyy HH.mm}.xlsx"; }

        public override async Task<bool> SaveExcel(IEnumerable data)
        {
            try
            {
                var currentTypeData = data as List<TelephonyModel>;
                if (currentTypeData == null)
                    throw new Exception("Не подходящий тип для создания excel файла");

                var file = new FileInfo(FileName);
                DeleteIfExists(file);

                //убираем Id т.к. в экселе не нужен
                var excelTypeData = currentTypeData.Select(_ => new
                {
                    Department = $"{_.DepartmentName} (индекс отдела{_.DepartmentIndex})",
                    _.Position,
                    _.Name,
                    _.CityPhone,
                    _.InternalPhone,
                    _.CabinetNum
                }).ToList();

                using (var package = new ExcelPackage(file))
                {
                    var ws = package.Workbook.Worksheets.Add("Телефония");

                    //заполняем данные
                    var range = ws.Cells["A2"].LoadFromCollection(excelTypeData, false);
                    range.AutoFitColumns();

                    //Заголовки в экселе
                    ws.Cells["A1"].Value = "Отдел";
                    ws.Cells["B1"].Value = "Занимаемая должность";
                    ws.Cells["C1"].Value = "ФИО";
                    ws.Cells["D1"].Value = "Городской телефонный номер";
                    ws.Cells["E1"].Value = "Внутренний телефонный номер";
                    ws.Cells["F1"].Value = "№ кабинета";

                    //стили для экселя
                    ws.Column(1).Width = 70;
                    ws.Column(2).Width = 25;
                    ws.Column(3).Width = 35;
                    ws.Column(4).Width = 35;
                    ws.Column(5).Width = 35;
                    ws.Column(6).Width = 15;

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
