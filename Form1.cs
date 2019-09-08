using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.HtmlConverter;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spirepdfapi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width = 900;
            pdfViewer2.Width = Convert.ToInt32(this.Width * 0.9);
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        void println(object s) => Console.WriteLine(s.ToString());

        async private void Button1_Click(object sender, EventArgs e)
        {
            string URL = textBox1.Text;
            string pathToFile = "";
            await Task.Run(() =>
            {
                pathToFile = createPdfDocFromURL(URL, "docFromUrl");
            });
            pdfViewer2.LoadFromFile(pathToFile);
        }

        private void PdfViewer1_Click(object sender, EventArgs e)
        {
            
        }
        String createPdfDocFromURL(string url, string outputName)
        {
            //Create a pdf document.
            PdfDocument doc = new PdfDocument();

            PdfPageSettings setting = new PdfPageSettings();

            setting.Size = new SizeF(1000, 1000);
            setting.Margins = new Spire.Pdf.Graphics.PdfMargins(20);

            PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
            htmlLayoutFormat.IsWaiting = true;

            Thread thread = new Thread(() =>
            { doc.LoadFromHTML(url, false, false, false, setting, htmlLayoutFormat); });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            //Save pdf file.
            doc.SaveToFile($"{outputName}.pdf");
            doc.Close();
            return $"{outputName}.pdf";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        async private void OpenFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await Task.Run(() =>
            {
                // Create a pdf document with a section and page added.
                PdfDocument doc = new PdfDocument();
                PdfSection section = doc.Sections.Add();
                PdfPageBase page = doc.Pages.Add();
                //Load a tiff image from system
                PdfImage image =
                    PdfImage.FromFile(
                        System.Text.RegularExpressions.Regex.Escape(
                            openFileDialog1.FileName).Replace(@"\.", "."));
                //Set image display location and size in PDF
                float widthFitRate = image.PhysicalDimension.Width / page.Canvas.ClientSize.Width;
                float heightFitRate = image.PhysicalDimension.Height / page.Canvas.ClientSize.Height;
                float fitRate = Math.Max(widthFitRate, heightFitRate);
                float fitWidth = image.PhysicalDimension.Width / fitRate;
                float fitHeight = image.PhysicalDimension.Height / fitRate;
                page.Canvas.DrawImage(image, 30, 30, fitWidth, fitHeight);
                doc.SaveToFile("imageToPdf.pdf");
                doc.Close();
            });
            pdfViewer2.LoadFromFile("imageToPdf.pdf");
        }
    }
}
