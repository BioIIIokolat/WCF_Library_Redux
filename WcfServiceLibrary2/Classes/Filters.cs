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

        //Minimum 18
        public int MinAge  { get; set; }

        public int ColorHair { get; set; }

        public string ColorEye { get; set; }

        public int Height { get; set; }

        public List<string> Hobbies { get; set; }
        public string Job { get; set; }

        public List<string> Educations { get; set; }

        public int MaxDistance  { get; set; }

        public string Searchingfor { get; set; }

        public User UserId { get; set; }
    }
}
