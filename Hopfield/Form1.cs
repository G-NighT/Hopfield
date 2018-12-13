using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra;

namespace Hopfield
{
    public partial class Form1 : Form
    {
        private Graphics formGraphics;
        private bool drawing = false;
        private DrawField drawField = new DrawField();
        private string[] bitmapFilePaths = new string[5];

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadSamplePictures()
        {
            bitmapFilePaths = new[] { "m.bmp", "a.bmp", "k.bmp", "c.bmp", "i.bmp" };
            picSample1.Image = BitmapParser.Scale(new Bitmap(bitmapFilePaths[0]), times: DrawField.CellSize);
            picSample2.Image = BitmapParser.Scale(new Bitmap(bitmapFilePaths[1]), times: DrawField.CellSize);
            picSample3.Image = BitmapParser.Scale(new Bitmap(bitmapFilePaths[2]), times: DrawField.CellSize);
            picSample4.Image = BitmapParser.Scale(new Bitmap(bitmapFilePaths[3]), times: DrawField.CellSize);
            picSample5.Image = BitmapParser.Scale(new Bitmap(bitmapFilePaths[4]), times: DrawField.CellSize);
        }

        private void NoiseAndLoad(int pictureIndex)
        {
            int noise = trackBar1.Value;
            Bitmap noised = BitmapParser.Noise(new Bitmap(bitmapFilePaths[pictureIndex]), noise).ToBitmap();
            Bitmap scaledNoised = BitmapParser.Scale(noised, times: DrawField.CellSize);
            picNoised.Image = scaledNoised;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadSamplePictures();
            NoiseAndLoad(pictureIndex: 0);
            btnRecognizeDrawn.Enabled = true;
            btnRecognizeNoised.Enabled = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                if (CursorIsInsideDrawField(e.X, e.Y))
                {
                    int xOffset = e.X - DrawField.Left;
                    int yOffset = e.Y - DrawField.Top;
                    int rowIndex = yOffset / DrawField.CellSize;
                    int columnIndex = xOffset / DrawField.CellSize;
                    if (rowIndex >= DrawField.CellsCount) rowIndex = DrawField.CellsCount - 1;
                    if (columnIndex >= DrawField.CellsCount) columnIndex = DrawField.CellsCount - 1;
                    drawField[rowIndex, columnIndex] = +1;
                    formGraphics.FillRectangle(Brushes.Black, DrawField.Left + DrawField.CellSize * columnIndex,
                        DrawField.Top + DrawField.CellSize * rowIndex, DrawField.CellSize, DrawField.CellSize);
                }
            }
        }

        private bool CursorIsInsideDrawField(int x, int y)
        {
            return x >= DrawField.Left && x <= DrawField.Left + DrawField.Size &&
                   y >= DrawField.Top && y <= DrawField.Top + DrawField.Size;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, DrawField.Left, DrawField.Top, 
                DrawField.Size, DrawField.Size);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formGraphics = CreateGraphics();
            formGraphics.DrawRectangle(Pens.Violet, 0, 0, DrawField.Size, DrawField.Size);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            drawField.Clear();
            formGraphics.FillRectangle(Brushes.White, DrawField.Left, DrawField.Top,
                DrawField.Size, DrawField.Size);
        }

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            LoadImage(picSample1);
        }

        private void btnSelect2_Click(object sender, EventArgs e)
        {
            LoadImage(picSample2);
        }

        private void btnSelect3_Click(object sender, EventArgs e)
        {
            LoadImage(picSample3);
        }

        private void btnSelect4_Click(object sender, EventArgs e)
        {
            LoadImage(picSample4);
        }

        private void btnSelect5_Click(object sender, EventArgs e)
        {
            LoadImage(picSample5);
        }

        private void LoadImage(PictureBox pictureBox)
        {
            using (var ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string filePath = ofd.FileName;
                    int index = int.Parse(pictureBox.Name.Substring(pictureBox.Name.Length - 1)) - 1;
                    bitmapFilePaths[index] = filePath;
                    pictureBox.Image = BitmapParser.Scale(new Bitmap(filePath), times: DrawField.CellSize);
                }
            }
        }

        private int GetCheckedRadioButtonIndex()
        {
            var checkedRadioButton = groupBox1.Controls.OfType<RadioButton>().First(r => r.Checked);
            return int.Parse(checkedRadioButton.Text) - 1;
        }

        private void radio1_CheckedChanged(object sender, EventArgs e)
        {
            NoiseAndLoad(GetCheckedRadioButtonIndex());
        }

        private void radio2_CheckedChanged(object sender, EventArgs e)
        {
            NoiseAndLoad(GetCheckedRadioButtonIndex());
        }

        private void radio3_CheckedChanged(object sender, EventArgs e)
        {
            NoiseAndLoad(GetCheckedRadioButtonIndex());
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            NoiseAndLoad(GetCheckedRadioButtonIndex());
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            NoiseAndLoad(GetCheckedRadioButtonIndex());
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            NoiseAndLoad(pictureIndex: GetCheckedRadioButtonIndex());
        }

        private void RecognizeAndLoadPicture(Matrix recognizable)
        {
            // Берем 5 образцов - векторы длины,
            // кодированные биполярно, т.е. +1 и -1,
            // и распознаем "испорченный" образец - вектор той же длины.

            // Получаем матрицу,
            // умножаем ее на "испорченный" образец,
            // обрабатываем результат умножения (вектор) функцией знака SIGN,
            // и получаем подправленный образец.
            // Выполняем пока не получится.

            //Debug.WriteLine(recognizable.ToPrettyString());
            Vector[] sampleVectors = bitmapFilePaths
                .Select(path => new Bitmap(path))
                .Select(bitmap => bitmap.ToMatrix().ToVectorByColumns())
                .ToArray();
            Matrix weights = Recognizer.GenerateWeightsMatrix(sampleVectors);
            Vector inputVector = recognizable.ToVectorByColumns();
            //Matrix recognized = Recognizer.RecognizeSynchronously(weights, inputVector, sampleVectors);
            Matrix recognized = Recognizer.RecognizeAsynchronously(weights, inputVector);
            Bitmap recognizedScaled = BitmapParser.Scale(recognized, times: DrawField.CellSize);
            picRecognized.Image = recognizedScaled;
        }

        private void btnRecognizeDrawn_Click(object sender, EventArgs e)
        {
            RecognizeAndLoadPicture(drawField.GetMatrix());
        }

        private string GetSelectedBitmapFilePath()
        {
            return bitmapFilePaths[GetCheckedRadioButtonIndex()];
        }

        private void btnRecognizeNoised_Click(object sender, EventArgs e)
        {
            Matrix noised = BitmapParser.Noise(new Bitmap(GetSelectedBitmapFilePath()), ammount: trackBar1.Value);
            //Debug.WriteLine("На вход:");
            //Debug.WriteLine(noised.ToPrettyString());
            RecognizeAndLoadPicture(noised);
        }
    }
}
