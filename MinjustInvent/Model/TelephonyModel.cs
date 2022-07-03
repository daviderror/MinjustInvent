using System;

namespace MinjustInvent.Model
{
    public class TelephonyModel
    {
        public System.Guid Id { get; set; }
        public Nullable<int> Num { get; set; }
        public string Name { get; set; }
        public Nullable<int> CabinetNum { get; set; }
        public string Position { get; set; }
        public string CityPhone { get; set; }
        public string InternalPhone { get; set; }
        public Nullable<System.Guid> DepartmentId { get; set; }
        public string DepartmentIndex { get; set; }
        public bool DBEquals(TelephonyModel comp)
        {
            if (comp == null)
                return false;
            if (comp.Name == this.Name &&
                comp.CabinetNum == this.CabinetNum &&
                comp.CityPhone == this.CityPhone &&
                comp.DepartmentIndex == this.DepartmentIndex &&
                comp.DepartmentId == this.DepartmentId &&
                comp.InternalPhone == this.InternalPhone &&
                comp.Num == this.Num &&
                comp.Position == this.Position)
                return true;
            else
                return false;
        }
    }
}
