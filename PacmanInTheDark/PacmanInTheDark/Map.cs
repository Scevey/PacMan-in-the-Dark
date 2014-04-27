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
        List<Pellet> pellets;

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
            pellets = new List<Pellet>();
            Parse(filename);
            CalculateIntersects(paths);
            CheckPellets(pellets);
        }

        //public Map(string filename, GraphicsDevice gd)
        //{
        //    paths = new List<Path>();
        //    pellets = new List<Pellet>();
        //    Parse(filename);
        //    CalculateIntersects(paths);
        //    //CheckPellets(paths);
        //}

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
                    //it's a path if the line contains a space
                    if (line.Contains(' '))
                    {
                        //splits the string by the space
                        string[] pointSplit = line.Trim().Split(' ');
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
                        //it's a pellet if it doesn't
                    else
                    {
                        //split the string into components and make a point
                        string[] coordSplit = line.Split(',');
                        Point pelletPoint = new Point(int.Parse(coordSplit[0]), int.Parse(coordSplit[1]));

                        //iterate through the path list
                        foreach (Path p in paths)
                        {
                            //if the point is on a path
                            if(Path.PointOnPath(pelletPoint, p))
                                //create a new pellet on that path at the proper position
                                pellets.Add(new Pellet(p, Point.Distance(p.Start, pelletPoint)));
                        }
                    }
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

        //removes duplicate pellets
        //duplicates will naturally occurr at path intersections
        void CheckPellets(List<Pellet> pelletList)
        {
            //for every pellet in the list...
            foreach (Pellet p in pelletList)
            {
                //check every other pellet in the list
                foreach (Pellet p2 in pelletList)
                {
                    //do nothing if the two pellets being examined are in fact the same pellet
                    if (p == p2)
                        continue;
                    //otherwise, remove the second if they are on top of each other
                    if (p.MapPos == p2.MapPos)
                        pelletList.Remove(p2);
                }
            }
        }

        //generates a Texture2D for a map file
        public static Texture2D DrawMap(Map m, GraphicsDevice gd)
        {
            //constants dealing with drawing style
            const int lineWidth = 1; //the width in pixels of the path lines
            const int padding = 1; //the number of pixels from the edge of the map tile that the line starts
            const int scaleFactor = padding * 2 + lineWidth; //a scale factor based on the above parameters
            Color lineColor = Color.Red; //the line color

            //create the texture and an array of color values
            Texture2D tempBG = new Texture2D(gd, (int)(m.MapSize.X) * scaleFactor, (int)(m.MapSize.Y) * scaleFactor);
            uint[] textureData = new uint[tempBG.Width * tempBG.Height];

            //interate through the path list
            foreach (Path p in m.paths)
            {
                //create Points for the start and end of the line in the texture's grid-space
                Point projStart = new Point(p.Start.X * scaleFactor + padding, p.Start.Y * scaleFactor + padding);
                Point projEnd = new Point(p.End.X * scaleFactor + padding, p.End.Y * scaleFactor + padding);

                //iterate through the color array and do stuff
                for (int x = (int)projStart.X; x <= (int)projEnd.X; x++)
                {
                    for (int y = (int)projStart.Y; y <= (int)projEnd.Y; y++)
                    {
                        textureData[x+y*tempBG.Width] = (uint)Color.Blue.ToArgb();
                    }
                }
            }

            //transfer the color array to the texture
            tempBG.SetData<uint>(textureData);

            return tempBG;
        }
    }
}
