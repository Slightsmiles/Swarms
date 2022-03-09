using System;
using Microsoft.Xna.Framework;

namespace Swarms.entities
{
    public class Agent : Boardentity
    {

        
        public Agent(){
            this.location = getLocationFromVector();
            this.temp = getTemp();
            color = Color.Black;
        }
        private void setTemp(int temp){
            this.temp = temp;
        }
        private void setLocation(int x, int y){
            this.location = new Location(x,y);
        }
    }
}