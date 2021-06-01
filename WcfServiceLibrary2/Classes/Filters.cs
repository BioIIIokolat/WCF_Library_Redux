using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    public class Filters
    {
        [Key]
        public int FiltersId { get; set; }
        public int MaxAge  { get; set; }
        public int MinAge  { get; set; }
        public string ColorHair { get; set; }
        public string ColorEye { get; set; }
        public int Height { get; set; }
        public int MaxDistance  { get; set; }
        public int UserId { get; set; }
    }
}
