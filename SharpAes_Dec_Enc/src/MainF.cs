using src.Classes;
using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace src
{
    public partial class MainF : Form
    {
        public bool method = true;
        public string selectedFilePath;
        private Encrypt encrypt;
        private Decrypt decrypt;
        public MainF()
        {
            InitializeComponent();
            Load += LoadFrm;
            encrypt = new Encrypt(textBox2);
            decrypt = new Decrypt(textBox2);
            radioButton1.CheckedChanged += Rdbuttons;
            radioButton2.CheckedChanged += Rdbuttons;
            button2.MouseClick += GenerateKeyBTN;
            button3.MouseClick += ChoseFl;
            button1.MouseClick += ActionBTN;
        }
        private void ActionBTN(object sender, EventArgs e)
        {
            if(method == true)
            {
                encrypt.EncryptFile(selectedFilePath);
            }
            else
            {
                decrypt.DecryptFile(selectedFilePath);
            }
        }
        private void LoadFrm(object sender, EventArgs e)
        {
            button1.Text = "Encrypt File";
            radioButton1.Enabled = true;
        }
        private void ChoseFl(object sender, EventArgs e)
        {
            using(OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "All Files|*.*|Encrypted Files|*.Encrypted";
                ofd.Title = "Choose an executable file";

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    selectedFilePath = ofd.FileName;
                    textBox1.Text = selectedFilePath;
                }
            }
        }
        private void Rdbuttons(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                method = true;
                button1.Text = "Encrypt File";
            }
            else if(radioButton2.Checked)
            {
                method = false;
                button1.Text = "Decrypt File";
            }
        }
        private void GenerateKeyBTN(object sender, EventArgs e)
        {
            string generatedKey = GenAESKey();
            textBox2.Text = generatedKey;
        }
        private string GenAESKey()
        {
            using(AesCryptoServiceProvider aesprov = new AesCryptoServiceProvider())
            {
                aesprov.GenerateKey();
                return Convert.ToBase64String(aesprov.Key);
            }
        }
    }
}
