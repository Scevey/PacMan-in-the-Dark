using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ToolForPacman
{
    public partial class ToolEditorForm : Form
    {
        //Made by Jeremy

        public ToolEditorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Provides information about each attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Speed: How fast Pacman travels (1-10).\n" +
                            "Ghosts: How many ghosts are there (1-5).\n" +
                            "Lives: How many lives you start with (1-5).\n" +
                            "Health: How much health you start with (25-200).\n" +
                            "Hunger: How fast your health drains(1-5).\n" +
                            "Light: The level of light around you(1-3).\n");
        }

        //Was the save successful?
        bool saveSuccessful = false;

        //Name of the saved file
        string saveFile = "Editor.txt";

        //attributes
        int speed;
        int ghosts;
        int lives;
        int health;
        int hunger;
        int light;

        //Stream to add info to the save file
        StreamWriter input;

        /// <summary>
        /// User saves data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Create the stream
                input = new StreamWriter(saveFile);

                //Set the attributes
                speed = int.Parse(txtSpeed.Text);
                ghosts = int.Parse(txtGhosts.Text);
                lives = int.Parse(txtLives.Text);
                health = int.Parse(txtHealth.Text);
                hunger = int.Parse(txtHunger.Text);
                light = int.Parse(txtLight.Text);

                //check for incorrect values
                if (speed < 1 || speed > 11 || ghosts < 1 || ghosts > 5
                    || lives < 1 || lives > 5 || health < 25 || health > 200
                    || hunger < 1 || hunger > 5 || light < 1 || light > 3)
                    saveSuccessful = false;
                else
                {
                    //no incorrect values, OK to proceed
                    saveSuccessful = true;

                    //Populate the text file with user's entered attributes
                    input.WriteLine(speed);
                    input.WriteLine(ghosts);
                    input.WriteLine(lives);
                    input.WriteLine(health);
                    input.WriteLine(hunger);
                    input.WriteLine(light);
                }
                //Close the stream
                input.Close();
            }
            catch (IOException ioe)
            {
                saveSuccessful = false;
                MessageBox.Show("Error: " + ioe.Message);
            }
            catch (Exception)
            {
                saveSuccessful = false;
                MessageBox.Show("Error saving file. Check for errors (Ie. invalid characters).");
            }

            //Alert user how to fix a possible save malfunction cause
            if (saveSuccessful == false)
                MessageBox.Show("Save was unsuccessful. See help box to view allowed values.");
        }

        /// <summary>
        /// User wants to close editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Pacman editor will now close.");
            this.Close();
        }
    }
}
