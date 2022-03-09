using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.entities
{
    public class Tree : Boardentity
    {
        bool isBurning;
        public Tree(){
            this.location = getLocationFromVector();
            this.temp = getTemp();
            if (isBurning){
                this.color = Color.Green;
            }
            else color = Color.Red;
        }
        public Tree(Location location, int temp, Color color){
            this.location = location;
            this.temp = temp;
            this.color = color;
        }
        public Tree(Location location){
            this.location = location;
        }
    }
}