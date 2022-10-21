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
    public partial class StartScreen : Form
    {
        public StartScreen()
        {
            InitializeComponent();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            Checkers gameWindow = new Checkers();
            gameWindow.Show();
            this.Hide();
        }

        private void HelpScreen(object sender, EventArgs e)
        {
            HelpScreen helpScr = new HelpScreen();
            helpScr.Show();
            this.Hide();
        }
    }
}
