using EIPMonitor.Domain.CustomException;
using EIPMonitor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EIPMonitor.DomainServices.UserService
{
    public static class UserEncryptionService
    {
        /// <summary>
        ///  Encrypt the cleartextPassword with securityStamp
        /// </summary>
        /// <param name="securityStamp">salt</param>
        /// <param name="cleartextPassword">password</param>
        /// <returns>a encrypted password</returns>
        /// <exception cref="EIPMonitor.Domain.CustomException.TheSecurityStampCanNotBeEmptyException">The Argument securityStamp is null or white spaces</exception>
        /// <exception cref="EIPMonitor.Domain.CustomException.ThePasswordCanNotBeEmptyException">The Argument cleartextPassword is null or white spaces</exception>
        public static String Encrypt(String securityStamp, String cleartextPassword)
        {
            if (String.IsNullOrWhiteSpace(securityStamp)) throw new TheSecurityStampCanNotBeEmptyException();
            if (String.IsNullOrWhiteSpace(cleartextPassword)) throw new ThePasswordCanNotBeEmptyException();
            Byte[] concatCleartextPasswordWithSalt = Encoding.UTF8.GetBytes(String.Format("{0}{1}", cleartextPassword, securityStamp));
            SHA512Managed encryptor = new SHA512Managed();
            Byte[] encryptedPassword = encryptor.ComputeHash(concatCleartextPasswordWithSalt);
            return BitConverter.ToString(encryptedPassword);
        }

        public static String Encrypt(EIPProductionIndexUsers eIPProductionIndexUsers, String cleartextPassword)
        {
            if (String.IsNullOrWhiteSpace(eIPProductionIndexUsers.EmployeeId)) throw new TheSecurityStampCanNotBeEmptyException();
            if (String.IsNullOrWhiteSpace(cleartextPassword)) throw new ThePasswordCanNotBeEmptyException();
            Byte[] concatCleartextPasswordWithSalt = Encoding.UTF8.GetBytes(String.Format("{0}{1}{2}", cleartextPassword, eIPProductionIndexUsers.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss"), eIPProductionIndexUsers.EmployeeId));
            SHA512Managed encryptor = new SHA512Managed();
            Byte[] encryptedPassword = encryptor.ComputeHash(concatCleartextPasswordWithSalt);
            return BitConverter.ToString(encryptedPassword);
        }

        public static String MD5Encrypt(String securityStamp, String cleartextPassword)
        {
            if (String.IsNullOrWhiteSpace(securityStamp)) throw new TheSecurityStampCanNotBeEmptyException();
            if (String.IsNullOrWhiteSpace(cleartextPassword)) throw new ThePasswordCanNotBeEmptyException();
            Byte[] concatCleartextPasswordWithSalt = Encoding.UTF8.GetBytes(String.Format("{0}{1}", cleartextPassword, securityStamp));
            MD5Cng encryptor = new MD5Cng();
            Byte[] encryptedPassword = encryptor.ComputeHash(concatCleartextPasswordWithSalt);
            return BitConverter.ToString(encryptedPassword).Replace("-", String.Empty).ToLower();
        }
    }
}
