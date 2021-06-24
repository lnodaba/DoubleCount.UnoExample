using System;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Diagnostics;

public static partial class StringExtensions 
{
    #region Common string extensions

    [DebuggerStepThrough]
    public static bool   IsBlank        (this string stringVal) => (stringVal?.Length > 0) == false;
    [DebuggerStepThrough]
    public static bool   IsUsed         (this string stringVal) => (stringVal?.Length > 0) == true;
    [DebuggerStepThrough]
    public static string StringOrNull   (this string stringVal) => (!IsBlank(stringVal) ? stringVal : null);
    [DebuggerStepThrough]
    public static string StringOrEmpty  (this string stringVal) => (!IsBlank(stringVal) ? stringVal : String.Empty);
    [DebuggerStepThrough]
    public static string StringOrDefault(this string stringVal, string defaultVal) => (!IsBlank(stringVal) ? stringVal : defaultVal);

    [DebuggerStepThrough]
    public static bool StartsWithAny(this string stringVal, params string[] compares)
    {
        for ( int i = 0 ; i < compares.Length ; i++ )
        {
            if (stringVal.StartsWith(compares[i]))
            {
                return true;
            }
        }
        return false;
    }

    [DebuggerStepThrough]
    public static string Left(this string s, int count)
    {
        if (s == null)
        {
            return null;
        }

        if (s.Length >= Math.Abs(count) && count != 0)
        {
            if (count > 0)
            {
                return s.Substring(0, count);
            }
            else
            {
                return s.Substring(0, s.Length + count);
            }
        }
        else
        {
            return s + new String(' ', count - s.Length);
        }
    }

    [DebuggerStepThrough]
    public static string Right(this string s, int count)
    {
        if (s.IsUsed() && s.Length >= Math.Abs(count))
        {
            if (count > 0)
            {
                return s.Substring(s.Length - count, count);
            }
            else
            {
                //  that is weird
                return s.Substring(0, s.Length + count);
            }
        }
        else
        {
            return String.Empty;
        }
    }

    [DebuggerStepThrough]
    public static string TrimToMaxLength(this string value, int maxLength) 
    {
        return (value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength));
    }

    [DebuggerStepThrough]
    public static string TrimToNumberString(this string value)
    {
        if (value.IsBlank())
        {
            return value;
        }
        string _result = new String(value.Where(c => Char.IsDigit(c) || Char.IsPunctuation(c)).ToArray());
        return _result;
    }


    //  see connections
    [DebuggerStepThrough]
    public static Guid ToGuid(this string value)
    {
        Guid _result = Guid.Empty;

        try
        {
            if (value.Length == 32 || value.Length == 36)
            {
                _result = new Guid(value);
            }
            else if (value.Length > 36)
            {
                _result = new Guid(value.Right(32));
            }
        }
        catch
        {
            try
            {
                _result = new Guid(value.Right(32));
            }
            catch {}
        }
        return _result;
    }
    #endregion


    public static int       ToInteger       (this string s)
    {
        if (s.IsBlank())
        {
            return 0;
        }

        if (int.TryParse(s, out int result))
        {
            return result;
        }
        else
        {
            return 0;
        }
    }

    public static Guid      ConvertToGuid   (this string s)
    {
        if (s.IsBlank())
        {
            return Guid.Empty;
        }

        if (Guid.TryParse(s, out Guid result))
        {
            return result;
        }
        else
        {
            return Guid.Empty;
        }
    }

    public static bool      ToBool          (this string s)
    {
        if (s.IsUsed())
        {
            var sgn = s[0];
            return (sgn =='1' ||  sgn == 't' || sgn== 'T');
        }
        return false;
    }

    public static bool      ToBoolean       (this string s)
    {
        if (s.IsBlank())
        {
            return false;
        }

        if (bool.TryParse(s, out bool result))
        {
            return result;
        }
        else
        {
            return false;
        }
    }
    public static double    ToDouble        (this string s)
    {
        if (s.IsBlank())
        {
            return 0d;
        }

        if (Double.TryParse(s, out double result))
        {
            return result;
        }
        else
        {
            return 0d;
        }
    }

    public static DateTime ToDateTime       (this string s)
    {
        if (s.IsBlank())
            return DateTime.MinValue;

        if (double.TryParse(s, out double ticks))
            return new DateTime((long)ticks).ToLocalTime();

        return DateTime.MinValue;
    }
    public static string    AsEmail         (this string s)
    {
        var result = default(string);

        if (s.IsUsed())
        {
            try
            {
                result = new System.Net.Mail.MailAddress(s).Address;
            }
            catch { }
        }

        return result;
    }



    public static bool IsValidEmail(this string value)
    {
        if (value.IsBlank())
        {
            return false;
        }
        try
        {
            var     _mailAdr = new System.Net.Mail.MailAddress(value);
            return  _mailAdr.Address == value;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhone(this string value)
    {
        try
        {
            //  pm: can be sype name as well
            return value?.Length > 5;
        }
        catch
        {
            return false;
        }
    }


    [DebuggerStepThrough]
    public static string EncodeBase64(this string value) => value.EncodeBase64(null);

    [DebuggerStepThrough]
    public static string EncodeBase64(this string value, Encoding encoding)
    {
        encoding = (encoding ?? Encoding.UTF8);
        var bytes = encoding.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    [DebuggerStepThrough]
    public static string DecodeBase64(this string encodedValue) => encodedValue.DecodeBase64(null);

    [DebuggerStepThrough]
    public static string DecodeBase64(this string encodedValue, Encoding encoding)
    {
        encoding = (encoding ?? Encoding.UTF8);
        var bytes = Convert.FromBase64String(encodedValue);
        return encoding.GetString(bytes);
    }
}
