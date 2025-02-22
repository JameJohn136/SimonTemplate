﻿// Simon Says Game
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
#endregion

namespace SimonSays
{
    public partial class GameScreen : UserControl
    {
        #region Global Variables
        int guess;
        int skips = 3;
        bool isPlaying;
        Random randGen = new Random();
        Dictionary<ColorType, int> colorMap = new Dictionary<ColorType, int>();
        SoundPlayer[] sounds = new SoundPlayer[5];

        GraphicsPath graphicPath = new GraphicsPath();
        Button[] buttons = new Button[4];
        Region region;
        enum ColorType { Green, Red, Yellow, Blue };
        // 😎 Wanted to add an emoji into a comment because it felt funny for some reason
        #endregion


        public GameScreen()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            // Call an async void to prevent freezing the entire application
            OnLoad();
        }

        private void CutButtons()
        {
            // Cut the buttons to make it look more similar to the original game
            graphicPath.AddEllipse(-5, -5, 290, 290);
            region = new Region(graphicPath);

            // Create the Elipse that will be removed to create the center circle
            GraphicsPath exludePath = new GraphicsPath();
            exludePath.AddEllipse(55, 55, 125, 125);

            region.Exclude(exludePath);

            // Remove the borders around the buttons and set the region
            foreach (Button button in buttons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.Region = region;

                // Rotate the graphics before applying it to the next button
                RotateGraphics();
            }

        }

        private void RotateGraphics()
        {
            //rotate the orientation of the screen by 90 degrees
            Matrix transformMatrix = new Matrix();
            transformMatrix.RotateAt(90, new PointF(55, 55));
            region.Transform(transformMatrix);
            // Then go back above and paste the new region
        }


        private async void OnLoad()
        {
            // Define everything like a dictionary and array here so it only happens once
            // as theres no point defining them multiple times

            // Set the colors to what we set them to later, this is to stop inconsistanties
            // from the Designer set colors and the colors we set it to
            ResetColors();

            // Prevent player from interacting with buttons upon the start
            EnableButton(false);


            // Set Dictionary
            colorMap.Add(ColorType.Green, 0);
            colorMap.Add(ColorType.Red, 1);
            colorMap.Add(ColorType.Blue, 2);
            colorMap.Add(ColorType.Yellow, 3);

            // Set Button Array
            buttons.SetValue(greenButton, 0);
            buttons.SetValue(redButton, 1);
            buttons.SetValue(blueButton, 2);
            buttons.SetValue(yellowButton, 3);

            // Set Array
            sounds.SetValue(new SoundPlayer(Properties.Resources.green), 0);
            sounds.SetValue(new SoundPlayer(Properties.Resources.red), 1);
            sounds.SetValue(new SoundPlayer(Properties.Resources.blue), 2);
            sounds.SetValue(new SoundPlayer(Properties.Resources.yellow), 3);
            sounds.SetValue(new SoundPlayer(Properties.Resources.mistake), 4);

            // Use Code to change the button shape
            CutButtons();

            // Clear Pattern list
            Form1.pattern.Clear();
            Refresh();
            await Task.Delay(1000);

            // Get the Computer to take their turn
            ComputerTurn();
        }

        private void ResetColors()
        {
            // Sets all the buttons back to their original colors
            greenButton.BackColor = Color.Green;
            redButton.BackColor = Color.Red;
            blueButton.BackColor = Color.Blue;
            yellowButton.BackColor = Color.Yellow;
        }


        private void EnableButton(bool enabled)
        { // Prevent the player from clicking the buttons when a computer turn is playing
            if (enabled)
            {
                greenButton.Enabled = true;
                redButton.Enabled = true;
                blueButton.Enabled = true;
                yellowButton.Enabled = true;
            }
            else
            {
                greenButton.Enabled = false;
                redButton.Enabled = false;
                blueButton.Enabled = false;
                yellowButton.Enabled = false;
            }
        }

        private async void ComputerTurn()
        {
            // Is not currently playing
            isPlaying = false;

            // Ensure all colors are accurate
            ResetColors();

            // Prevent button interaction
            EnableButton(false);

            // Set Skip Label Text
            skipLabel.Text = $"Skips Remaining: {skips}";

            // General Delay to help pacing
            await Task.Delay(250);
  
            // Random Number from 0 - # of buttons (Each being a button)
            int random = randGen.Next(0, buttons.Length + 1);

            // Save what button it is for next loop
            Form1.pattern.Add(random);

            // Play the pattern, and light up each button
            foreach (int num in Form1.pattern)
            {
                switch (num)
                { // Green/TopLeft, Red/TopRight, Yellow/BottomLeft, Blue/BottomRight
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
                    case 2:

                        blueButton.BackColor = Color.White;
                        await Task.Delay(500);
                        blueButton.BackColor = Color.Blue;
                        break;
                    case 3: // Yellow

                        yellowButton.BackColor = Color.White;
                        await Task.Delay(500);
                        yellowButton.BackColor = Color.Yellow;
                        break;
                }

                // Slight Delay after each light up to more easily show when the same button
                // lights up multiple times
                await Task.Delay(500);
            }

            // Reset the players current guess
            guess = 0;

            // Allow button interaction
            EnableButton(true);

            // Player can now play
            isPlaying = true;
        }
        public async void GameOver()
        {
            // Sound 4 is the game-over/mistake sound
            sounds[4].Play();

            // Disable button interaction
            EnableButton(false);

            // Make each button light up red
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].BackColor = Color.Red;
            }

            // Delay to let sound play
            await Task.Delay(2000);

            // Change Screens
            Form1.ChangeScreen(this, new GameOverScreen());
        }

        #region Button Click Events
        private void greenButton_Click(object sender, EventArgs e)
        {

            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Green, out colorNum);

            ButtonEvent(colorNum);
        }


        private void redButton_Click(object sender, EventArgs e)
        {
            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Red, out colorNum);

            ButtonEvent(colorNum);
        }

        private void yellowButton_Click(object sender, EventArgs e)
        {
            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Yellow, out colorNum);

            ButtonEvent(colorNum);
        }

        private void blueButton_Click(object sender, EventArgs e)
        {
            // Check if the pattern index is equal to green
            int colorNum;
            colorMap.TryGetValue(ColorType.Blue, out colorNum);
            
            ButtonEvent(colorNum);
        }

        private async void ButtonEvent(int colorNum)
        {   
            // See if the player got a game over
            if (Form1.pattern[guess] != colorNum) { GameOver(); return; }

            // Set Button Color
            buttons[colorNum].BackColor = Color.White;

            // Play Sound
            sounds[colorNum].Play();

            // Quick Refresh
            Refresh();

            // Brief Pause
            await Task.Delay(200);

            // Make sure all colors are correct
            ResetColors();

            // Add one to guess counter
            guess++;

            // See if the pattern is done
            if (guess == Form1.pattern.Count())
            {
                ComputerTurn();
            }
 
        }
        #endregion

        private void skipButton_Click(object sender, EventArgs e)
        {
            // See if the player can currently skip
            if (!isPlaying) { return; }

            // Remove a skip
            skips--;

            // Check to see if theres remaining skips
            // if not, disable the button
            if (skips == 0)
            {
                skipButton.Enabled = false;

                // Give Feedback that the button is disabled
                skipButton.BackColor = Color.DarkGray;
            }

            ComputerTurn();
        }
    }
}
