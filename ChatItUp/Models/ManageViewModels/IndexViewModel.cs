using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ChatItUp.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<ThreadPost> threadPost { get; set; }

        public ICollection<Thread> thread { get; set; }

        public ICollection<Relation> friends { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile profileImg { get; set; }


        [Display(Name = "Banner Image")]
        public IFormFile bannerImg { get; set; } 


        public List<Relation> friendList { get; set; }
        public List<ThreadPost>totalPosts { get; set; }
        public List<Relation> friendList2 { get; set; }
    }
}
