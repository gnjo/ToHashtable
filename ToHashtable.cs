
namespace StringExtensions{
    using System.Collections;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    static class StringExtension
    {
        public static Hashtable ToHashtable(this string me)
        {
            return _to(me);
        }
        public static Hashtable ToHashtable(this string me, string head)
        {
            return _to(_head(me, head));
        }

        static string _head(string me, string head)
        {
            var ret = "";
            if (!ishead(head)) head = "[" + head + "]";
            var CR = Environment.NewLine;
            bool flg = false;
            var ary = me.Split(CR);
            foreach (var buf in ary)
            {
                if (buf.Trim() == head) { flg = true; continue; }
                if (!flg) continue;
                if (ishead(buf)) break;
                ret += buf + CR;
            }
            return ret;
            //
            bool ishead(string s) => new Regex(@"^\[").IsMatch(s);
        }
        static Hashtable _to(string str)
        {
            var ret = new Hashtable();
            string pattern = @"(\w.*)=\.([\s\S]*?)END|(\w.*)=(.*)";
            string input = str;

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                var key = "";
                var value = "";
                //Console.WriteLine(":" + match.Groups[0].Value);
                if (match.Groups[1].Value == "")
                {
                    //use 3,4
                    key = match.Groups[3].Value;
                    value = match.Groups[4].Value;
                }
                else
                {
                    //use 1,2
                    key = match.Groups[1].Value;
                    value = match.Groups[2].Value;
                }
                key = key.Trim();
                value = value.Trim();
                //Console.WriteLine($"{key}={value}");
                ret[key] = f(value);
            }
            return ret;
            ////////////////
            object f(string n)
            {
                if (new Regex(@"^0\d").IsMatch(n)) return n;
                if (int.TryParse(n, out int a)) return a;
                if (float.TryParse(n, out float b)) return b;
                return n;
            }

        }

/*usage
                var a=@"
[data1]  
//bbb
;aaa
aaa   =1
bbb=2
ccc=3
ddd=.
aiueo
kakikukeko
sasisuseso
naninuneno
END
eee=1000
fff=0.1
g=0001

[data2]
ffff=1

                ".ToHashtable("[data1]");

            foreach(var k in a.Keys){
                Console.WriteLine($"{k}={a[k]}");
            }


*/

    }



}
