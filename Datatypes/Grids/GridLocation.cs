using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace Swarms.Datatypes.Grids
{
    //individual grid locations
    public class GridLocation
    {
        //filled = there is something here
        // traversable, means that it is filled by something we can traverse.
        // unPathable = cant make a path but not necessarily filled (edges maybe?)
        // we might not need all these bools
        public bool _filled, _traversable, _unPathable ;
        // some floats for pathfinding, cost is cost to move through a single square.
        public float _currentDist, _cost;

        public Vector2 _parent, _pos;

        public Color _color {get; set;}

        public GridLocation(float cost, bool filled, Vector2 location, bool traversable = true){
            _cost = cost;
            _filled = filled;
            _pos = location;

            _traversable = traversable;
        }
    }
}