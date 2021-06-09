using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joshua.Reflection
{
    public static class Reflection
    {
        public static bool HasProperty(this Type model, string property)
        {
            return model.GetProperty(property) != null;
        }
    }
}
