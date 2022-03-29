using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Agent : Boardentity
    {
        
        public Vector2 _prevLocation { get; set; }
        public Agent(Vector2 location) : base(-1, false, location){
            _location = location;
            _temp = defaultTemp;
            _color = Color.Black;
        }
        
        // -------Mulig Optimering-------
        // Måske en IEnumerable<Vector2> eller andet her for memory reasons
        private List<Vector2> getAdjacent()
        {
            var adjacent = new List<Vector2>();
            
            adjacent.Add(new Vector2(_location.X, _location.Y - 1)); //up
            adjacent.Add(new Vector2(_location.X, _location.Y + 1)); //down
            adjacent.Add(new Vector2(_location.X - 1, _location.Y)); //left
            adjacent.Add(new Vector2(_location.X + 1, _location.Y)); //right
            
            return adjacent;
        }

        private List<Vector2> checkAvailable(GridLocation[][] grid)
        {
            var available = getAdjacent().Where(position => isWithinBounds(position, grid) && isTraversable(position, grid)).ToList();
            return available;
        }
        
        private bool isTraversable(Vector2 location, GridLocation[][] grid) {
            return grid[(int) location.X][(int) location.Y]._traversable;
        }

        private bool isWithinBounds(Vector2 loc, GridLocation[][] grid) {
            return      loc.X >= 0 
                        &&  loc.X < grid.Length
                        &&  loc.Y >= 0 
                        &&  loc.Y < grid[0].Length;
        }

        // This is where the magic happens
        private Vector2 randomDirection(GridLocation[][] grid) {
            List<Vector2> traversableSquares = checkAvailable(grid);
            
            var randomize = new Random().Next(0, traversableSquares.Count);

            var direction = traversableSquares[randomize];
            
            if(traversableSquares.Count == 0) return this._location;
            else return direction;
        }
        
        private void move(GridLocation[][] grid)
        {
            var newPos = randomDirection(grid);
            var from = _location;
            _location = newPos;
            _prevLocation = from;

        }

        public void autoMove(GridLocation[][] grid)
        {
           // var direction = randomDirection(grid);
            move(grid);
        }
    }
}