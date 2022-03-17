using System.Runtime.CompilerServices;
using System.Globalization;
using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.entities
{
    public class Tree : Boardentity
    {

        bool isBurning;
        public Tree(){
            this.location = new Vector2(-1,-1);
            this.temp = getTemp();
            this.color = GetColor();
        }
        public Tree(Vector2 location){
            this.location = location;
            this.temp = defaultTemp;
            this.color = GetColor();
        }

        
        public new Color GetColor(){
            if (this.temp <= 80) {
                isBurning = false;
                return Color.LawnGreen;
            }
            else{
              isBurning = true;  
            } return Color.Red;
        }
    }
}