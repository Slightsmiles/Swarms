using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.Entities
{
    public class Boardentity : GridLocation
    {
        public int _temp {get; protected set;}

        public Boardentity(float cost, bool traversable, Vector2 location) : base(cost, false, location, traversable){
            
        }
        protected int defaultTemp = 20;
    }
}