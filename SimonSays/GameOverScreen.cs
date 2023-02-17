using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class GameOverScreen : UserControl
    {
        public GameOverScreen()
        {
            InitializeComponent();
        }

        private void GameOverScreen_Load(object sender, EventArgs e)
        {
            // Get the length of the pattern (player score)
            int length = Form1.pattern.Count() - 1;

            // Set Player Score Labels Text
            lengthLabel.Text = $"{length}";

            // Check to see if theres a new highscore
            if (length > Form1.highscore) { Form1.highscore = length; }

            // Set the Label Text
            highScoreLabel.Text = $"Highscore: {Form1.highscore}";

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            // Return to Main Menu
            Form1.ChangeScreen(this, new MenuScreen());
        }
    }
}
