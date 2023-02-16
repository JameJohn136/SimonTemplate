// Simon Says Game
// By James Johnson
// Due: Feb 16th 2023

#region Libraries
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Collections;
using System.Drawing.Drawing2D;
#endregion

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        #region Global Variables
        int guess;
        Random randGen = new Random();
        Dictionary<ColorType, int> colorMap = new Dictionary<ColorType, int>();
        enum ColorType { Green, Red, Yellow, Blue };
        SoundPlayer[] sounds = new SoundPlayer[5];

        GraphicsPath graphicPath = new GraphicsPath();
        Region region;
        // 😎
        #endregion


        public GameScreen()
        {
            InitializeComponent();
        }

        private async void GameScreen_Load(object sender, EventArgs e)
        {
            // Call an async void to prevent freezing the entire application
            OnLoad();
        }

        private void CutButtons()
        {
            // Cut the buttons to make it look more similar to the original game
            graphicPath.AddEllipse(-5, -5, 290, 290); //147, 135
            region = new Region(graphicPath);
            GraphicsPath exludePath = new GraphicsPath();
            exludePath.AddEllipse(55, 55, 125, 125);

            region.Exclude(exludePath);

            // Set the Regions
            greenButton.Region = region;
            RotateGraphics();
            redButton.Region = region;
            RotateGraphics();
            blueButton.Region = region;
            RotateGraphics();
            yellowButton.Region = region;

            // Remove the borders
            greenButton.FlatStyle = FlatStyle.Flat;
            redButton.FlatStyle = FlatStyle.Flat;
            blueButton.FlatStyle = FlatStyle.Flat;
            yellowButton.FlatStyle = FlatStyle.Flat;

        }

        private void RotateGraphics()
        {
            //rotate the orientation of the screen by 90 degrees
            Matrix transformMatrix = new Matrix();
            transformMatrix.RotateAt(90, new PointF(55, 55));
            region.Transform(transformMatrix);
        }


        private async void OnLoad()
        {
            ResetColors();
            EnableButton(false);
            CutButtons();

            // Set Dictionary
            colorMap.Add(ColorType.Green, 0);
            colorMap.Add(ColorType.Red, 1);
            colorMap.Add(ColorType.Yellow, 2);
            colorMap.Add(ColorType.Blue, 3);

            // Set Array
            sounds.SetValue(new SoundPlayer(Properties.Resources.green), 0);
            sounds.SetValue(new SoundPlayer(Properties.Resources.red), 1);
            sounds.SetValue(new SoundPlayer(Properties.Resources.yellow), 2);
            sounds.SetValue(new SoundPlayer(Properties.Resources.blue), 3);
            sounds.SetValue(new SoundPlayer(Properties.Resources.mistake), 4);

            // Clear Pattern list
            Form1.pattern.Clear();
            Refresh();
            await Task.Delay(1000);
            ComputerTurn();
        }

        private void ResetColors()
        {
            greenButton.BackColor = Color.Green;
            redButton.BackColor = Color.Red;
            blueButton.BackColor = Color.Blue;
            yellowButton.BackColor = Color.Yellow;
        }

        /*
         * This is used to prevent the player from clicking the buttons
         * whenever the pattern is being played
         */

        private void EnableButton(bool enabled)
        {
            if (enabled)
            {
                greenButton.Enabled = true;
                redButton.Enabled = true;
                blueButton.Enabled = true;
                yellowButton.Enabled = true;
            } else
            {
                greenButton.Enabled = false;
                redButton.Enabled = false;
                blueButton.Enabled = false;
                yellowButton.Enabled = false;
            }
        }

        private async void ComputerTurn()
        {
            ResetColors();
            EnableButton(false);
            await Task.Delay(250);
            //TODO: get rand num between 0 and 4 (0, 1, 2, 3) and add to pattern list. Each number represents a button. For example, 0 may be green, 1 may be blue, etc.
            int random = randGen.Next(0, 4);

            Form1.pattern.Add(random);

            //TODO: create a for loop that shows each value in the pattern by lighting up approriate button
            foreach (int num in Form1.pattern)
            {
                switch (num)
                { // Green, Red, Yellow, Blue
                    case 0: // Green
                        greenButton.BackColor = Color.White;
                        await Task.Delay(500);
                        greenButton.BackColor = Color.Green;
                       break;
                    case 1: // Red
                        redButton.BackColor = Color.White;
                        await Task.Delay(500);
                        redButton.BackColor = Color.Red;
                       break;
                    case 2: // Yellow
                        yellowButton.BackColor = Color.White;
                        await Task.Delay(500);
                        yellowButton.BackColor = Color.Yellow;
                       break;
                    case 3:
                        blueButton.BackColor = Color.White;
                        await Task.Delay(500);
                        blueButton.BackColor = Color.Blue;
                       break;
                }
                await Task.Delay(150);
            }

            guess = 0;
            EnableButton(true);
        }
        public void GameOver()
        {
            sounds[4].Play();


            Form1.ChangeScreen(this, new GameOverScreen());

        }

        private async void greenButton_Click(object sender, EventArgs e)
        {

            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Green, out colorNum);

            if (Form1.pattern[guess] == colorNum)
            {
                // Set Color
                greenButton.BackColor = Color.DarkGreen;
                // Play Sound
                sounds[colorNum].Play();
                Refresh();
                // Brief Pause
                await Task.Delay(200);
                ResetColors();
                guess++;
            }
            else // Game is over
            {
                GameOver();
            }

            // See if the pattern is done
            if (guess == Form1.pattern.Count())
            {
                ComputerTurn();
            }
        }


        private async void redButton_Click(object sender, EventArgs e)
        {
            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Red, out colorNum);

            if (Form1.pattern[guess] == colorNum)
            {
                // Set Color
                redButton.BackColor = Color.DarkRed;
                // Play Sound
                sounds[colorNum].Play();
                Refresh();
                // Brief Pause
                await Task.Delay(200);
                ResetColors();
                guess++;
            }
            else // Game is over
            {
                GameOver();
            }

            // See if the pattern is done
            if (guess == Form1.pattern.Count())
            {
                ComputerTurn();
            }
        }

        private async void yellowButton_Click(object sender, EventArgs e)
        {
            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Yellow, out colorNum);

            if (Form1.pattern[guess] == colorNum)
            {
                // Set Color
                yellowButton.BackColor = Color.Gold;
                // Play Sound
                sounds[colorNum].Play();
                Refresh();
                // Brief Pause
                await Task.Delay(200);
                ResetColors();
                guess++;
            }
            else // Game is over
            {
                GameOver();
            }

            // See if the pattern is done
            if (guess == Form1.pattern.Count())
            {
                ComputerTurn();
            }
        }

        private async void blueButton_Click(object sender, EventArgs e)
        {
            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Blue, out colorNum);

            if (Form1.pattern[guess] == colorNum)
            {
                // Set Color
                blueButton.BackColor = Color.DarkBlue;
                // Play Sound
                sounds[colorNum].Play();
                Refresh();
                // Brief Pause
                await Task.Delay(200);
                ResetColors();
                guess++;
            }
            else // Game is over
            {
                GameOver();
            }

            // See if the pattern is done
            if (guess == Form1.pattern.Count())
            {
                ComputerTurn();
            }
            
        }
    }
}
