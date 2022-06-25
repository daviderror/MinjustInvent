using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinjustInvent
{
    class Computer
    {
        private int Cabinet { get; set; }
        private string Segment { get; set; }
        private string Ip { get; set; }
        private string Os { get; set; }
        private int Ozu { get; set; }
        private string Fio { get; set; }
        private string Name { get; set; }
        private int Id { get; set; }
        public Computer()
        {

        }
        public Computer(string Segment, string Ip, string Os, string Fio, string Name, int Cabinet, int Ozu, int Id)
        {
            this.Segment = Segment;
            this.Ip = Ip;
            this.Os = Os;
            this.Fio = Fio;
            this.Name = Name;
            this.Cabinet = Cabinet;
            this.Ozu = Ozu;
            this.Id = Id;

        }
    }
}
