using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rg.Forms.ThreadView.EventArgs
{
    public class ThreadViewInternalExceptionArgs
    {
        public Exception Exception { get; }

        public ThreadViewInternalExceptionArgs(Exception e)
        {
            Exception = e;
        }
    }
}
