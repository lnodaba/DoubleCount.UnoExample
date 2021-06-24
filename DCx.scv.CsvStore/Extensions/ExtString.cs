using System;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;

public static partial class ExtString 
{
    private static byte[]   s_hashKey = Assembly.GetExecutingAssembly().GetType().GUID.ToByteArray();


    public static string    AsCsvField  (this string s)
    {
        var result = s;

        if (s.IsUsed())
        {
            result = result.Replace(';', '¦');
            result = result.Replace(Environment.NewLine, "¶");
        }

        return result;
    }

    public static string    AsHashBase64(this string s)
    {
        var result = default(string);

        if (s.IsUsed())
        {
            try
            {
                var input   = Encoding.UTF8.GetBytes(s);
                var output  = new HMACSHA256(s_hashKey).ComputeHash(input);
                    result  = Convert.ToBase64String(output);
            }
            catch { }
        }

        return result;
    }

}
