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
    public partial class HelpScreen : Form
    {
        public HelpScreen()
        {
            InitializeComponent();
        }

        private void backToMenu(object sender, EventArgs e)
        {
            StartScreen back = new StartScreen();
            back.Show();
            this.Hide();
        }
    }
}
