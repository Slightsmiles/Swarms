namespace Swarms.Entities
{
    public class Path
    {
        protected Location startPoint;
        protected Location endPoint;

        public Location getStartPoint(){
            return startPoint;
        }
        public Location getEndPoint(){
            return endPoint;
        }
    }
}