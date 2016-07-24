using System;
using System.Diagnostics;

namespace WPCore.Encryption
{
    public class DataEncryptor
    {
        public static string MD5Encode(string input)
        {
            try
            {
                return MD5Core.GetHashString(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MD5Encode: " + ex.Message);
            }

            return input;
        }

        public static string RC4Encode(string key, string input)
        {
            try
            {
                return BaseRC4.Encrypt(key, input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RC4Encode: " + ex.Message);
            }

            return input;
        }

        public static string RC4Decode(string key, string input)
        {
            try
            {
                return BaseRC4.Decrypt(key, input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("RC4Decode: " + ex.Message);
            }

            return input;
        }

        public static string Base32Encrypt(byte[] input)
        {
            try
            {
                return Base32.ToBase32String(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Base32Encrypt: " + ex.Message);
            }

            return string.Empty;
        }

        public static byte[] Base32Decrypt(string input)
        {
            try
            {
                return Base32.FromBase32String(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Base32Decrypt: " + ex.Message);
            }

            return null;
        }

        public static string TripleDESEncrypt(string key, string input)
        {
            try
            {
                return DESCryptography.TripleDESEncryptString(key, input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TripleDESEncrypt: " + ex.Message);
            }

            return input;
        }

        public static string TripleDESDecrypt(string key, string input)
        {
            try
            {
                return DESCryptography.TripleDESDecryptString(key, input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TripleDESDecrypt: " + ex.Message);
            }

            return input;
        }
    }
}
