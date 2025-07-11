using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottReece.BorderCompare.App
{
    public static class Extensions
    {
        public static PointF GetCenter(this Rectangle rect)
        {
            return new PointF(rect.Left + rect.Width / 2f, rect.Top + rect.Height / 2f);
        }
    }
}
