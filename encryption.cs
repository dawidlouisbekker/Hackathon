using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.Json;

namespace HackaThon.encryption
{
    internal class SymmetricEncryption
    {
        byte[] Key = [];
        string IVec = " ";
        public  SymmetricEncryption()
        {
            GetIVect();
        }

        private async void GetIVect()
        {
            HttpClient _client = new HttpClient();

            string url = "http://13.246.49.85:8080/";

            HttpResponseMessage response = await _client.GetAsync(url);
            IVec = await response.Content.ReadAsStringAsync();

        }

        private async void GetSalt()
        {



            using (HttpClient _client = new HttpClient())
            {
                string url = "http://13.246.49.85:8080/salt";

                try
                {
                    HttpResponseMessage response = await _client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string JsonVariables = await response.Content.ReadAsStringAsync();
                        if (JsonVariables != null)
                        {
                            Variables variables = JsonSerializer.Deserialize<Variables>(JsonVariables);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occurred: " + ex.Message);
                }
            }
        }


        public byte[] EncryptString(string toEncrypt)
        {
            var key = GetKey(IVec);

            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(key, key))
            {
                var plainText = Encoding.Unicode.GetBytes(toEncrypt);
                return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
            }

        }

        public string DecryptToStringAsync(byte[] encryptedData)
        {
            var key = GetKey(IVec);

            using (var aes = Aes.Create())
            using (var decryptor = aes.CreateDecryptor(Encoding.Unicode.GetBytes(IVec), Encoding.Unicode.GetBytes(IVec))) //changed to unicode
            {
                var decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.Unicode.GetString(decryptedBytes);
            }
        }


        private static byte[] GetKey(string iv)
        {

            var keyBytes = Encoding.Unicode.GetBytes(iv);
            using (var md5 = MD5.Create())  //using sha256 stops the call to the login endpoint to be made for some reason
            {
                return md5.ComputeHash(keyBytes);
            }
        }

    }

}

public class Variables()
{
    int a;
    int b;
    int c;
    int x;
    int context;
}
