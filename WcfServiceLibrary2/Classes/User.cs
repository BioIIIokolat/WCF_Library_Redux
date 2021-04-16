using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WcfServiceLibrary2.Classes;

namespace WcfServiceLibrary2
{
   public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> Hobbies { get; set; }

        public List<string> Photos { get; set; }

        public string Avatarka { get; set; }

        public string Description { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public string ColorHairCut { get; set; }

        public string ColorEye { get; set; }

        public string Faith { get; set; }

        public List<string> Education { get; set; }

        public string Job { get; set; }

        public string Gender { get; set; }

        public string Orientation { get; set; }

        public int Count_Liked { get; set; }

        public List<Likes> Likes { get; set; }

        //public  List<int> Disliked { get; set; }

        public List<Datings> Datings { get; set; }

        [DataMember]
        public ICollection<ChatItemUsers> chatItems { get; set; }

        public int Age { get; set; }
    }
    
}
