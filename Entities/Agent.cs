using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Agent : Boardentity
    {

        public Agent(Vector2 location) : base(-1, false, location){
            _location = location;
            _temp = defaultTemp;
            _color = Color.Black;
        }

        public GridLocation[] checkSurrounding(SquareGrid grid) {
            var surrounding = new GridLocation[4];
            
            int x = (int)_location.X;
            int y = (int)_location.Y;
            
            surrounding[0] = y == 0                      ? null : grid.slots[x][y - 1]; //Entity above
            surrounding[1] = y == grid.slots[0].Length   ? null : grid.slots[x][y + 1]; //Entity below
            surrounding[2] = x == 0                      ? null : grid.slots[x - 1][y]; //Entity to the left
            surrounding[3] = x == grid.slots.Length      ? null : grid.slots[x + 1][y]; //Entity to the right
            
            return surrounding; // TODO: Implement to check for surrounding entities
        }

        // This is where the magic happens
        public Vector2 decideDirection(SquareGrid grid) {
            var traversableSquares = checkSurrounding(grid);
            traversableSquares = traversableSquares.Where(gridLocation => 
                gridLocation != null 
                && gridLocation._traversable == true).ToArray();
            
            for (int i = 0; i < traversableSquares.Length; i++)
            {
                var direction = traversableSquares[i];
                if( direction._traversable == true ) return direction._location;
            }
            return _location;
        }
        private bool isLocAllowed(Vector2 loc, SquareGrid grid) {
            return      loc.X >= 0 
                    &&  loc.X < grid.slots.Length
                    &&  loc.Y >= 0 
                    &&  loc.Y < grid.slots[0].Length;
        }
        public void move(Vector2 toPos, SquareGrid grid) {
            if (isLocAllowed(toPos, grid)){
                int fromPosX = (int)_location.X;
                int fromPosY = (int)_location.Y;

                grid.slots[fromPosX][fromPosY] = new GridLocation(1, false, _location); //Change to what it was before
                grid.slots[(int)toPos.X][(int)toPos.Y] = this;

                _location = toPos;
            }
            else return;   

        }
        public void autoMove(SquareGrid grid) {
            var direction = decideDirection(grid);
            move(direction, grid);
        }
    }
}