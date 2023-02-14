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

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        int guess;
        Random randGen = new Random();
        Dictionary<ColorType, int> colorMap = new Dictionary<ColorType, int>();
        SoundPlayer greenSound = new SoundPlayer(Properties.Resources.green);
        SoundPlayer blueSound = new SoundPlayer(Properties.Resources.blue);
        SoundPlayer redSound = new SoundPlayer(Properties.Resources.red);
        SoundPlayer yellowSound = new SoundPlayer(Properties.Resources.yellow);
        // 😎

        enum ColorType { Green, Red, Yellow, Blue };

        public GameScreen()
        {
            InitializeComponent();
        }

        private async void GameScreen_Load(object sender, EventArgs e)
        {
            // Call an async void to prevent freezing the entire application
            OnLoad();
        }


        private async void OnLoad()
        {
            ResetColors();
            EnableButton(false);

            // Set Dictionary
            colorMap.Add(ColorType.Green, 0);
            colorMap.Add(ColorType.Red, 1);
            colorMap.Add(ColorType.Yellow, 2);
            colorMap.Add(ColorType.Blue, 3);

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
            //TODO: Play a game over sound


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
                greenButton.BackColor = Color.White;
                // Play Sound
                greenSound.Play();
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
                redButton.BackColor = Color.White;
                // Play Sound
                redSound.Play();
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
                yellowButton.BackColor = Color.White;
                // Play Sound
                yellowSound.Play();
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
                blueButton.BackColor = Color.White;
                // Play Sound
                blueSound.Play();
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
