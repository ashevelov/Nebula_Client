using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIC
{
    public interface IPanel
    {
        IView IView
        {
            get;
            set;
        }
    }
}

