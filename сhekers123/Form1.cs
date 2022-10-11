using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace сhekers123
{
    public partial class Form1 : Form
    {
        const int mapSize = 8;   // length of the desk
        const int cellSize = 100;   //size of the cage

        int currentPlayer;

        Button prevButton;

        bool isMoving;

        int[,] map = new int[mapSize, mapSize];

        Image whiteFig;
        Image blackFig;

        public Form1()
        {
            InitializeComponent();
            blackFig = new Bitmap(new Bitmap(@"C:\source\repos\сhekers123\b.png"), new Size(cellSize - 4, cellSize - 4));      //adding a png for white figure
            whiteFig = new Bitmap(new Bitmap(@"C:\source\repos\сhekers123\w.png"), new Size(cellSize - 1, cellSize - 1));  // the same for black one
            this.Text = "Checkers";

            InIt();
        }

        public void InIt()
        {
            currentPlayer = 1;
            isMoving = false;
            prevButton = null;

            map = new int[mapSize, mapSize] {
                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 3,0,3,0,3,0,3,0 },
                { 0,3,0,3,0,3,0,3 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };

            CreateMap();
        }
        public void CreateMap()
        {
            this.Width = mapSize * cellSize;
            this.Height = mapSize * cellSize;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {

                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.Click += new EventHandler(OnFigurePress);
                    if (map[i, j] == 1)
                    {
                        button.BackgroundImage = whiteFig;   //adding whiteFig png on 1 in massive
                        button.BackColor = Color.Gray;

                    }
                    else
                    {
                        if (map[i, j] == 2)
                        {
                            button.BackgroundImage = blackFig;   //the same for blackFig png on 2 
                            button.BackColor = Color.Gray;
                        }
                        else {
                            if (map[i, j] == 3)
                                button.BackColor = Color.Gray;
                            else
                                button.BackColor = Color.Linen;
                        }
                    }

                    this.Controls.Add(button);
                }



            }
        }
        public void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;

        }

        public Color GetPrevButtonColor()
        {
            if ((prevButton.Location.Y / cellSize % 2) != 0) {
                if ((prevButton.Location.X / cellSize % 2) == 0)
                    return Color.Gray;
            }
            if ((prevButton.Location.Y / cellSize) % 2 == 0) {
                if ((prevButton.Location.X / cellSize % 2) != 0)
                    return Color.Gray;
            }
            return Color.White;
        }

        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPrevButtonColor();

            Button pressedButton = sender as Button;
            if (map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] != 0 &&
                map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] == currentPlayer) 
            {
                pressedButton.BackColor = Color.Red; 

            }
            prevButton = pressedButton;
        }

    }
}

