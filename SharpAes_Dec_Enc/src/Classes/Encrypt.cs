using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace src.Classes
{
    public class Encrypt
    {
        private TextBox textBox2;

        public Encrypt(TextBox textBox2)
        {
            this.textBox2 = textBox2;
        }

        public void EncryptFile(string selectedFilePath)
        {
            if(!string.IsNullOrEmpty(textBox2.Text))
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

                        using(FileStream inputFileStream = new FileStream(selectedFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using(FileStream encryptedFileStream = new FileStream(selectedFilePath + ".Encrypted", FileMode.Create, FileAccess.Write))
                            {
                                encryptedFileStream.Write(aesProvider.IV, 0, aesProvider.IV.Length);
                                using(CryptoStream cryptoStream = new CryptoStream(encryptedFileStream, aesProvider.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    inputFileStream.CopyTo(cryptoStream);
                                }
                            }
                        }
                    }
                    MessageBox.Show("File encrypted successfully!", "Encryption Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please choose a file first.", "File Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
