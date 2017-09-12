using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public ICollection<Forum> Forum { get; set; }
    }
}
