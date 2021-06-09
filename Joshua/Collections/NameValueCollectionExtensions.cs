using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joshua.Collections
{
    public static class NameValueCollectionExtensions
    {
        public static IDictionary<string, object> ToDictionary(this NameValueCollection source)
        {
            return source.Keys.Cast<string>().ToDictionary<string, string, object>(key => key, key => source[key]);
        }
    }
}
