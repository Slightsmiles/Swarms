using System;
using Microsoft.Xna.Framework;
using Swarms.Datatypes.Grids;

namespace Swarms.Entities
{
    public class Agent : Boardentity
    {
        public Agent(){
            this.location = new Vector2(-1,-1);
            this.temp = defaultTemp;
            color = Color.Black;
        }

        public Agent(Vector2 location){
            this.location = location;
            this.temp = defaultTemp;
            color = Color.Black;
        }

        public GridLocation[] checkSurrounding(SquareGrid grid) {
            var surrounding = new GridLocation[4];
            
            int x = (int)location.X;
            int y = (int)location.Y;
            
            surrounding[0] = y == 0                      ? null : grid.slots[x][y - 1]; //Entity above
            surrounding[1] = y == grid.slots[0].Length   ? null : grid.slots[x][y + 1]; //Entity below
            surrounding[2] = x == 0                      ? null : grid.slots[x - 1][y]; //Entity to the left
            surrounding[3] = x == grid.slots.Length      ? null : grid.slots[x + 1][y]; //Entity to the right
            
            return surrounding; // TODO: Implement to check for surrounding entities
        }

        public Vector2 decideDirection(SquareGrid grid) {
            var traversableSquares = new GridLocation[4];
            var surroundingSquares = checkSurrounding(grid);

            return new Vector2(1,1);
        }

        private bool isLocAllowed(Vector2 loc, SquareGrid grid) {
            return      loc.X >= 0 
                    &&  loc.X < grid.slots.Length
                    &&  loc.Y >= 0 
                    &&  loc.Y < grid.slots[0].Length;
        }
        public void move(Vector2 toPos, SquareGrid grid) {
            if (isLocAllowed(toPos, grid)){
                int fromPosX = (int)this.location.X;
                int fromPosY = (int)this.location.Y;

                grid.slots[fromPosX][fromPosY] = new GridLocation(1, false);
                grid.slots[(int)toPos.X][(int)toPos.Y] = this;

                location = toPos;
            }
            else return;   

        }
    }
}