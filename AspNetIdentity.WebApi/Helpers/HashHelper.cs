using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using AspNetIdentity.WebApi.Controllers;

namespace AspNetIdentity.WebApi.Helpers
{
    public class HashHelper :BaseApiController
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}