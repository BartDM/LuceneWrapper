using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneWrapper.TestApp
{
    public static class ExtensionMethods
    {
        public static StringBuilder AppendLineFormat(this StringBuilder sb, string format, dynamic val)
        {
            return sb.AppendLine(string.Format(format, val));
        }

        public static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object[] args)
        {
            return sb.AppendLine(string.Format(format, args));
        }
    }
}
