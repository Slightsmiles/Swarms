using Microsoft.Xna.Framework;

namespace Swarms.Entities
{
    public class Direction
    {

        public int Up(Vector2 curr)
        {
            return (int) curr.Y - 1;
            
        }

        public int Down(Vector2 curr)
        {
            return (int) curr.Y + 1;
        }
        
        public int Left(Vector2 curr)
        {
            return (int) curr.X - 1;
            
        }
        
        public int Right(Vector2 curr)
        {
            return (int) curr.X - 1;
            
        }
        
    }
}