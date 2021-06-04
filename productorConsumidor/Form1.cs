using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace productorConsumidor
{

    public partial class Form1 : Form
    {
        Random turn = new Random();
        int indexofProducer= 0;
        int indexofProducerchecked = 0;
        int indexofConsumer = 0;
        int contbool = 0;
        int previusIndexImage;
        string imageName = "Imagen";
        string indexedImage;
        List<int> lugaresProducto = new List<int>();
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
        int nLeftRect,     // x-coordinate of upper-left corner
        int nTopRect,      // y-coordinate of upper-left corner
        int nRightRect,    // x-coordinate of lower-right corner
        int nBottomRect,   // y-coordinate of lower-right corner
        int nWidthEllipse, // height of ellipse
        int nHeightEllipse // width of ellipse

        );
        private bool mouseDown;
        private Point lastLocation;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            panel1.BorderStyle = BorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));


        }

        #region UIButtons
        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizedButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region MouseControl
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        #endregion

        #region MainFunctions

        private void SetTurns()
        {

            int turnNumber = turn.Next(1,11 );
            int veces = turn.Next(3, 6);

            //Turno Prodictor
            if (turnNumber % 2 == 0)
            {
                if(lugaresProducto.Count == 20)
                {
                    statusIndicator.Text = "No hay espacio para producir";
                    statusIndicator.ForeColor = Color.Red;
                }
                if (lugaresProducto.Count <= 20)
                {
                    
                    for (int i=0; i<veces; i++)
                    {
                        vecesValue.Text = veces.ToString();
                        producir();
                    }

                }
            }

            //Turno Consumidor
            if (turnNumber % 2 != 0 && lugaresProducto.Count > 0)
            {
                for(int i =0; i<veces; i++)
                {
                    if(lugaresProducto.Count > 0) { 
                        
                    vecesValue.Text = veces.ToString();
                    consumiendo();
                    }
                }

            }

            if (turnNumber % 2 != 0 && lugaresProducto.Count <= 0)
            {
                statusIndicator.Text = "Sin producto para consumir";
                statusIndicator.ForeColor = Color.Red;
            }


        }

        private void producir()
        {
            
            contbool =  checkifisFull();
            Console.WriteLine("contbool; " + contbool);
            if (contbool < 20)
            {


                statusIndicator.Text = "Produciendo";
                statusIndicator.ForeColor = Color.DarkGreen;
                productorImage.Image = Properties.Resources.Protogema;
                consumerImage.Image = Properties.Resources.disabledPaimon;
                indexofProducer++;

                indexofProducerchecked = indexofProducer;
                gemvalue.Text = indexofProducer.ToString();
                indexedImage = imageName + indexofProducer.ToString();
                if (!lugaresProducto.Contains(indexofProducer))
                {
                    lugaresProducto.Add(indexofProducer);
                    PictureBox mybox = (PictureBox)this.Controls.Find(indexedImage, true)[0];
                    mybox.Image = Properties.Resources.Protogema;
                }
            }

            if (indexofProducer == 20)
            {
                indexofProducer = 0;
            }
        }

        private void consumiendo()
        {
            statusIndicator.Text = "Consumiendo";
            statusIndicator.ForeColor = Color.DarkOrange;
            productorImage.Image = Properties.Resources.DisabledGem;
            consumerImage.Image = Properties.Resources.paimon;
            indexofConsumer++;
            indexConsumer.Text = indexofConsumer.ToString();


            if (indexofConsumer > 1)
            {
                previusIndexImage = indexofConsumer - 1;
                PictureBox previusmybox = (PictureBox)this.Controls.Find(imageName + previusIndexImage.ToString(), true)[0];
                previusmybox.Image = Properties.Resources.g909;

                if(lugaresProducto.Count == 20)
                {
                    previusIndexImage = indexofConsumer - 1;
                    PictureBox previusmybox1 = (PictureBox)this.Controls.Find(imageName + previusIndexImage.ToString(), true)[0];
                    previusmybox1.Image = Properties.Resources.Protogema;
                }

            }

            if (indexofConsumer == 1)
            {
                PictureBox previusmybox = (PictureBox)this.Controls.Find(imageName + "20".ToString(), true)[0];
                previusmybox.Image = Properties.Resources.g909;
            }
            indexedImage = imageName + indexofConsumer.ToString();
            PictureBox mybox = (PictureBox)this.Controls.Find(indexedImage, true)[0];

            if (lugaresProducto.Contains(indexofConsumer))
            {
                lugaresProducto.Remove(indexofConsumer);
            }

            mybox.Image = Properties.Resources.paimon;
            if (indexofConsumer == 20)
            {

                indexofConsumer = 0;

            }
        }
        #endregion

        int checkifisFull()
        {
            int checker = 0;
            for (int i = 1; i <= 20; i++)
            {
                if (lugaresProducto.Contains(i))
                {
                    checker++;
                }
            }

            return checker;
        }


        private void pictureBox15_Click(object sender, EventArgs e)
        {
            if (timerRB.Checked == true)
            {
                Tempo.Start();
            }
            
            else if(clickRB.Checked == true)
            {
                
                SetTurns();
                
            }          

            else
            {
                MessageBox.Show("Seleccione un tipo de control");
            }
        }



        private void Tempo_Tick(object sender, EventArgs e)
        {
            
            SetTurns();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Escape)
            {
                MessageBox.Show("Gracias por usar el programa");
                Tempo.Stop();
            }
        }
    }
}
