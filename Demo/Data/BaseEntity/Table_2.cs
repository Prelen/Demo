using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Data.BaseEntity
{
    [Table("Table_2")]
    public class Table_2
    {
        [Key]
        public int Table_1ID { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string company { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
}
