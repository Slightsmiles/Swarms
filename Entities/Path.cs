using Microsoft.Xna.Framework;
namespace Swarms.Entities
{
    public class Path
    {
        protected Vector2 startPoint;
        protected Vector2 endPoint;

        public Vector2 getStartPoint(){
            return startPoint;
        }
        public Vector2 getEndPoint(){
            return endPoint;
        }
    }
}