using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.ViewModels
{
    public class MainThreadViewModel
    {
        public string ThreadText { get; set; } = "Thread Text";
        public string MainThreadText { get; set; } = "Main Thread Text";
    }
}
