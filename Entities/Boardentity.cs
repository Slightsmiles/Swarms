using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.Entities
{
    public class Boardentity : GridLocation
    {
        public int _temp {get; set;}

        public Boardentity(float cost, bool traversable, Vector2 location) : base(cost, location, traversable){
            
        }

        public Boardentity(){}
        
        
        protected int defaultTemp = 20;
    }
}