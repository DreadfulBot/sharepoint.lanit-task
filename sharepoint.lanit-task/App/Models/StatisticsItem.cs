using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace sharepoint.lanit_task.App.Models
{
    [DataContract]
    public class StatisticsItem
    {
        [DataMember]
        public string Value { get; set; }
        
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string User { get; set; }
    }
}
