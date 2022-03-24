using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Agent : Boardentity
    {
        
        public Vector2 prevLocation { get; set; }
        public Agent(Vector2 location) : base(-1, false, location){
            _location = location;
            _temp = defaultTemp;
            _color = Color.Black;
        }

        public GridLocation[] checkSurrounding(GridLocation[][] grid) {
            var surrounding = new GridLocation[4];
            
            int x = (int)_location.X;
            int y = (int)_location.Y;
            
            surrounding[0] = y == 0                      ? null : grid[x][y - 1]; //Entity above
            surrounding[1] = y == grid[0].Length   ? null : grid[x][y + 1]; //Entity below
            surrounding[2] = x == 0                      ? null : grid[x - 1][y]; //Entity to the left
            surrounding[3] = x == grid.Length      ? null : grid[x + 1][y]; //Entity to the right
            
            return surrounding;
        }

        // This is where the magic happens
        public Vector2 decideDirection(GridLocation[][] grid) {
            var traversableSquares = checkSurrounding(grid);
            traversableSquares = traversableSquares.Where(gridLocation => 
                gridLocation != null 
                && gridLocation._traversable).ToArray();
            
            for (int i = 0; i < traversableSquares.Length; i++)
            {
                var direction = traversableSquares[i];
                if( direction._traversable) return direction._location;
            }
            return _location;
        }

        private bool isLocAllowed(Vector2 loc, GridLocation[][] grid) {
            return      loc.X >= 0 
                    &&  loc.X < grid.Length
                    &&  loc.Y >= 0 
                    &&  loc.Y < grid[0].Length;
        }
        
        public void move(Vector2 toPos, GridLocation[][] grid) {
            if (isLocAllowed(toPos, grid)){
                int fromPosX = (int)_location.X;
                int fromPosY = (int)_location.Y;
                _location = toPos;
                grid[(int)toPos.X][(int)toPos.Y] = this;
                grid[fromPosX][fromPosY] = new GridLocation(1, new Vector2(fromPosX, fromPosY)); //Change to what it was before
                
            }

        }
        public void autoMove(GridLocation[][] grid)
        {
            
            var direction = decideDirection(grid);
            move(direction, grid);

        }
    }
}