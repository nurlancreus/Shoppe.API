using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mock.ShippingProvider.Application.Interfaces.Services
{
    public interface IKeyGenerator
    {
        public static string GenerateApiKey()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));  // 256-bit key
        }

        public static string GenerateSecretKey()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));  // 512-bit key
        }
    }
}
