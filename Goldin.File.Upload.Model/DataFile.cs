using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Model
{
    public class DataFile
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool? Search { get; set; }
        public bool? LibraryFilter { get; set; }
        public bool? Visible { get; set; }
    }
}
