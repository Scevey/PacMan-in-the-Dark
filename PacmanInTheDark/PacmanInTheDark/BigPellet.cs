using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PacmanInTheDark
{
    class BigPellet:Pellet
    {
        /// <summary>
        /// override for GamePiece's pacman collision
        /// </summary>
        /// <param name="pac"></param>
        public override void Collision(Pacman pac)
        {
            ////run the big pellet method in a new thread
            if (Active)
            {
                Thread t = new Thread(new ThreadStart(Game1.Main.BigPellet));
                t.Priority = ThreadPriority.BelowNormal;
                t.Start();
            }
            //do what normal pellets do
            base.Collision(pac);            
        }

        public BigPellet(Path path, float pos)
            : base(path, pos)
        {
        }
    }
}
