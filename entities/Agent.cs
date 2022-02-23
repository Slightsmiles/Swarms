using System;
namespace Swarms.entities
{
    public class Agent : Boardentity
    {

        
        public Agent(){
            this.location = getLocation();
            this.temp = getTemp();
        }
        private void setTemp(int temp){
            this.temp = temp;
        }
        private void setLocation(int x, int y){
            this.location = new Location(x,y);
        }
    }
}