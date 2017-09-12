using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models
{
    public class Relation
    {
        [Key]
        public int RelationshipId { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public ApplicationUser Friend { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ConnectedOn { get; set; }

        public bool? Connected { get; set; }
    }
}
