using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Model
{

    public class DataFileRow
    {
        [Required(ErrorMessage = "The 'Name' column is required.")]
        [MaxLength(100, ErrorMessage = "The 'Name' column cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The 'Type' column is required.")]
        [MaxLength(50, ErrorMessage = "The 'Type' column cannot exceed 50 characters.")]
        [RegularExpression("^(Text|Number|Yes/No|Date|Image)$", ErrorMessage = "Invalid field type in the 'Type' column. Valid types are Text, Number, Yes/No, Date, or Image.")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "The 'Search' column is required.")]
        [RegularExpression("^(Yes|No)$", ErrorMessage = "The 'Search' column can only have values Yes or No.")]
        public string? Search { get; set; }

        [Required(ErrorMessage = "The 'Library Filter' column is required.")]
        [RegularExpression("^(Yes|No)$", ErrorMessage = "The 'Library Filter' column can only have values Yes or No.")]
        public string? LibraryFilter { get; set; }

        [Required(ErrorMessage = "The 'Visible' column is required.")]
        [RegularExpression("^(Yes|No)$", ErrorMessage = "The 'Visible' column can only have values Yes or No.")]
        public string? Visible { get; set; }
    }
}
