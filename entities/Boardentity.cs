using System.Drawing;

namespace Swarms.entities
{
    public class Boardentity
    {
        protected Location location;
        protected int temp;

        protected Color color;
        public Location getLocation(){
            return location;
        }
        public int getTemp(){
            return temp;
        }
        public Color GetColor(){
            return color;
        }
    }
}