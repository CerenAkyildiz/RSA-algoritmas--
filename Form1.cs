using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rsa2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var plainTextData = textBox1.Text;
            var csp = new RSACryptoServiceProvider(2048);


            var privKey = csp.ExportParameters(true);
            var pubKey = csp.ExportParameters(false);


            string pubKeyString;
            {

                var sw = new System.IO.StringWriter();
                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

                xs.Serialize(sw, pubKey);
                pubKeyString = sw.ToString();
            }


            {

                var sr = new System.IO.StringReader(pubKeyString);

                var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));

                pubKey = (RSAParameters)xs.Deserialize(sr);
            }


            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(pubKey);

            var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);


            var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);


            var cypherText = Convert.ToBase64String(bytesCypherText);

            label2.Text = cypherText;//RSA ya göre şifrelenmiş metin yazdırılıyor

            //Bu adımdan sonra RSA ya göre şifrelenen veri ,orjinal haline döndürülüyor 
            bytesCypherText = Convert.FromBase64String(cypherText);


            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privKey);

            bytesPlainTextData = csp.Decrypt(bytesCypherText, false);


            plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
            label4.Text = plainTextData;

        }
    }
}
