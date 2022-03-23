using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.Entities
{
    public class Boardentity : GridLocation
    {
        public Vector2 _location {get; protected set;} // Array indexes
        public int _temp {get; protected set;}

        public Boardentity() : base(1, true){
            
        }
        protected int defaultTemp = 20;
    }
}