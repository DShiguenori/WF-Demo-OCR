using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Google.Cloud.Vision.V1;

namespace WF_Demo_OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void butOCR_Click(object sender, EventArgs e)
        {
            try
            {
                
                OpenFileDialog openFileDialog = new OpenFileDialog();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtResult.AppendText(openFileDialog.FileName);

                    var client = ImageAnnotatorClient.Create();

                    var image = Image.FromFile(openFileDialog.FileName);

                    var response = client.DetectText(image);

                    foreach (var annotation in response)
                    {
                        if (annotation.Description != null)
                        {
                            txtResult.AppendText(annotation.Description);

                            foreach (var ver in annotation.BoundingPoly.Vertices)
                            {
                                txtResult.AppendText("\t x: " + ver.X + ", y: " + ver.Y);
                            }
                            txtResult.AppendText("\r\n");
                        }
                    }

                    var response2 = client.DetectDocumentText(image);

                    TextAnnotation myObject = new TextAnnotation();
                    // Insert code to set properties and fields of the object.  
                    XmlSerializer mySerializer = new XmlSerializer(typeof(TextAnnotation));

                    using (StringWriter textWriter = new StringWriter())
                    {
                        mySerializer.Serialize(textWriter, response2);
                        txtLocale.AppendText( textWriter.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.Message;
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string credential_path = @"C:\Estudo\@GoogleCloud\Credential\OWTracker-9b0d94c8daf7.json";
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);
        }
    }
}
