using System;
using Microsoft.Xna.Framework;

namespace Swarms.entities
{
    public class Agent : Boardentity
    {


        public Agent(){
            this.location = new Vector2(-1,-1);
            this.temp = defaultTemp;
            color = Color.Black;
        }

        public Agent(Vector2 location){
            this.location = location;
            this.temp = defaultTemp;
            color = Color.Black;
        }
        public void setTemp(int temp){
            this.temp = temp;
        }
        public void setLocation(int x, int y){
            this.location = new Vector2(x,y);
        }
    }
}