using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models.ViewModels
{
    public class UserListViewModel
    {
        public IEnumerable<ApplicationUser> user { get; set; }
        public IEnumerable<Forum> forum { get; set; }
    }
}
