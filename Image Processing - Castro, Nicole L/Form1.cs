using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Processing___Castro__Nicole_L
{
    public partial class Form1 : Form
    {
        Bitmap loadImage;
        Bitmap resultImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
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
    }
}
