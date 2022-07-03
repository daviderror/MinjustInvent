using System;

namespace MinjustInvent.Model
{
    public class CardModel
    {
        public System.Guid Id { get; set; }
        public Nullable<int> Num { get; set; }
        public string Name { get; set; }
        public Nullable<int> CabinetNum { get; set; }
        public string Card { get; set; }
        public string ReceivedSignature { get; set; }
        public string DepartmentIndex { get; set; }
        public string IssuedSignature { get; set; }
        public Nullable<System.Guid> DepartmentId { get; set; }
        public bool DBEquals(CardModel comp)
        {
            if (comp == null)
                return false;
            if (comp.Name == this.Name &&
                comp.CabinetNum == this.CabinetNum &&
                comp.Card == this.Card &&
                comp.ReceivedSignature == this.ReceivedSignature &&
                comp.DepartmentId == this.DepartmentId &&
                comp.IssuedSignature == this.IssuedSignature)
                return true;
            else
                return false;
        }
    }
}
