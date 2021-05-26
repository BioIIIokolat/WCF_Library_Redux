using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    //свидание
   public class Datings
    {
        [Key]
        public int DatingId { get; set; }
        [DataMember]
        public DateTime  StartTime { get; set; }
        [DataMember]
        public User male { get; set; }
        [DataMember]
        public User female { get; set; }
        public bool IsAcсeptedByFirst { get; set; }
        [DataMember]
        public bool IsAcсeptedBySecond { get; set; }
        [DataMember]
        public string Address { get; set; }
    }
}
