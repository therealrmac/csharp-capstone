using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models
{
    public class Thread
    {
        [Key]
        public int ThreadId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string AuthorMessage { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public int ForumId { get; set; }
        public Forum Forum { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime created { get; set; }

        public ICollection<ThreadPost> ThreadPost { get; set; }
    }
}
