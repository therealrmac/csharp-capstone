using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatItUp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }

        public virtual ICollection<Thread> Threads { get; set; }
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; }

        public string ProfileImage { get; set; }
        public string BannerImage { get; set; }

        [InverseProperty("Friend")]
        public virtual ICollection<Relation> Friends { get; set; }
    }
}
