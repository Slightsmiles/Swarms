using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;
namespace Swarms.entities
{
    public class Boardentity : GridLocation
    {
        protected Location location;
        protected int temp;


        public Location getLocationFromVector(){
            return new Location((int)base.getLocation().X, (int)base.getLocation().Y);
        }
        public int getTemp(){
            return temp;
        }
        public Color GetColor(){
            return color;
        }
    }
}