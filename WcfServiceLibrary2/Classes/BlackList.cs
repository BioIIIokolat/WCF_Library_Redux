using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    public class BlackList
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int UserEnemyID { get; set; }
    }
}
