using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

public static class ByteArrayExtensions 
{
    [DebuggerStepThrough]
    public static Guid ToGuid(this byte[] value)
    {
        if (value == null || value.Length != 16)
        {
            return Guid.Empty;
        }
        else
        {
            return new Guid(value);
        }
    }

    [DebuggerStepThrough]
    public static byte[] ToASCII    (this   string stringVal)   => stringVal != null ? ASCIIEncoding.ASCII.GetBytes(stringVal) : null;


    [DebuggerStepThrough]
    public static byte[] ToUTF8     (this   string stringVal)   => stringVal != null ? Encoding.UTF8.GetBytes(stringVal) : null;
    [DebuggerStepThrough]
    public static byte[] ToUTF8     (this   string stringVal,
                                            int index,
                                            int count)          => stringVal != null ? Encoding.UTF8.GetBytes (stringVal.Substring(index, count)) : null;

    [DebuggerStepThrough]
    public static string FromASCII  (this   byte[] byteArray)   => byteArray != null ? ASCIIEncoding.ASCII.GetString(byteArray) : null;

    [DebuggerStepThrough]
    public static string FromUTF8   (this   byte[] byteArray)   => byteArray != null ? Encoding.UTF8.GetString(byteArray) : null;
    [DebuggerStepThrough]
    public static string FromUTF8   (this   byte[] byteArray,
                                            int index,
                                            int count)          => byteArray != null ? Encoding.UTF8.GetString(byteArray, index, count) : null;


    [DebuggerStepThrough]
    public static void  FillPart    (this   byte[] byteArray, 
                                            int position,
                                            byte[] srcArray)    =>  Buffer.BlockCopy(srcArray, 0, byteArray, position, srcArray.Length);

    [DebuggerStepThrough]
    public static byte[] Extract    (this  byte[] byteArray, int index, int count)
    {
        byte[] result = new byte[count];
        Buffer.BlockCopy(byteArray, index, result, 0, count);
        return result;
    }

    [DebuggerStepThrough]
    public static byte[] MergeWith(this byte[] byteArray, params byte[][] byteParams)
    {
        int totLength = byteArray.Length;
        int posOffset = totLength;
        int itmLength = 0;

        for (int i=0; i< byteParams.Length; i++)
        {
            totLength += (byteParams[i]?.Length ?? 0);
        }

        byte[]  rawBytes = new byte[totLength];
                rawBytes.FillPart(0, byteArray);
                
        for (int i=0; i< byteParams.Length; i++)
        {
            itmLength = (byteParams[i]?.Length ?? 0);

            if (itmLength > 0)
            {
                rawBytes.FillPart(posOffset, byteParams[i]);
                posOffset += itmLength;
            }
        }
        return rawBytes;
    }

}

