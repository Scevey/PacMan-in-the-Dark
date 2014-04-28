using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PacmanInTheDark
{
    // Deals with keeping track of hiscores
    class Scores
    {
        // Holds all of the hi scores
        List<ScoresStruct> hiScores = new List<ScoresStruct>();

        // Structures that contain the name of the person and their score
        struct ScoresStruct
        {
            public string Name;
            public int Score;
        }

        // Creates a new score  struct object
        public Scores()
        {
        }

        /// <summary>
        /// Displays all of the scores on the hiscores page
        /// </summary>
        void DisplayScores()
        {
            //TODO: sort hiscores and figure out which are highest
        }

        /// <summary>
        /// Loads in all of the scores from the text file
        /// </summary>
        void LoadScores()
        {
            try
            {
                // Reader to read in scores
                StreamReader readScores = new StreamReader("scores.txt");

                // Clear previously existing scores
                hiScores.Clear();

                // Loops until no more scores
                string input = "";
                while ((input = readScores.ReadLine()) != null)
                {
                    // create temp attributes to hold values
                    ScoresStruct tempScores;
                    string[] tempArray = input.Split(',');

                    // Get the name and score and add to the struct
                    tempScores.Name = tempArray[0];
                    tempScores.Score = int.Parse(tempArray[1]);

                    // add the struct to the list
                    hiScores.Add(tempScores);
                }
            }
            catch (Exception) 
            {
                // clear previously existing hiscores
                hiScores.Clear();

                // Add the error message to the hiscores
                ScoresStruct error;
                error.Name = "Error with loading scores";
                error.Score = 0;
                hiScores.Add(error);
            }
        }
    }
}
