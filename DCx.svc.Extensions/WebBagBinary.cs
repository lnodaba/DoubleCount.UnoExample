using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DCx.Messaging
{
    public struct WebBagBinary
    {
        public  bool            IsSuccess   => this.HttpStatus == HttpStatusCode.OK;
        public  Http​Status​Code  Http​Status;
        public  byte[]          Result;
        public  string          Error;

        public WebBagBinary(Http​Status​Code httpStatus, byte[] result = null, string error = null)
        {
            this.Http​Status = httpStatus;
            this.Result     = result;
            this.Error      = error;
        }

        public WebBagBinary(byte[] rawBytes)
        {
            int lenBytes    = rawBytes?.Length ?? 0;
            int lenHeader   = sizeof(Int32);
            int lenContent  = lenBytes - lenHeader;

            if (lenBytes < lenHeader)
            {
                this.Http​Status = Http​Status​Code.Unused;
                this.Result     = null;
                this.Error      = null;
            }
            else
            { 
                this.Http​Status = (Http​Status​Code)BitConverter.ToInt32(rawBytes, 0);
                if (this.HttpStatus == HttpStatusCode.OK)
                {
                    this.Result = rawBytes.Extract (lenHeader, lenContent);
                    this.Error  = null;
                }
                else
                {
                    this.Result = null;
                    this.Error  = rawBytes.FromUTF8(lenHeader,  lenContent);
                }
            }
        }

        public byte[] GetBytes()
        {
            byte[]  rawHeader   = BitConverter.GetBytes((Int32)this.HttpStatus);
            byte[]  rawContent  = this.IsSuccess ? this.Result : this.Error.ToUTF8();
            byte[]  rawBytes    = rawHeader.MergeWith(rawContent);

            return rawBytes;
        }

        public string ErrorMsg  => $"{this.HttpStatus.ToString()}:{this.Error}";
    }
}
