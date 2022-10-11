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
        const int cellSize = 55;   //size of the cage

        int[,] map = new int[mapSize, mapSize];

        Image whiteFig;
        Image blackFig;

        public Form1()
        {
            InitializeComponent();
            whiteFig = new Bitmap("w.png");     //adding a png for white figure
            blackFig = new Bitmap("b.png");    // the same for black one
            this.Text = "Checkers";

            InIt();
        }

        public void InIt()
        {
            map = new int[mapSize, mapSize] {
                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };

            CreateMap();
        }
        public void CreateMap()
        {
            this.Width= mapSize*cellSize;
            this.Height= mapSize*cellSize;

            for (int i = 0; i < mapSize; i++) 
            {
                for(int j=0; j < mapSize; j++)
                {
                    
                    Button button = new Button();
                    button.Location = new Point(j*cellSize,i*cellSize);
                    button.Size= new Size(cellSize,cellSize);
                    if (map[i, j] == 1)
                        button.BackgroundImage = whiteFig;   //adding whiteFig png on 1 in massive
                    else
                        if (map[i,j]==2)
                            button.BackgroundImage = blackFig;   //the same for blackFig png on 2 
                    this.Controls.Add(button);
                }
            }
        
        }
    }
}
