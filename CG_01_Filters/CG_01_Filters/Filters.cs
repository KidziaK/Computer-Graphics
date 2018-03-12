using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_01_Filters
{
    public enum Functions
    {
        Brightness,
        Contrast,
        Negation,
        GammaCorrection
    };

    public partial class Filters : Form
    {
        public double contrastValue = 1.5, gammaFactor = 1f;
        public int kernelH, kernelW, div, off;
        public Filters()
        {
            InitializeComponent();
            Choice.Items.Add(Functions.Brightness);
            Choice.Items.Add(Functions.Contrast);
            Choice.Items.Add(Functions.Negation);
            Choice.Items.Add(Functions.GammaCorrection);

            ClearKernel();

            Kernel.ColumnCount = 3;
            Kernel.RowCount = 3;
            Kernel.Size = new Size(50 * 3 + 5, 50 * 3 + 5);
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 0);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 0);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 0);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 1);
            Kernel.Controls.Add(new TextBox() { Text = "4", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, 1, 1);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 2);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 2);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 2);

            D.Text = "16";
            H.Text = "3";
            W.Text = "3";
            Off.Text = "0";

            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            foreach (TextBox space in Kernel.Controls)
            {
                space.MouseDoubleClick += new MouseEventHandler(ItemClick);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Images|*.png;*.bmp;*.jpg";
                ImageFormat format = ImageFormat.Png;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string ext = System.IO.Path.GetExtension(sfd.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }
                    Output.Image.Save(sfd.FileName, format);
                }
            }

            catch
            {

            }

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select a picture";
                dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Input.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Choice_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(Choice.SelectedItem)
            {
                case Functions.Brightness:
                    Track.Enabled = true;
                    Gamma.Enabled = false;
                    break;

                case Functions.Contrast:
                    Track.Enabled = false;
                    Gamma.Enabled = false;
                    break;

                case Functions.Negation:
                    Track.Enabled = false;
                    Gamma.Enabled = false;
                    break;
                case Functions.GammaCorrection:
                    Gamma.Enabled = true;
                    Track.Enabled = false;
                    break;
            }
        }

        private void Track_Scroll(object sender, EventArgs e)
        {

        }

        private void ChangeBrightness()
        {
            Bitmap outputImage = new Bitmap(Input.Image);
            int change = Track.Value;
            for (int x = 0; x < outputImage.Width; x++)
            {
                for (int y = 0; y < outputImage.Height; y++)
                {
                    
                    Color pixel = outputImage.GetPixel(x, y);
                    int red = pixel.R + change;
                    int green = pixel.G + change;
                    int blue = pixel.B + change;

                    if (red < 256 && red > 0) ;
                    else if (red > 255) red = 255;
                    else red = 0;

                    if (green < 256 && green > 0) ;
                    else if (green > 255) green = 255;
                    else green = 0;

                    if (blue < 256 && blue > 0) ;
                    else if (blue > 255) blue = 255;
                    else blue = 0;
                    outputImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            Output.Image = outputImage;
        }
        private void Negation()
        {
            Bitmap outputImage = new Bitmap(Input.Image);
            for(int x = 0; x<outputImage.Width; x++)
            {
                for (int y = 0; y < outputImage.Height; y++)
                {
                    Color pixel = outputImage.GetPixel(x, y);
                    outputImage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }
            }
            Output.Image = outputImage;
        }
        private void ChangeContrast()
        {
            Bitmap outputImage = new Bitmap(Input.Image);
            for (int x = 0; x < outputImage.Width; x++)
            {
                for (int y = 0; y < outputImage.Height; y++)
                {

                    Color pixel = outputImage.GetPixel(x, y);
                    int red = (int)(((double)pixel.R - 127) * contrastValue) + 127;
                    int green = (int)(((double)pixel.G - 127) * contrastValue) + 127;
                    int blue = (int)(((double)pixel.B - 127) * contrastValue) + 127;

                    if (red < 256 && red > 0) ;
                    else if (red > 255) red = 255;
                    else red = 0;

                    if (green < 256 && green > 0) ;
                    else if (green > 255) green = 255;
                    else green = 0;

                    if (blue < 256 && blue > 0) ;
                    else if (blue > 255) blue = 255;
                    else blue = 0;
                    outputImage.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            Output.Image = outputImage;
        }
        private void GammaCorrection()
        {
            Bitmap outputImage = new Bitmap(Input.Image);
            for (int x = 0; x < outputImage.Width; x++)
            {
                for (int y = 0; y < outputImage.Height; y++)
                {
                    Color pixel = outputImage.GetPixel(x, y);
                    outputImage.SetPixel(x, y, Color.FromArgb((int)(255*Math.Pow((double)pixel.R/255, gammaFactor)), (int)(255*Math.Pow((double)pixel.G/255, gammaFactor)), (int)(255*Math.Pow((double)pixel.B/255, gammaFactor))));
                }
            }
            Output.Image = outputImage;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            try
            {
                switch (Choice.SelectedItem)
                {
                    case Functions.Brightness:
                        ChangeBrightness();
                        break;

                    case Functions.Contrast:
                        ChangeContrast();
                        break;

                    case Functions.Negation:
                        Negation();
                        break;
                    case Functions.GammaCorrection:
                        GammaCorrection();
                        break;
                }

            }
            catch
            {

            }
        }

        private void ClearKernel()
        {
            Kernel.Controls.Clear();    
            Kernel.RowStyles.Clear();
            Kernel.ColumnStyles.Clear();

        }

        private void GAUSS_Click(object sender, EventArgs e)
        {
            ClearKernel();

            Kernel.ColumnCount = 3;
            Kernel.RowCount = 3;
            Kernel.Size = new Size(50 * 3 + 5, 50 * 3 + 5);
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 0);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 0);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 0);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 1);
            Kernel.Controls.Add(new TextBox() { Text = "4", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, 1, 1);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 2);
            Kernel.Controls.Add(new TextBox() { Text = "2", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 2);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 2);

            D.Text = "16";
            H.Text = "3";
            W.Text = "3";
            Off.Text = "0";

            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            foreach (TextBox space in Kernel.Controls)
            {
                space.MouseDoubleClick += new MouseEventHandler(ItemClick);
            }
        }

        private void HIGH_Click(object sender, EventArgs e)
        {
            ClearKernel();

            Kernel.ColumnCount = 3;
            Kernel.RowCount = 3;
            Kernel.Size = new Size(50 * 3 + 5, 50 * 3 + 5);
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 0);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 0);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 0);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 1);
            Kernel.Controls.Add(new TextBox() { Text = "5", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, 1, 1);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 1);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 2);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 2);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 2);

            D.Text = "1";
            H.Text = "3";
            W.Text = "3";
            Off.Text = "0";

            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            foreach (TextBox space in Kernel.Controls)
            {
                space.MouseDoubleClick += new MouseEventHandler(ItemClick);
            }
        }

        private void LOW_Click(object sender, EventArgs e)
        {
            ClearKernel();

            Kernel.ColumnCount = 3;
            Kernel.RowCount = 3;
            Kernel.Size = new Size(50 * 3 + 5, 50 * 3 + 5);
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 0);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 0);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 0);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, 1, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 2);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 2);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 2);

            D.Text = "9";
            H.Text = "3";
            W.Text = "3";
            Off.Text = "0";

            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            foreach (TextBox space in Kernel.Controls)
            {
                space.MouseDoubleClick += new MouseEventHandler(ItemClick);
            }
        }

        private void EMBOSS_Click(object sender, EventArgs e)
        {
            ClearKernel();

            Kernel.ColumnCount = 3;
            Kernel.RowCount = 3;
            Kernel.Size = new Size(50 * 3 + 5, 50 * 3 + 5);
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 0);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 0);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 0);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, 1, 1);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 1);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 2);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 2);
            Kernel.Controls.Add(new TextBox() { Text = "1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 2);

            D.Text = "1";
            H.Text = "3";
            W.Text = "3";
            Off.Text = "0";

            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            foreach (TextBox space in Kernel.Controls)
            {
                space.MouseDoubleClick += new MouseEventHandler(ItemClick);
            }
        }

        private void EDGE_Click(object sender, EventArgs e)
        {
            ClearKernel();

            Kernel.ColumnCount = 3;
            Kernel.RowCount = 3;
            Kernel.Size = new Size(50 * 3 + 5, 50 * 3 + 5);
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 0);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 0);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 0);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 1);
            Kernel.Controls.Add(new TextBox() { Text = "4", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, 1, 1);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 1);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 0, 2);
            Kernel.Controls.Add(new TextBox() { Text = "-1", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 1, 2);
            Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center }, 2, 2);

            D.Text = "1";
            H.Text = "3";
            W.Text = "3";
            Off.Text = "127";

            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            foreach (TextBox space in Kernel.Controls)
            {
                space.MouseDoubleClick += new MouseEventHandler(ItemClick);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                ClearKernel();
                int width, height;
                Int32.TryParse(H.Text, out height);
                Int32.TryParse(W.Text, out width);
                Kernel.ColumnCount = width;
                Kernel.RowCount = height;
                Kernel.Size = new Size(50 * width + 5, 50 * height + 5);
                for (int i = 0; i < width; i++)
                {
                    Kernel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50) );
                }
                for (int i = 0; i < height; i++)
                {
                    Kernel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                }               

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if(i == 0 && j ==0) Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center, BackColor = Color.YellowGreen }, i, j);
                        else Kernel.Controls.Add(new TextBox() { Text = "0", Anchor = AnchorStyles.Left, TextAlign = HorizontalAlignment.Center}, i, j);
                    }
                }

                foreach (TextBox space in Kernel.Controls)
                {
                    space.MouseDoubleClick += new MouseEventHandler(ItemClick);
                }

               


            }
            catch
            {
                
            }
        }

        private void ItemClick(object sender, EventArgs e)
        {
            foreach (TextBox space in Kernel.Controls)
            {
                space.BackColor = Color.White;
            }
            (sender as TextBox).BackColor = Color.YellowGreen;
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Gamma_ValueChanged(object sender, EventArgs e)
        {
            gammaFactor = Gamma.Value/10f;
        }

        private void Gamma_Scroll(object sender, EventArgs e)
        {
            
        }

        private Color [,] CopyColorArr(Bitmap output, int width, int height)
        {
            Color[,] ret = new Color[width, height];

            for(int x = 0; x<width; x++)
            {
                for(int y = 0; y<height; y++)
                {
                    ret[x, y] = output.GetPixel(x, y);
                }
            }

            return ret;
        }

        private void CApply_Click(object sender, EventArgs e)
        {
            Int32.TryParse(D.Text, out div);
            Int32.TryParse(H.Text, out kernelH);
            Int32.TryParse(W.Text, out kernelW);
            Int32.TryParse(Off.Text, out off);

            Point center = FindCenter();
            Bitmap output = new Bitmap(Input.Image);
            int width = output.Width, height = output.Height;
            Color [,] original = CopyColorArr(output, width, height);


            int[,] weight = new int[kernelW, kernelH];
            for (int i = 0; i < kernelW; i++)
            {
                for(int j = 0; j<kernelH; j++)

                weight[i,j] = Int32.Parse((Kernel.GetControlFromPosition(i, j) as TextBox).Text);
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double sumR = 0;
                    double sumG = 0;
                    double sumB = 0;
                    for (int i = 0; i < kernelW; i++)
                    {
                        
                        for (int j = 0; j < kernelH; j++)
                        {                            
                            Color pixel = new Color();
                            int newX = x + i - center.X, newY = y + j - center.Y;
                            if (newX < 0 || newX >= width || newY < 0 || newY >= height) pixel = Color.FromArgb(0, 0, 0);
                            else pixel = original[newX, newY];                           

                            sumR += pixel.R * weight[i,j];
                            sumG += pixel.G * weight[i,j];
                            sumB += pixel.B * weight[i,j];

                        }
                    }

                    int red = off + (int)((double)sumR / (double)div);
                    int green = off + (int)((double)sumG / (double)div);
                    int blue = off + (int)((double)sumB / (double)div);
                    if (red < 0) red = 0;
                    else if (red > 255) red = 255;
                    if (green < 0) green = 0;
                    else if (green > 255) green = 255;
                    if (blue < 0) blue = 0;
                    else if (blue > 255) blue = 255;
                    output.SetPixel(x, y, Color.FromArgb(red, green, blue));

                }
            }

            Output.Image = output;
        }

        private Point FindCenter()
        {
            Point cords = new Point();

            for(int x = 0; x<Kernel.ColumnCount; x++)
            {
                for(int y = 0; y<Kernel.RowCount; y++)
                {
                    if ((Kernel.GetControlFromPosition(x, y) as TextBox).BackColor == Color.YellowGreen)
                    {
                        cords = new Point(x, y);
                        goto A;
                    }
                }
            }

            A:
            return cords;
        }
    }
}
