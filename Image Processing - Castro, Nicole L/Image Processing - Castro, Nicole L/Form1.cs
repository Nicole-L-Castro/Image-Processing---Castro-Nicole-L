using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCamLib;
using AForge.Video;
using AForge.Video.DirectShow;

namespace Image_Processing___Castro__Nicole_L
{
    public partial class Form1 : Form
    {
        Bitmap loadImage;
        Bitmap imageB;
        Bitmap imageA;
        Bitmap colorgreen;
        Bitmap resultImage;

        bool isGrey = false;
        bool isSubtract = false;
        bool isInvert = false;
        public Form1()
        {
            InitializeComponent();
        }
        //FilterInfoCollection filterInfoCollection;
        //VideoCaptureDevice videoCaptureDevice;
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadImage;
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog2.FileName);
            pictureBox3.Image = imageB;
        }

        private void openFileDialog3_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog3.FileName);
            pictureBox4.Image = imageA;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            resultImage.Save(saveFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e) //COPY
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for(int i=0; i<resultImage.Width; i++)
            {
                for(int j=0; j<resultImage.Height; j++)
                {
                    Color pixel = loadImage.GetPixel(i, j);
                    resultImage.SetPixel(i, j, pixel); 
                }
                pictureBox2.Image = resultImage;
            }
        }

        private void button3_Click(object sender, EventArgs e) //GRAYSCALE
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < resultImage.Width; i++)
            {
                for (int j = 0; j < resultImage.Height; j++)
                {
                    Color pixel = loadImage.GetPixel(i, j);
                    int gray = (pixel.R + pixel.G + pixel.B) / 3;
                    resultImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
                pictureBox2.Image = resultImage;
            }
        }

        private void button4_Click(object sender, EventArgs e) //INVERSION
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < resultImage.Width; i++)
            {
                for (int j = 0; j < resultImage.Height; j++)
                {
                    Color pixel = loadImage.GetPixel(i, j);
                    resultImage.SetPixel(i, j, Color.FromArgb(255-pixel.R, 255-pixel.G, 255-pixel.B));
                }
                pictureBox2.Image = resultImage;
            }
        }

        private void button5_Click(object sender, EventArgs e) //SEPIA
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for (int i = 0; i < resultImage.Width; i++)
            {
                for (int j = 0; j < resultImage.Height; j++)
                {
                    Color pixel = loadImage.GetPixel(i, j);

                    int sepiaR = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int sepiaG = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int sepiaB = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    resultImage.SetPixel(i, j, Color.FromArgb(Math.Min(255, sepiaR), Math.Min(255, sepiaG), Math.Min(255, sepiaB)));
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void button6_Click(object sender, EventArgs e) //HISTOGRAM
        {
            resultImage = new Bitmap(loadImage.Width, loadImage.Height);
            for(int i = 0;i < loadImage.Width; i++)
                for(int j = 0;j < loadImage.Height; j++)
                {
                    Color pixel = loadImage.GetPixel(i, j);
                    int gray = (pixel.R + pixel.G + pixel.B) / 3;
                    resultImage.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            Color sample;
            int[] histData = new int[256];
            for(int i = 0;i < loadImage.Width; i++)
                for(int j = 0;j < loadImage.Height; j++)
                {
                    sample = resultImage.GetPixel(i, j);
                    histData[sample.R] = histData[sample.R] + 1;
                }
            Bitmap myData = new Bitmap(256, 800);
            for(int i = 0;i < 256;i++)
                for(int j = 0;j < 800; j++)
                {
                    myData.SetPixel(i, j, Color.White);
                }
            for (int i = 0; i < 256; i++)
                for (int j = 0; j < Math.Min(histData[i]/5, 800); j++)
                {
                    myData.SetPixel(i, 799-j, Color.Black);
                }
            pictureBox2.Image = myData;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        // WEBCAM & SUBTRACTION
        private void button10_Click(object sender, EventArgs e) // IMAGE SUBTRACTION
        {
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int threshold = 5;

            resultImage = new Bitmap(imageB.Width, imageB.Height);

            for (int i = 0; i < imageB.Width; i++)
                for( int j = 0; j < imageB.Height; j++)
                {
                    Color pixel = imageB.GetPixel(i, j);
                    Color back = imageA.GetPixel(i, j);                                                            
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue < threshold)
                        resultImage.SetPixel(i, j, back);
                    else
                        resultImage.SetPixel(i, j, pixel);
                }
            pictureBox2.Image = resultImage;
        }

        Device[] devices = DeviceManager.GetAllDevices();
        Device webcam = DeviceManager.GetDevice(0);

        private void button11_Click(object sender, EventArgs e) // ON WEBCAM
        {
            webcam.ShowWindow(pictureBox5);
        }

        private void button12_Click(object sender, EventArgs e) // OFF WEBCAM
        {
            webcam.Stop();
        }

        private void button13_Click(object sender, EventArgs e) // VID SUB
        {
            isSubtract = !isSubtract;
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));

            if (imageB != null && bmap.Size == imageB.Size)
            {
                timer1.Enabled = isSubtract;
            }
            else
            {
                Console.WriteLine("Background is null or not the same resolution as webcam");
            }
        }

        private void timer1_Tick(object sender, EventArgs e) //VID SUB2
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            int threshold = 5;

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);
                    ImageProcess2.BitmapFilter.Subtract(b, imageA, Color.Green, threshold);
                    pictureBox6.Image = b;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e) // VID GRAY
        {
            isGrey = !isGrey;
            timer2.Enabled = isGrey;
            timer1.Enabled = false;
            timer3.Enabled = false;
            button2.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e) // VID GRAY2
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);
                    ImageProcess2.BitmapFilter.GrayScale(b);
                    pictureBox6.Image = b;
                }
            }
        }

        private void button15_Click(object sender, EventArgs e) // VID INVERT
        {
            isInvert = !isInvert;
            timer3.Enabled = false;
            timer1.Enabled = false;
            timer3.Enabled = isInvert;
            button2.Enabled = false;
        }

        private void timer3_Tick(object sender, EventArgs e) // VID INVERT
        {
            IDataObject data;
            Image bmap;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            if (data != null)
            {
                bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
                if (bmap != null)
                {
                    Bitmap b = new Bitmap(bmap);
                    ImageProcess2.BitmapFilter.Invert(b);
                    pictureBox6.Image = b;
                }
            }
        }
    }
}
