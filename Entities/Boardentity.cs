using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.Entities
{
    public class Boardentity : GridLocation
    {
        public Vector2 _location {get; protected set;} // Array indexes
        public int _temp {get; protected set;}

        public Boardentity(float cost, bool traversable, Vector2 location) : base(cost, traversable, location){
            
        }
        protected int defaultTemp = 20;
    }
}