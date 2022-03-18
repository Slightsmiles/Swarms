using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.Entities
{
    public class Boardentity : GridLocation
    {
        public Vector2 location {get; protected set;} // Array indexes
        protected int temp;

        protected int defaultTemp = 20;
        public int getTemp(){
            return temp;
        }
        public Color GetColor(){
            return color;
        }
    }
}