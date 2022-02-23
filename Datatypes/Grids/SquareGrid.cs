using System;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;



namespace Swarms.Datatypes.Grids
{
    public class SquareGrid
    {

        public bool showGrid;

        //todo find the equivalent of Basic2D
        // this is supposed to represent the img in each grid square
         public Texture2D gridImg; 

        //slotDims: Size of each slot in the grid
        //gridDims: size of the entire grid. good since we use arrays
        //gridOffset helps us tremendously since reasons
        //totalphysicalDims: slotDims*gridDims
        // caching for what we hover over, prolly doesnt matter to us since this aint no game
        public Vector2 slotDims, gridDims, gridOffset, totalPhysicalDims, currentHoverSlot;

        //this is essentially our matrix for all the squares.
        public GridLocation[][] slots = new GridLocation[10][];

        public SquareGrid(Vector2 SLOTDIMS, Vector2 STARTPOS, Vector2 TOTALDIMS)
        {
            showGrid = false;
            slotDims = SLOTDIMS;

            gridOffset = new Vector2((int)STARTPOS.X, (int)STARTPOS.Y);
            totalPhysicalDims = new Vector2((int)TOTALDIMS.X, (int)TOTALDIMS.Y);

            currentHoverSlot = new Vector2(-1,-1);

            //todo, make this a rectangle yakno
            // maybe make a class to represent a rectangle in the grid, containing the img etc.
            // Maybe this is the reasoning for having this matrix GridLocation[][], and GridLocation is what i'm thinking about?
            // We want the position of this to be with an offset, because we draw from the middle, but the position currently in our grid is the top left corner of each square.
            gridImg = null;

        }

        public SquareGrid()
        {

        }

        public virtual void Update(Vector2 offset){
            
        }

        //if statement simply checks if the location is within bounds.
        public virtual GridLocation getSlotFromLocation(Vector2 loc){
            
            if(loc.X >= 0 && loc.Y >= 0 && loc.X < slots[(int)loc.X].Count()){
                return slots[(int)loc.X][(int)loc.Y];
            }
            return null;
        }

        //this might not be used and doesnt work atm plz no usy papi
        public virtual Vector2 getSlotFromPixel(Vector2 pix, Vector2 offset){
            return slotDims;
        }
    }
}