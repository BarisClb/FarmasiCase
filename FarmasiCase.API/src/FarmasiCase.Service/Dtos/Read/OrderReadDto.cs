using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Dtos.Read
{
    public class OrderReadDto<T>
    {
        public string Id { get; set; }
        public string User { get; set; }
        public List<T> Items { get; set; }
    }
}
