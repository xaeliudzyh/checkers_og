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

        List<Button> simpleSteps = new List<Button>();  //simple(1-cage) steps
        int countEatSteps;     // number of "eatable" steps
        Button prevButton;    //previous button
        Button pressedButton;
        bool isContinue = false;  //bool that shows us if we can continue step after eating 

        bool isMoving;

        int[,] map = new int[mapSize, mapSize];

        Button[,] buttons = new Button[mapSize, mapSize];

        Image whiteFig;
        Image blackFig;

        public Form1()
        {
            InitializeComponent();
            blackFig = new Bitmap(new Bitmap(@"C:\source\repos\сhekers123\b.png"), new Size(cellSize - 4, cellSize - 4));      //adding a png for white figure
            whiteFig = new Bitmap(new Bitmap(@"C:\source\repos\сhekers123\w.png"), new Size(cellSize - 1, cellSize - 1));  // the same for black one
            this.Text = "Checkers";

            Init();
        }

        public void Init()
        {
            currentPlayer = 1;
            isMoving = false;
            prevButton = null;

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
                        button.Image = whiteFig;   //adding whiteFig png on 1 in massive
                    else
                        if (map[i, j] == 2)
                            button.Image = blackFig;   //the same for blackFig png on 2 

                    button.BackColor = GetPrevButtonColor(button);


                    this.Controls.Add(button);
                }



            }
        }
        public void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;

        }

        public Color GetPrevButtonColor(Button prevButton)
        {
            if ((prevButton.Location.Y / cellSize % 2) != 0) {
                if ((prevButton.Location.X / cellSize % 2) == 0)
                    return Color.Gray;
            }
            if ((prevButton.Location.Y / cellSize) % 2 == 0) {
                if ((prevButton.Location.X / cellSize % 2) != 0)
                    return Color.Gray;
            }
            return Color.Linen ;
        }

        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPrevButtonColor(prevButton);

            Button pressedButton = sender as Button;
            if (map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] != 0 &&
                map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] == currentPlayer) 
            {
                pressedButton.BackColor = Color.Red;
                isMoving = true;

            }
            else
            {
                if (isMoving)
                {
                    int temp = map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize];
                    map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] = map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize];
                    map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize]=temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    isMoving = false;
                }
            }
                 
            prevButton = pressedButton;
        }

        public void ShowSteps(int iCurrFigure, int jCurrFigure, bool isOnestep = true)
        {
            simpleSteps.Clear();
            ShowDiagonal(iCurrFigure, jCurrFigure, isOnestep);
            if (countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        public bool DeterminePath(int ti, int tj)      //determing possible path
        {

            if (map[ti, tj] == 0 && isContinue==false)
            {
                buttons[ti, tj].BackColor = Color.Yellow;
                buttons[ti, tj].Enabled = true;
                simpleSteps.Add(buttons[ti, tj]);
            }
            else
            {

                if (map[ti, tj] != currentPlayer)
                {
                    if (pressedButton.Text == "D")     //checking if the figure is the queen of not, if yes, isOneStep = false
                        ShowNextStep(ti, tj, false);
                    else ShowNextStep(ti, tj);
                }

                return false;
            }
            return true;
        }

        public void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        public void ShowNextStep(int i,int j, bool isOneStep = true) 
        {
            int dirX = i - pressedButton.Location.X / cellSize;    //moving on X-axis
            int dirY = j - pressedButton.Location.Y / cellSize;    //moving on Y-axis
            dirX = dirX > 0 ? 1 : -1;
            dirY = dirY > 0 ? 1 : -1;
            int il = i;
            int jl = j;
            bool canWe = true;   //bool that shows us possibility of building the next step
            while (IsInsideBorders(il, jl))
            {
                if (map[i,j]!=0 && map[i,j]!= currentPlayer)
                {
                    canWe = false;
                    break;
                }
                if (canWe)
                    return;
                List <Button> turnOff = new List<Button>();    //list of buttons to turn off
                bool closeSimple = false;   //do we need to close "uneatable" steps?
                int ik = il + dirX;
                int jk = jl + dirY;
                while (IsInsideBorders(ik, jk))
                {
                    if (map[ik, jk] == 0)
                    {
                        if (IsFigureHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))   //checking if we has an opportunity to eat smth from this cage
                        {
                            closeSimple = true;
                        }
                        else
                            turnOff.Add(buttons[ik, jk]);
                        buttons[ik, jk].BackColor = Color.Yellow;
                        buttons[ik, jk].Enabled = true;
                        countEatSteps++;
                    }
                    else
                        break;
                    if (isOneStep)
                        break;
                    jk += dirY;
                    ik += dirX;
                }
                if (closeSimple && turnOff.Count > 0)
                    CloseSimpleSteps(turnOff);


            }
        }
 
        public bool IsFigureHasEatStep(int iCurrentFigure, int jCurrentFigure, bool isOneStep, int[] dir) //function that shows us if there is an enemy figure that can be eaten by us,
                                                                                                           //and bool isOneStep shows us if the figure is the queen or just a simple figure
        {
            bool eatStep = false;
            int j = jCurrentFigure + 1;
            for(int i = iCurrentFigure; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue)      // figure is not a qoeen, its first step and player 1
                    break;
                if (dir[0] == 1 && dir[1] == -1 && !isOneStep)   //figure stepped down and left
                    break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1))
                            eatStep = false;
                        else
                        {
                            if (map[i - 1, j + 1] != 0)
                                eatStep = false;
                            else
                                return eatStep;
                        }
                    }
                }
                if (j < 7)
                    j++;
                else
                    break;
                if (isOneStep)
                    break;
            }


            j = jCurrentFigure - 1;
            for (int i = iCurrentFigure; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue)      // figure is not a qoeen, its first step and player 1
                    break;
                if (dir[0] == 1 && dir[1] == 1 && !isOneStep)   //figure stepped down and right
                    break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            eatStep = false;
                        else
                        {
                            if (map[i - 1, j - 1] != 0)
                                eatStep = false;
                            else
                                return eatStep;
                        }
                    }
                }
                if (j > 0 )
                    j++;
                else
                    break;
                if (isOneStep)
                    break;
            }



            j=jCurrentFigure - 1;
            for (int i = iCurrentFigure; i >= 0; i--)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue)      // figure is not a qoeen, its first step and player 1
                    break;
                if (dir[0] == -1 && dir[1] == 1 && !isOneStep)   //figure stepped up and right
                    break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                            eatStep = false;
                        else
                        {
                            if (map[i + 1, j - 1] != 0)
                                eatStep = false;
                            else
                                return eatStep;
                        }
                    }
                }
                if (j > 0 )
                    j++;
                else
                    break;
                if (isOneStep)
                    break;
            }



            j = jCurrentFigure + 1;
            for (int i = iCurrentFigure; i >= 0; i--)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue)      // figure is not a qoeen, its first step and player 1
                    break;
                if (dir[0] == -1 && dir[1] == -1 && !isOneStep)   //figure stepped up and left 
                    break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            eatStep = false;
                        else
                        {
                            if (map[i - 1, j - 1] != 0)
                                eatStep = false;
                            else
                                return eatStep;
                        }
                    }
                }
                if (j > 0)
                    j++;
                else
                    break;
                if (isOneStep)
                    break;
            }

            return eatStep;

        }

        public void CloseSteps()
        {
            for (int i = 0; i < mapSize; i++)
                for (int j = 0; j < mapSize; j++)
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i,j]);
        }

        public bool IsInsideBorders(int ti, int tj)                            //checking if the choosen button inside the map
        {
            if (ti >= mapSize || tj >= mapSize || tj < 0 || ti < 0)
                return false;
            return true;
        }

        public void ActivateAllButtons()                                               //function that turns on all buttons
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        public void DeactivateAllButtons()                                         //function that turns off all buttons
        {
            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }

    }
}

