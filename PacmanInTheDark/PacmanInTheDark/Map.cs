using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PacmanInTheDark
{
    class Map
    {
        //list of paths
        List<Path> paths;
        public List<Path> Paths
        {
            get
            {
                return paths;
            }
        }

        //list of pellets
        //TODO

        //takes a map file name, does stuff
        public Map(string filename)
        {
            Parse(filename);
            CalculateIntersects(paths);
            CheckPellets(paths);
        }

        //parses map file and populates path and pellet lists. All I/O occurs here
        void Parse(string filename)
        {
            //TODO
        }

        //populates the intersection dictionaries of all the paths in the path list
        void CalculateIntersects(List<Path> pathList)
        {
            //TODO
        }

        //removes from the pellet list any pellets not on a path
        void CheckPellets(List<Path> pathList) //TODO add pellet list parameter
        {
            //TODO
        }
    }
}
