﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;

namespace PacmanInTheDark
{
    // Mike Teixeira
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
        /// a dictionarywhose keys are integer IDs and whose values are pairs of points to be connected with warps
        /// </summary>
        Dictionary<int, List<Point>> warpPointDictionary = new Dictionary<int, List<Point>>();

        /// <summary>
        /// The map's background. Can be loaded from a file if one exists, or generated on the fly.
        /// </summary>
        Texture2D mapBG;

        //list of pellets
        List<Pellet> pellets;
        public List<Pellet> Pellets
        {
            get
            {
                return pellets;
            }
        }

        /// <summary>
        /// Scale reference. This will be the map's width
        /// </summary>
        const int xRef = 50;

        /// <summary>
        /// Scale reference. This will be the map's height
        /// </summary>
        const int yRef = 50;

        float xScale;
        float yScale;

        #endregion

        #region Properties

        Point mapSize;

        /// <summary>
        /// Gets a point representing the dimensions of the map
        /// </summary>
        public Point MapSize
        {
            get
            {
                return mapSize;
            }
        }

        /// <summary>
        /// Gets a float representing the ratio between the map's width and its height
        /// </summary>
        public float MapRatio
        {
            get
            {
                Point size = MapSize;
                return size.X / size.Y;
            }
        }

        /// <summary>
        /// Gets the total amount of pellets active on the map
        /// </summary>
        public int PelletCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < pellets.Count; i++)
                {
                    if (pellets[i].Active == true) count++;
                }
                return count;
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
            CalculateIntersects();
            CheckPellets();
            CreateWarps();
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

                //list for all the lines of the map file
                List<string> lines = new List<string>();

                //populate the list
                while ((line = input.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                //temp variables for storing max coords
                int maxX = 0;
                int maxY = 0;

                //iterate through the line list
                foreach(string s in lines)
                {
                    try
                    {
                        //only do stuff to paths
                        if (s.Contains(' '))
                        {
                            //split the start and end
                            string[] pointSplit = s.Split(' ');

                            //this thing
                            List<string[]> pointSplitSplit = new List<string[]>();

                            //split the start and end into coordinate arrays, add each array to the thing
                            pointSplitSplit.Add(pointSplit[0].Split(','));
                            pointSplitSplit.Add(pointSplit[1].Split(','));

                            //temp variables for max dimensions
                            int x; int y;

                            //iterate through the thing
                            foreach (string[] coordSplit in pointSplitSplit)
                            {
                                //for each one, check the x and the y
                                //if it won't parse, remove characters until it does
                                //out to the temp variables
                                while (!int.TryParse(coordSplit[0], out x))
                                {
                                    if (coordSplit[0].Length == 0)
                                    {
                                        x = 0;
                                        break;
                                    }
                                    coordSplit[0] = coordSplit[0].Remove(coordSplit[0].Length - 1);
                                }
                                while (!int.TryParse(coordSplit[1], out y))
                                {
                                    if (coordSplit[1].Length == 0)
                                    {
                                        y = 0;
                                        break;
                                    }
                                    coordSplit[1] = coordSplit[1].Remove(coordSplit[1].Length - 1);
                                }

                                //if the temp variables are larger than the max dimensions, set max dimensions to temp variables
                                if (x > maxX)
                                    maxX = x;
                                if (y > maxY)
                                    maxY = y;
                            }                            
                        }
                    }
                    catch (FormatException fe)
                    {
                        //ignore the line if it doesn't parse right
                        continue;
                    }
                }

                //create scale factors based on max dimensions
                //the dimensions of every point created will be multiplied by these scale factors
                xScale = xRef / (float)maxX;
                yScale = yRef / (float)maxY;

                for (int i = 0; i < lines.Count;i++)
                {
                    try
                    {
                        //it's a path if the s contains a space
                        if (lines[i].Contains(' '))
                        {
                            #region Path parsing
                            //splits the string by the space
                            string[] pointSplit = lines[i].Trim().Split(' ');

                            #region warp parsing
                            //checks each point on the pair for a warp identifier
                            for (int n = 0; n < pointSplit.Length; n++)
                            {
                                //conditional for the presence of the identifier
                                if (pointSplit[n].Contains('w'))
                                {
                                    //checks the character after the w, this is the warp's ID
                                    int warpID = int.Parse(pointSplit[n].Substring(pointSplit[n].IndexOf('w') + 1));

                                    //remove the warp identifier so the string is in the proper format for the rest of the parse
                                    pointSplit[n] = pointSplit[n].Remove(pointSplit[n].IndexOf('w'));

                                    //split the coordinate pair into separate coordinates
                                    string[] stringPoint = pointSplit[n].Split(',');

                                    //create an entry in the warp point dictionary for the ID if one doesn't exist
                                    if (!warpPointDictionary.ContainsKey(warpID))
                                        warpPointDictionary.Add(warpID, new List<Point>());

                                    //add the point to the entry
                                    warpPointDictionary[warpID].Add(new Point(float.Parse(stringPoint[0])*xScale, float.Parse(stringPoint[1])*yScale));
                                }
                            }
                            #endregion

                            Point[] points = new Point[pointSplit.Length];

                            //iterates through the string-form points and converts them to actual points
                            for (int n = 0; n < pointSplit.Length; n++)
                            {
                                string[] stringPoint = pointSplit[n].Split(',');
                                points[n] = new Point(float.Parse(stringPoint[0])*xScale, float.Parse(stringPoint[1])*yScale);
                            }

                            //creates a path from the points and adds it to the path list
                            paths.Add(new Path(points[0], points[1]));
                            #endregion
                        }

                        //it's a pellet if it doesn't
                        else
                        {
                            #region pellet parsing
                            //check for a big pellet identifier
                            bool big;
                            if (lines[i][lines[i].Length - 1] == 'b')
                            {
                                big = true;
                                lines[i] = lines[i].Remove(lines[i].IndexOf('b'));
                            }
                            else
                                big = false;

                            //split the string into components and make a point
                            string[] coordSplit = lines[i].Split(',');
                            Point pelletPoint = new Point(float.Parse(coordSplit[0])*xScale, float.Parse(coordSplit[1])*yScale);

                            //iterate through the path list
                            foreach (Path p in paths)
                            {
                                //if the point is on a path
                                if (Path.PointOnPath(pelletPoint, p))
                                    //create a new pellet (or big pellet) on that path at the proper position
                                    pellets.Add((!big) ? new Pellet(p, Point.Distance(p.Start, pelletPoint)) : new BigPellet(p, Point.Distance(p.Start, pelletPoint)));

                            }
                            #endregion
                        }
                    }
                    catch (FormatException fe)
                    {
                        //ignore the line if it doesn't parse right
                        continue;
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

            SetMapSize();
        }

        void SetMapSize()
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

            mapSize = new Point(maxX + 1, maxY + 1);
        }

        /// <summary>
        /// populates the intersection dictionaries of all the paths in the path list
        /// </summary>
        /// <param name="paths">The list of paths contained in the map</param>
        void CalculateIntersects()
        {
            //checks every path in the path list for intersections with every other path
            foreach (Path p in paths)
            {
                foreach (Path p2 in paths)
                {
                    //does nothing if the two paths being checked are the same path
                    if (p == p2)
                        continue;
                    p.AddIntersect(p2);
                }
            }
        }

        /// <summary>
        /// removes duplicate pellets. duplicates will naturally occurr at path intersections. check pellets on intersection points too
        /// </summary>
        void CheckPellets()
        {
            //for every pellet in the list...
            for (int p1index = 0; p1index < pellets.Count; p1index++)
            {
                //check every other pellet in the list
                for (int p2index = 0; p2index < pellets.Count; p2index++)
                {
                    //do nothing if the two pellets being examined are in fact the same pellet
                    if (pellets[p1index] == pellets[p2index])
                        continue;
                    //otherwise, remove the second if they are on top of each other
                    if (pellets[p1index].MapPos == pellets[p2index].MapPos)
                        pellets.RemoveAt(p2index);
                }
            }

            //for every path
            foreach (Path p in paths)
            {
                //check every pellet
                foreach (Pellet pel in pellets)
                {
                    //for every intersection point on the path
                    foreach (Point ip in p.IntersectionDictionary.Values)
                    {
                        //compare the position of the point and the pellet point
                        if (pel.MapPos == ip && !p.pieces.Contains(pel))
                        {
                            //if they're the same and the pellet isn't in the path's piece list, add it to the list
                            p.pieces.Add(pel);
                        }
                    }
                }
            }
        }

        void CreateWarps()
        {
            //a listlist of a special format of points
            List<List<PathPosition>> pathPosList = new List<List<PathPosition>>();

            //iterates through all the point lists in the warp dictionary
            foreach (List<Point> pl in warpPointDictionary.Values)
            {
                //new temp list for the listlist
                List<PathPosition> pposList = new List<PathPosition>();

                //iterates through all the points in each point list
                //needs to be a for because we'll be removing points
                for (int i = 0; i < pl.Count; i++)
                {
                    bool isOnPath = false;
                    //iterates through all the paths in the path list
                    foreach (Path path in paths)
                    {
                        //if the point is not on the path...
                        if (Path.PointOnPath(pl[i], path))
                        {
                            isOnPath = true;
                            //create a new PathPosition and add it to the temp list
                            pposList.Add(new PathPosition(path, Point.Distance(pl[i], path.Start)));                            
                        }                        
                    }
                    if (!isOnPath)
                    {
                        //remove it from the point list
                        pl.Remove(pl[i]);

                        //decrement the counter for the point list
                        //you'll skip an element if this doesn't happen
                        i--;
                    }
                }
                //add the temp list to the listlist
                pathPosList.Add(pposList);
            }

            //iterates through the listlist
            foreach (List<PathPosition> pposList in pathPosList)
            {
                //skips the sublist list if it has more or less than two PathPositions
                if (pposList.Count != 2) continue;

                //creates the warp
                Warp newWarp = new Warp(pposList[0]._Path, pposList[0].Position, pposList[1]._Path, pposList[1].Position);
            }
        }

        /// <summary>
        /// generates a Texture2D for a map file
        /// </summary>
        /// <param name="m">the map to draw</param>
        /// <param name="gd">the active graphics device</param>
        /// <returns>returns a texture representation of the map</returns>
        public static Texture2D DrawMap(Map m, GraphicsDevice gd)
        {
            //because we have a working map, this value will be used to scale all other maps to its draw-scale
            //float drawscale = 29 / m.MapSize.X;

            //constants dealing with drawing style
            const int lineWidth = 10; //the width in pixels of the path lines
            const int padding = 5; //the number of pixels from the edge of the map tile that the line starts
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
                for (int x = (int)projStart.X; x < (int)projEnd.X + lineWidth; x++)
                {
                    for (int y = (int)projStart.Y; y < (int)projEnd.Y + lineWidth; y++)
                    {
                        textureData[x + y * tempBG.Width] = (uint)lineColor.ToArgb();
                    }
                }
            }

            //transfer the color array to the texture
            tempBG.SetData<uint>(textureData);

            return tempBG;
        }
    }
}
