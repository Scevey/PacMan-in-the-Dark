using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PacmanInTheDark
{
    //Anthony Giallella
    class GameSetting
    {
        //load in file from editor
        public void Load()
        {
            // to store the data
            List<int> data = new List<int>();

            //parse text file to ints with this
            int val;

            StreamReader input = null;
            try
            {
                //open stream
                input = new StreamReader("../../../../../ToolForPacman.ToolForPacman.bin.Debug.Editor.txt");
                string text = "";
                //take text, parse, add to list
                while ((text = input.ReadLine()) != null)
                {
                    val = int.Parse(text);
                    data.Add(val);
                }
            }
            //if no file add default values
            catch (FileNotFoundException)
            {
                data.Add(3);
                data.Add(5);
                data.Add(3);
                data.Add(100);
                data.Add(1);
                data.Add(3);
            }

        }
    }
}
