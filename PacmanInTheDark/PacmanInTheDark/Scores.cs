using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PacmanInTheDark
{
    //Jeremy Hall
    //Anthony Giallella
    // Deals with keeping track of hiscores
    class Scores
    {
        // Holds all of the hi scores
        List<ScoresStruct> hiScores = new List<ScoresStruct>();
        // property to retrieve all of the scores
        public List<ScoresStruct> HiScores
        {
            get { return hiScores; }
            set { hiScores = value; }
        }

        // Structures that contain the name of the person and their score
        public struct ScoresStruct
        {
            public string Name;
            public int Score;
        }

        // Creates a new score struct object
        public Scores()
        {
            // Load the scores
            this.LoadScores();
            // We only want the top 10 scores. Delete any others
            if (hiScores.Count > 9)
            {
                for (int i = 10; i <= hiScores.Count - 1; i++)
                {
                    hiScores.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Displays all of the scores on the hiscores page
        /// </summary>
        public void SortScores()
        {
            for (int i = 0; i < hiScores.Count; i++)
            {
                int j = i;
                while (j > 0 && (hiScores[j - 1].Score < hiScores[j].Score))
                {
                    //int temp = hiScores[j];
                    ScoresStruct tempSS = hiScores[j];
                    hiScores[j] = hiScores[j - 1];
                    hiScores[j - 1] = tempSS;
                    j = j - 1;
                }
            }

        }

        /// <summary>
        /// Loads in all of the scores from the text file
        /// </summary>
        public void LoadScores()
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

                // Sort newly loaded scores
                SortScores();
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

        /// <summary>
        /// Writes the hiscore(s) to the text file for future use
        /// </summary>
        public void WriteScores(string name, int score)
        {
            try
            {
                // Open writer
                StreamWriter writescores = new StreamWriter("scores.txt");

                // Write values
                writescores.WriteLine(name + ',' + score);

                // Close writer
                writescores.Close();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
