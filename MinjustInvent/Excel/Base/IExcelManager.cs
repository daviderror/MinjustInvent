using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace MinjustInvent.Excel
{
    public interface IExcelManager
    {
        Task<bool> SaveExcel(IEnumerable data);
        string FileName { get; }
    }
}
