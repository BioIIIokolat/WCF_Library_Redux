using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WcfServiceLibrary2.Classes;

namespace WcfServiceLibrary2.Classes
{
    public class Photos
    {
        [Key]
        public int PhotoID { get; set; }
        public int UserID { get; set; }
        public string Photo { get; set; }
    }
}
