using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace src.Classes
{
    public class Decrypt
    {
        private TextBox textBox2;

        public Decrypt(TextBox textBox2)
        {
            this.textBox2 = textBox2;
        }
        public void DecryptFile(string selectedFilePath)
        {
            if(!string.IsNullOrEmpty(selectedFilePath))
            {
                try
                {
                    string keyText = textBox2.Text;

                    if(string.IsNullOrEmpty(keyText))
                    {
                        MessageBox.Show("Please enter the AES key in the textbox.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    byte[] key = Encoding.UTF8.GetBytes(keyText);
                    Array.Resize(ref key, 32);

                    using(AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                    {
                        aesProvider.Key = key;

                        using(FileStream encryptedFileStream = new FileStream(selectedFilePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] iv = new byte[aesProvider.IV.Length];
                            encryptedFileStream.Read(iv, 0, iv.Length);

                            using(CryptoStream cryptoStream = new CryptoStream(encryptedFileStream, aesProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                            {
                                using(FileStream decryptedFileStream = new FileStream(selectedFilePath.Replace(".Encrypted", "_Decrypted"), FileMode.Create, FileAccess.Write))
                                {
                                    cryptoStream.CopyTo(decryptedFileStream);
                                }
                            }
                        }
                    }
                    MessageBox.Show("File decrypted successfully!", "Decryption Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please choose an encrypted executable file first.", "File Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
