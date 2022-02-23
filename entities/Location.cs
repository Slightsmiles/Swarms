namespace Swarms.entities
{
    public class Location
    {
        int x;
        int y;
        public Location(int p, int q){
            this.x = p;
            this.y = q;
        }
        public int getX(){
            return x;
        }
        public int getY(){
            return y;
        }
     
    }
}