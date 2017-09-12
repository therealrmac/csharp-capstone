using ChatItUp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatItUp.Models.ViewModels
{
    public class ThreadCreateViewModel
    {
        public Thread thread { get; set; }
        public ThreadCreateViewModel(ApplicationDbContext ctx)
        {

        }
    }
}
