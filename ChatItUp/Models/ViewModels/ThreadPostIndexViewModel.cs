using ChatItUp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models.ViewModels
{
    public class ThreadPostIndexViewModel
    {
        public Thread Thread { get; set; }
        public List<ThreadPost> threadpost { get; set; }
       
    }
}
