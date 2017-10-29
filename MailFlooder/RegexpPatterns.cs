using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MailFlooder
{
    static class RegexpPatterns
    {
        public static string MailFormat { get; } = @"\S+@\S*";

        public static string HostFormat { get; } = @"(\S)+:(\d)+";
    }
}
