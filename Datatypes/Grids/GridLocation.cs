using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Swarms.Datatypes.Grids
{
    //individual grid locations
    public class GridLocation
    {
        //filled = there is something here
        // traversable, means that it is filled by something we can traverse.
        // unPathable = cant make a path but not necessarily filled (edges maybe?)
        // we might not need all these bools
        public bool filled, traversable, unPathable;
        // some floats for pathfinding, cost is cost to move through a single square.
        public float currentDist, cost;

        public Vector2 parent, pos;

        public GridLocation(float Cost, bool FILLED){
            cost = Cost;
            filled = FILLED;

            traversable = true;
            
        
        }
    }
}