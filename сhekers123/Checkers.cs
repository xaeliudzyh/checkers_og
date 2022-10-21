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


    public partial class Checkers : Form
    {
        

        const int mapSize = 8;   // length of the desk
        const int cellSize = 80;   //size of the cage

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
        Image whiteQ;
        Image blackQ;

        public Checkers()
        {
            InitializeComponent();
            blackFig = new Bitmap(new Bitmap(@"C:\source\repos\davaii\b.png"), new Size(cellSize - 4, cellSize - 4));      //adding a png for black figure
            whiteFig = new Bitmap(new Bitmap(@"C:\source\repos\davaii\w.png"), new Size(cellSize - 1, cellSize - 1));  // the same for white one

            blackQ = new Bitmap(new Bitmap(@"C:\source\repos\davaii\bq.png"), new Size(cellSize - 4, cellSize - 4));      //adding a png for black queen
            whiteQ = new Bitmap(new Bitmap(@"C:\source\repos\davaii\wq.png"), new Size(cellSize - 1, cellSize - 1));  // the same for white one

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


        /// <summary>
        /// restarting the game
        /// </summary>
        public void ResetGame()
        {
            bool player1 = false;
            bool player2 = false;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == 1)
                        player1 = true;
                    if (map[i, j] == 2)
                        player2 = true;
                }
            }
            if (!player1 || !player2)
            {
                this.Controls.Clear();
                Init();
            }
        }


        /// <summary>
        /// creating the map 8*8
        /// </summary>
        public void CreateMap()
        {
            this.Width = (mapSize + 1) * cellSize;
            this.Height = (mapSize + 1) * cellSize;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.Click += new EventHandler(PressingOnFigure);
                    if (map[i, j] == 1)
                        button.Image = whiteFig;
                    else if (map[i, j] == 2) button.Image = blackFig;

                    button.BackColor = GetPreviousButtonColor(button);
                    button.ForeColor = Color.Red;

                    buttons[i, j] = button;

                    this.Controls.Add(button);
                }
            }
        }

        /// <summary>
        /// switching the player after the step
        /// </summary>
        public void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 1 ? 2 : 1;
            ResetGame();
        }


        /// <summary>
        /// rturning the color of the sage when figure left it
        /// </summary>
        /// <param name="prevButton"></param>
        /// <returns></returns>
        public Color GetPreviousButtonColor(Button prevButton)
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

        /// <summary>
        /// giving a pressed figure red color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PressingOnFigure(object sender, EventArgs e)
        {
            if (prevButton != null)
                prevButton.BackColor = GetPreviousButtonColor(prevButton);

            pressedButton = sender as Button;

            if (map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] != 0 && map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] == currentPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red;   //coloring chosen figure
                DeactivateAllButtons();
                pressedButton.Enabled = true;      //enabling 
                countEatSteps = 0;
                if (pressedButton.Image == blackQ || pressedButton.Image == whiteQ)
                    ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize);

                if (isMoving)
                {
                    CloseSteps();
                    pressedButton.BackColor = GetPreviousButtonColor(pressedButton);
                    ShowPossibleSteps();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }
            else
            {
                if (isMoving)
                {
                    isContinue = false;
                    if (Math.Abs(pressedButton.Location.X / cellSize - prevButton.Location.X / cellSize) > 1)
                    //if length of step>1, it means that figure ate someone
                    {
                        isContinue = true;
                        DeleteEaten(pressedButton, prevButton);
                    }
                    int temp = map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize];
                    //switching images
                    map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] = map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize];
                    map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize] = temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";
                    SwitchButtonToQueen(pressedButton);
                    countEatSteps = 0;
                    isMoving = false;
                    CloseSteps();
                    DeactivateAllButtons();
                    if (pressedButton.Image == blackQ || pressedButton.Image == whiteQ)
                        ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                    else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize);
                    if (countEatSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        SwitchPlayer();
                        ShowPossibleSteps();
                        isContinue = false;
                    }
                    else if (isContinue)
                    {
                        pressedButton.BackColor = Color.Red;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }
                }
            }

            prevButton = pressedButton;
        }

        /// <summary>
        /// showing figures that have an "eatable" step
        /// </summary>
        public void ShowPossibleSteps()
        {
            bool isOneStep = true;
            bool isEatStep = false;
            DeactivateAllButtons();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == currentPlayer)
                    {
                        if (buttons[i, j].Image == blackQ || buttons[i, j].Image == whiteQ)//checking if the fihure os queen
                            isOneStep = false;
                        else isOneStep = true;
                        if (PossibilityOfStep(i, j, isOneStep, new int[2] { 0, 0 }))
                        {
                            isEatStep = true;
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if (!isEatStep)
                ActivateAllButtons();
        }

        /// <summary>
        /// giving the figure rights of Queen
        /// </summary>
        /// <param name="button"></param>
        public void SwitchButtonToQueen(Button button)
        {
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 1 && button.Location.Y / cellSize == mapSize - 1)
            {
                
                button.Image = whiteQ;

            }
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 2 && button.Location.Y / cellSize == 0)
            {
                button.Image = blackQ;
            }
        }

        /// <summary>
        /// deleting buttons between endButton and startButton that was ate
        /// </summary>
        /// <param name="endButton"> starting cage</param>
        /// <param name="startButton"> cage to jump on</param>
        public void DeleteEaten(Button endButton, Button startButton)     
        { 
            int count = Math.Abs(endButton.Location.Y / cellSize - startButton.Location.Y / cellSize);
            int startIndexX = endButton.Location.Y / cellSize - startButton.Location.Y / cellSize;
            int startIndexY = endButton.Location.X / cellSize - startButton.Location.X / cellSize;
            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;
            int currCount = 0;
            int i = startButton.Location.Y / cellSize + startIndexX;
            int j = startButton.Location.X / cellSize + startIndexY;
            
            while (currCount < count - 1)   //running on [i,j] diapazone and deleting figures that needs to be deleted
            {
                map[i, j] = 0;
                buttons[i, j].Image = null;   //deleting image of the figurt
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                currCount++;
            }

        }

        /// <summary>
        ///calling for this function when we tap on the button 
        /// </summary>
        /// <param name="dirXofCurrentFigure"></param>
        /// <param name="dirYofCurrentFigure"></param>
        /// <param name="isOnestep"> is the figure a simple figure or not</param>
        public void ShowSteps(int dirXofCurrentFigure, int dirYofCurrentFigure, bool isOnestep = true)   
        {
            simpleSteps.Clear();
            ShowDiagonalSteps(dirXofCurrentFigure, dirYofCurrentFigure, isOnestep);
            if (countEatSteps > 0)      //means that if we have eatable steps we turning off usuall steps
                CloseSimpleSteps(simpleSteps);
        }


        /// <summary>
        /// showing the diagonal for the steps
        /// </summary>
        /// <param name="dirXofCurrentFigure"></param>
        /// <param name="dirYofCurrentFigure"></param>
        /// <param name="isOneStep"></param>

        public void ShowDiagonalSteps(int dirXofCurrentFigure, int dirYofCurrentFigure, bool isOneStep = false)    
        {

            //checking for 1st player
            int j = dirYofCurrentFigure + 1;  
            for (int i = dirXofCurrentFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))     //if the button is inside the borders
                {
                    if (!DeterminePath(i, j))  
                        break;
                }
                if (j < 7)   //j<7 means that if we will step forward, we still will be in the borders of the map 
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = dirYofCurrentFigure - 1;   
            for (int i = dirXofCurrentFigure - 1; i >= 0; i--)
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



            //cheking for the 2nd player
            j = dirYofCurrentFigure - 1;
            for (int i = dirXofCurrentFigure + 1; i < 8; i++)
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

            j = dirYofCurrentFigure + 1;
            for (int i = dirXofCurrentFigure + 1; i < 8; i++)
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



        /// <summary>
        /// determing if there is a possible path
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="tj"></param>
        /// <returns></returns>
        public bool DeterminePath(int ti, int tj)      
        {

            if (map[ti, tj] == 0 && isContinue==false)      //is continuing shows us if we an continue the step to kill more than 1 figure 
            {
                buttons[ti, tj].BackColor = Color.Yellow;      //lighing on this cage 
                buttons[ti, tj].Enabled = true;    
                simpleSteps.Add(buttons[ti, tj]);    
            }
            else
            {

                if (map[ti, tj] != currentPlayer)
                {
                    if (pressedButton.Image == blackQ || pressedButton.Image == whiteQ)     //checking if the figure is the queen of not, if yes, isOneStep = false
                        ShowProceduralEat(ti, tj, false);
                    else ShowProceduralEat(ti, tj);
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// turning off the buttons that have 1 step
        /// </summary>
        /// <param name="simpleSteps"></param>
        public void CloseSimpleSteps(List<Button> simpleSteps)    
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPreviousButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }    

        /// <summary>
        /// showing the procedure of eating
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="isOneStep"></param>
        public void ShowProceduralEat(int i,int j, bool isOneStep = true)    
        {
            int dirX = i - pressedButton.Location.Y / cellSize;    //moving on X-axis
            int dirY = j - pressedButton.Location.X / cellSize;    //moving on Y-axis
            dirX = dirX > 0 ? 1 : -1;
            dirY = dirY > 0 ? 1 : -1;
            int il = i;
            int jl = j;
            bool canWe = true;   //bool that shows us possibility of building the next step
            while (IsInsideBorders(il, jl))
            {
                if (map[i, j] != 0 && map[i, j] != currentPlayer)
                {
                    canWe = false;
                    break;
                }
                if (isOneStep)
                    return;
                il += dirX;
                jl += dirY;
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
                        if (PossibilityOfStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))   //checking if we has an opportunity to eat smth from this cage
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

        /// <summary>
        /// function that shows us if there is an enemy figure that can be eaten by us,
        ///and bool isOneStep shows us if the figure is the queen or just a simple figure
        /// </summary>
        /// <param name="dirXofCurrentFigure"></param>
        /// <param name="dirYofCurrentFigure"></param>
        /// <param name="isOneStep"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool PossibilityOfStep(int dirXofCurrentFigure, int dirYofCurrentFigure, bool isOneStep, int[] dir) 
        {
            bool eatStep = false;
            int j = dirYofCurrentFigure + 1;
            for(int i = dirXofCurrentFigure; i >= 0; i--)
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


            j = dirYofCurrentFigure - 1;
            for (int i = dirXofCurrentFigure; i >= 0; i--)
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



            j=dirYofCurrentFigure - 1;
            for (int i = dirXofCurrentFigure; i >= 0; i--)
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



            j = dirYofCurrentFigure + 1;
            for (int i = dirXofCurrentFigure; i >= 0; i--)
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

        /// <summary>
        /// closing the button that changed by eating og figure 
        /// </summary>
        public void CloseSteps()
        {
            for (int i = 0; i < mapSize; i++)
                for (int j = 0; j < mapSize; j++)
                    buttons[i, j].BackColor = GetPreviousButtonColor(buttons[i,j]);
        }

        /// <summary>
        /// cheking if the [ti,tj] button is inside tha borders of the map
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="tj"></param>
        /// <returns></returns>
        public bool IsInsideBorders(int ti, int tj)  
        {
            if (ti >= mapSize || tj >= mapSize || tj < 0 || ti < 0)
                return false;
            return true;
        }

        /// <summary>
        /// turning on all buttons
        /// </summary>
        public void ActivateAllButtons()  
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        /// <summary>
        /// turning off all buttons
        /// </summary>
        public void DeactivateAllButtons()    
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

