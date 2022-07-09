using System.Collections;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace MinjustInvent.Excel
{
    public abstract class ExcelManager : IExcelManager
    {
        public ExcelManager()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static string FilePath
        {
            get => Properties.Settings.Default.ExcelFilePath;
            set => Properties.Settings.Default.ExcelFilePath = value;
        }

        public string FileName { get => throw new System.NotImplementedException(); }

        public abstract Task<bool> SaveExcel(IEnumerable data);

        public static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}
