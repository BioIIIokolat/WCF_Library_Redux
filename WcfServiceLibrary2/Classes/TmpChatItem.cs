using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary2.Classes
{
    [DataContract]
    public class TmpChatItem
    {
        [DataMember]
        public int Chatid { get; set; }

        [DataMember]
        public byte[] ImagePath { get; set; }
        [DataMember]
        public List<tmpMessage> messages { get; set; }
        [DataMember]
        public string LastMessage { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
}
