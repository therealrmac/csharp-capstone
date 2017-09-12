using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models
{
    public class Forum
    {
        [Key]
        public int ForumId { get; set; }
        public string ThreadTitles { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public ICollection<Thread> thread { get; set; }
    }
}
