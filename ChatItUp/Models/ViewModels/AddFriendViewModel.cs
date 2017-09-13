using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models.ViewModels
{
    public class AddFriendViewModel
    {
        public ApplicationUser User { get; set; }

        public List<Relation> relation { get; set; }
    }
}
