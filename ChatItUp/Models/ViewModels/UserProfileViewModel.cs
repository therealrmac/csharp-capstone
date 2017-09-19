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

        public bool checkconnectedbutnotconfirmed { get; set; }
        public bool checkConnectedAndConfirmed { get; set; }
        public bool checkConnectedAndConfirmed2 { get; set; }
        public List<Relation> friendList { get; set; }
        public List<Relation> friendList2 { get; set; }
        public List<Relation> friendList3 { get; set; }
        public List<Relation> friendList4 { get; set; }
        public List<Thread> totalThreads { get; set; }
        public IEnumerable<ThreadPost> totalPosts { get; set; }
    }
}
