using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    class Filters
    {
        [Key]
        public int FiltersId { get; set; }

        public int MaxAge  { get; set; }

        //Minimum 18
        public int MinAge  { get; set; }

        public int MaxDistance  { get; set; }

        public string Searchingfor { get; set; }

        public User UserId { get; set; }

    }
}
