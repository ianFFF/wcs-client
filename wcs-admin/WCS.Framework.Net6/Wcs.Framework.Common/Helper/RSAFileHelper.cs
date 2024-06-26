﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.Framework.Common.Helper
{
    public class RSAFileHelper
    {
        public static RSA GetKey()
        {
            return GetRSA("key.pem");
        }
        public static RSA GetPublicKey()
        {
            return GetRSA("public.pem");
        }

        private static RSA GetRSA(string fileName)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string filePath = Path.Combine(rootPath, fileName);
            if (!System.IO.File.Exists(filePath))
                throw new Exception("文件不存在");
            string key = System.IO.File.ReadAllText(filePath);

            #region windows server 使用
            //CspParameters csp = new CspParameters();
            //csp.Flags = CspProviderFlags.UseMachineKeyStore;
            //var _rsaP = new RSACryptoServiceProvider(1024, csp);

            //_rsaP.ImportFromPem(key.AsSpan());

            //return _rsaP;
            #endregion

            #region 其他环境使用
            var rsa = RSA.Create();
            rsa.ImportFromPem(key.AsSpan());
            return rsa;
            #endregion
        }
    }
}
