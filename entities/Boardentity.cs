using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.entities
{
    public class Boardentity : GridLocation
    {
        protected Vector2 location;
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