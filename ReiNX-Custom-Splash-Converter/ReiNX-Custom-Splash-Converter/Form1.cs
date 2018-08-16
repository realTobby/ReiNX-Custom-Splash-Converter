using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReiNX_Custom_Splash_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select IMAGE you want to convert to splash.bin";
                dlg.Filter = "ALL FILES |*.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dlg.FileName;
                }
            }
            LoadImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap im = new Bitmap(textBox1.Text);
            Bitmap img = new Bitmap(im, 1280, 720);
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            img.RotateFlip(RotateFlipType.RotateNoneFlipY);

            List<byte> pixs = new List<byte>();

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    pixs.Add(Convert.ToByte(img.GetPixel(x, y).R));
                    pixs.Add(Convert.ToByte(img.GetPixel(x, y).G));
                    pixs.Add(Convert.ToByte(img.GetPixel(x, y).B));
                    pixs.Add(Convert.ToByte(0));
                }

                for (int x = 0; x < 48 * 4; x++)
                {
                    pixs.Add(Convert.ToByte(0));
                }

            }

            byte[] pixels = pixs.ToArray();


            byte[] data = Encoding.UTF8.GetBytes("00921600");
            try
            {
                using (FileStream fs = File.Create("splash.bin"))
                {
                    fs.Write(data, 0, data.Length);
                    fs.Write(pixels, 0, pixels.Length);
                }

                MessageBox.Show("Successfully created/wrote to splash.bin!");

            }catch(Exception ex)
            {
                MessageBox.Show("Tell someone this: " + ex.Message);
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                LoadImage();
        }

        private void LoadImage()
        {
            if (System.IO.File.Exists(textBox1.Text))
            {
                pictureBox1.Image = new Bitmap(textBox1.Text);
            }
        }
    }
}
