using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Dtos.Create
{
    public class OrderCreateDto<T>
    {
        public string User { get; set; }
        public string Status { get; set; } = "Created";
        public List<T> Items { get; set; }
    }
}
