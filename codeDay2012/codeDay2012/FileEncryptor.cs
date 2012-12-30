using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace codeDay2012
{
    class FileEncryptor
    {
        private byte[] IV;
        private byte[] key;

        public FileEncryptor(string username, string password)
        {
            //key generation
            Random rand = new Random();
            Rfc2898DeriveBytes keyDev = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(username));

            //setup IV and key
            IV = keyDev.GetBytes(16);
            key = keyDev.GetBytes(16);
        }

        public void encrypt(string str, string path)
        {
            RijndaelManaged crypto = new RijndaelManaged();
            crypto.Key = key;
            crypto.IV = IV;

            ICryptoTransform encryptor = crypto.CreateEncryptor(key, IV);
            MemoryStream memStream = new MemoryStream();
            CryptoStream Crypto_Stream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);

            byte[] strBytes = Encoding.Unicode.GetBytes(str);
            Crypto_Stream.Write(strBytes, 0, strBytes.Length);

            Crypto_Stream.Close();
            
            //write to file
            SoapHexBinary bytes = new SoapHexBinary(memStream.ToArray());
            StreamWriter writer = new StreamWriter(path);
            writer.Write(bytes.ToString());
            writer.Close();
        }

        public string decrypt(string path)
        {
            //read the file's content
            StreamReader file = new StreamReader(path);
            string cipher_txt = file.ReadToEnd();
            file.Close();

            SoapHexBinary bytes = SoapHexBinary.Parse(cipher_txt);
            //------------------------

            StreamReader reader;
            byte[] strBytes = bytes.Value;
            string result;

            RijndaelManaged crypto = new RijndaelManaged();
            crypto.Key = key;
            crypto.IV = IV;

            ICryptoTransform decryptor = crypto.CreateDecryptor(key, IV);
            MemoryStream memStream = new MemoryStream(strBytes);
            CryptoStream Crypto_Stream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read);

            reader = new StreamReader(Crypto_Stream);
            result = trimChar(reader.ReadToEnd());

            Crypto_Stream.Close();
            reader.Close();

            return result;
        }

        public string trimChar(string str)
        {
            string ret = "";
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] != '\0'))
                    ret = ret + str[i];
            }
            return ret;
        }
    }
}
