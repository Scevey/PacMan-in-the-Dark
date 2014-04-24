using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace PacmanInTheDark
{
    class Map
    {
        #region Fields
        /// <summary>
        /// list of paths
        /// </summary>
        List<Path> paths;

        /// <summary>
        /// property for the path list
        /// </summary>
        public List<Path> Paths
        {
            get
            {
                return paths;
            }
        }

        /// <summary>
        /// The map's background. Can be loaded from a file if one exists, or generated on the fly.
        /// </summary>
        Texture2D mapBG;

        //list of pellets
        //TODO

        #endregion

        #region Properties

        /// <summary>
        /// Gets a point representing the dimensions of the map
        /// </summary>
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

                return new Point(maxX+1, maxY+1);
            }
        }

        #endregion

        /// <summary>
        /// takes a map file name, does stuff
        /// </summary>
        /// <param name="filename">string representing the path to the map file</param>
        public Map(string filename)//, Texture2D _mapBG)
        {
            paths = new List<Path>();
            Parse(filename);
            CalculateIntersects(paths);
            //mapBG = _mapBG;
            //CheckPellets(paths);
        }

        public Map(string filename, GraphicsDevice gd)
        {
            paths = new List<Path>();
            Parse(filename);
            CalculateIntersects(paths);
            //CheckPellets(paths);
        }

        /// <summary>
        /// parses map file and populates path and pellet lists. All I/O occurs here
        /// </summary>
        /// <param name="filename">string representing the path to the map file</param>
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

        /// <summary>
        /// populates the intersection dictionaries of all the paths in the path list
        /// </summary>
        /// <param name="pathList">The list of paths contained in the map</param>
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

        public static Texture2D DrawMap(Map m, GraphicsDevice gd)
        {
            const int lineWidth = 1;
            const int padding = 1;
            const int scaleFactor = padding * 2 + lineWidth;
            Color lineColor = Color.Blue;

            Texture2D tempBG = new Texture2D(gd, (int)(m.MapSize.X) * scaleFactor, (int)(m.MapSize.Y) * scaleFactor);
            uint[] textureData = new uint[tempBG.Width * tempBG.Height];

            foreach (Path p in m.paths)
            {
                Point projStart = new Point(p.Start.X * scaleFactor + padding, p.Start.Y * scaleFactor + padding);
                Point projEnd = new Point(p.End.X * scaleFactor + padding, p.End.Y * scaleFactor + padding);

                for (int x = (int)projStart.X; x <= (int)projEnd.X; x++)
                {
                    for (int y = (int)projStart.Y; y <= (int)projEnd.Y; y++)
                    {
                        textureData[x+y*tempBG.Width] = (uint)Color.Blue.ToArgb();
                    }
                }
            }

            tempBG.SetData<uint>(textureData);

            return tempBG;
        }

        /// <summary>
        /// removes from the pellet list any pellets not on a path
        /// </summary>
        /// <param name="pathList">The list of paths contained in the map</param>
        void CheckPellets(List<Path> pathList) //TODO add pellet list parameter
        {
            //TODO
        }
    }
}
