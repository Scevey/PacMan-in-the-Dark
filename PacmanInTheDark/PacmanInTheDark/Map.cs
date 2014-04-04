using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PacmanInTheDark
{
    class Map
    {
        #region Fields
        //list of paths
        List<Path> paths;

        //property for the path list
        public List<Path> Paths
        {
            get
            {
                return paths;
            }
        }

        //list of pellets
        //TODO

        #endregion

        #region Properties

        public Point MapSize
        {
            get
            {
                float maxX = 0;
                float maxY = 0;
                foreach (Path p in Paths)
                {
                    if (p.End.X > maxX)
                        maxX = p.End.X;
                    if (p.End.Y > maxY)
                        maxY = p.End.Y;
                }

                return new Point(maxX, maxY);
            }
        }

        #endregion

        //takes a map file name, does stuff
        public Map(string filename)
        {
            paths = new List<Path>();
            Parse(filename);
            CalculateIntersects(paths);
            //CheckPellets(paths);
        }

        //parses map file and populates path and pellet lists. All I/O occurs here
        void Parse(string filename)
        {
            //this section may be revised after milestone 2
            //presently it will assume every line in the map file denotes a path
            //the format for each line in the map file is as follows: #,# #,#
            //the first ordered pair is the start point, the second is the end
            //when writing map files, make sure the start point is closer to the origin than the end point
            //assuming origin is the top left, this means that start is the left-most point for horizontal paths, and top-most for vertical

            //create the stream and stream reader
            Stream str = null;
            StreamReader input = null;

            try
            {
                //instantiate the stream and stream reader
                str = File.OpenRead(filename);
                input = new StreamReader(str);

                //blank string for input
                string line = "";

                while ((line = input.ReadLine()) != null)
                {
                    //splits the string by the space
                    string[] pointSplit = line.Split(' ');
                    Point[] points = new Point[pointSplit.Length];

                    //iterates through the string-form points and converts them to actual points
                    for (int i = 0; i < pointSplit.Length; i++)
                    {
                        string[] stringPoint = pointSplit[i].Split(',');
                        points[i] = new Point(int.Parse(stringPoint[0]), int.Parse(stringPoint[1]));
                    }

                    //creates a path from the points and adds it to the path list
                    paths.Add(new Path(points[0], points[1]));
                }
            }
            catch (IOException ioe)
            {

            }
            finally
            {
                //close the file if it was opened
                if (str != null)
                    str.Close();
                if (input != null)
                    input.Close();
            }
        }

        //populates the intersection dictionaries of all the paths in the path list
        void CalculateIntersects(List<Path> pathList)
        {
            //checks every path in the path list for intersections with every other path
            foreach (Path p in pathList)
            {
                foreach (Path p2 in pathList)
                {
                    //does nothing if the two paths being checked are the same path
                    if (p == p2)
                        continue;
                    p.AddIntersect(p2);
                }
            }
        }

        //removes from the pellet list any pellets not on a path
        void CheckPellets(List<Path> pathList) //TODO add pellet list parameter
        {
            //TODO
        }
    }
}
