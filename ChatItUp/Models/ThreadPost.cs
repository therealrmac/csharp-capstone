using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models
{
    public class ThreadPost
    {
        [Key]
        public int ThreadPostId { get; set; }

        public string message { get; set; }

        [Required]
        public ApplicationUser user {get;set;}

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime dateCreatd { get; set; }

        public int ThreadId { get; set; }
        public Thread Thread { get; set; }

    }
}
