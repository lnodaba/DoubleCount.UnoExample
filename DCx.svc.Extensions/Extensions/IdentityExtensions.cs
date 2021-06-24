using System;
using System.Collections.Generic;

public static partial class IdentityExtensions 
{
    #region GetOidcValue

    public static string GetOidcValue(this string jsonString, string oidcConstant)
    {
        #region quick search

        string  oidcValue   = null;
        string  keySearch   = oidcConstant == "expires_in" ? $"\"{oidcConstant}\":" : $"\"{oidcConstant}\":\"";
        string  endIndex    = oidcConstant == "expires_in" ? "," : "\"";
        int     posStart    = jsonString.IndexOf(keySearch);

        if (posStart > -1)
        {
            posStart += keySearch.Length;
            int posEnd = jsonString.IndexOf(endIndex, posStart);
            if (posEnd > posStart)
            {
                oidcValue = jsonString.Substring(posStart, (posEnd - posStart));
            }
        }
        #endregion

        #region // logical traversal

        //if (result == null)
        //{
        //    string[] jsonProps  = jsonString.Substring(1, jsonString.Length - 2).Split(',');

        //    for (int i=0; i<jsonProps.Length; i++)
        //    {
        //        string[] jsonEntry = jsonProps[i].Split(':');
        //        string   jsonKey   = jsonEntry[0].Substring(1, jsonEntry[0].Length-2);

        //        if (jsonKey == oidcConstant)
        //        {
        //            result = jsonEntry[1].Substring(1, jsonEntry[1].Length-2);   
        //            break;
        //        }
        //    }
        //}
        #endregion

        return oidcValue;
    }

    #endregion
}
