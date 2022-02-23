using System.Drawing;
namespace Swarms.entities
{
    public class Tree : Boardentity
    {
        
        public Tree(){
            this.location = getLocation();
            this.temp = getTemp();
            this.color = GetColor();
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