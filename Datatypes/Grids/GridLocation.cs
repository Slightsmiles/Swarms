using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace Swarms.Datatypes.Grids
{
    //individual grid locations
    public class GridLocation
    {
        //filled = there is something here
        // traversable, means that it is filled by something we can traverse.
        // unPathable = cant make a path but not necessarily filled (edges maybe?)
        // we might not need all these bools
        public bool _traversable {get; set;} 
        //public bool _unPathable {get; protected set;}
        // some floats for pathfinding, cost is cost to move through a single square.
      //  public float _currentDist, _cost;

        public Vector2 _parent {get;  set;}
        public Vector2 _location {get; set;}

        public Color _color {get; set;}

        public GridLocation(float cost, Vector2 location, bool traversable = true){
           // _cost = cost;
            _location = location;

            _traversable = traversable;
        }

        public GridLocation(){}
        protected List<Vector2> getAdjacent(GridLocation[][] grid, int range = 2)
        {
            var adjacent = new List<Vector2>();

            for (int i = -range; i  <= range; i++)
            {
                for (int j = -range; j <= range; j++)
                {
                    var loc = new Vector2(_location.X + i, _location.Y + j);

                    if((i==0 && j==0) || !isWithinBounds(loc, grid) ) continue;
                    
                    adjacent.Add(loc);
                }

            }
            
            return adjacent;
        }

         private bool isWithinBounds(Vector2 loc, GridLocation[][] grid) {
            return      loc.X >= 0 
                        &&  loc.X < grid.Length
                        &&  loc.Y >= 0 
                        &&  loc.Y < grid[0].Length;
        }

         public double getEuclidianDistance(Vector2 target, Vector2 agent)
         {
             var dist = Math.Sqrt(Math.Pow(target.X - agent.X, 2) + Math.Pow(target.Y - agent.Y, 2));
             
             // we allegedly want 
             var reciprocralVal = 1 / dist;
             
             return dist;
         }
    }
}