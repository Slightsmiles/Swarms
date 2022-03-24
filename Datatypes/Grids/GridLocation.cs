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
        public bool _traversable {get; protected set;} 
        public bool _unPathable {get; protected set;}
        // some floats for pathfinding, cost is cost to move through a single square.
        public float _currentDist, _cost;

        public Vector2 _parent {get; protected set;}
        public Vector2 _location {get; set;}

        public Color _color {get; set;}

        public GridLocation(float cost, Vector2 location, bool traversable = true){
            _cost = cost;
            _location = location;

            _traversable = traversable;
        }


    }
}