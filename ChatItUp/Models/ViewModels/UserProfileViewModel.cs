using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public List<ThreadPost> threadPost { get; set; }
        public List<Thread> thread { get; set; }

        public ApplicationUser User { get; set; }

        public Relation relation { get; set; }
    }
}
