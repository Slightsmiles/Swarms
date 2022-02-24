using System.Net;
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

        //UPDATE THESE IF THE HEIGHT AND WIDTH CHANGES OBVIOUSLY ;DD;D;D;D;;D;D;D DONT U... FORGET ABOUT ME.

        //DONT DONT DONT DONT
        public int _screenHeight = 480;
        public int _screenWidth = 800;
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
            showGrid = true;
            slotDims = SLOTDIMS;

            gridOffset = new Vector2((int)STARTPOS.X, (int)STARTPOS.Y);
            totalPhysicalDims = new Vector2((int)TOTALDIMS.X, (int)TOTALDIMS.Y);

            currentHoverSlot = new Vector2(-1,-1);

            setBaseGrid();

            //todo, make this a rectangle yakno
            // maybe make a class to represent a rectangle in the grid, containing the img etc.
            // Maybe this is the reasoning for having this matrix GridLocation[][], and GridLocation is what i'm thinking about?
            // We want the position of this to be with an offset, because we draw from the middle, but the position currently in our grid is the top left corner of each square.
            gridImg = null;

        }

        public SquareGrid()
        {

        }
        //this won't work until we figure out how to get current mouse position and remove that stupid Global shit
        public virtual void Update(Vector2 offset){
            currentHoverSlot = getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), -offset);
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
            Vector2 adjustedPos = pix - gridOffset + offset;

            Vector2 tempVec = new Vector2(Math.Min(Math.Max(0,(int)(adjustedPos.X/slotDims.X)), slots.Count()-1), Math.Min(Math.Max(0, (int)(adjustedPos.Y/slotDims.Y)), slots[0].Count()-1));
            
            return slotDims;
        }

        // size of slot divided by number of slots, i would say we just initialize it with these dims in the constructor.   
        public virtual void setBaseGrid(){
            gridDims = new Vector2((int)(totalPhysicalDims.X/slotDims.X),totalPhysicalDims.X/slotDims.X);
            
            //make sure our grid is clear initially
            Array.Clear(slots, 0, slots.Length);

            for(int i = 0; i> gridDims.X; i++){
                //this might fuck shit up, but adds to rows
                slots[i] = new GridLocation[(int)gridDims.Y];
                for(int j=0; j<gridDims.Y; i++){
                    slots[i][j] = new GridLocation(1, false);
                }
            }
        }

        public virtual void drawGrid(Vector2 offset){
            Vector2 topLeft = getSlotFromPixel(new Vector2(0,0), Vector2.Zero);
            //botRight MIGHT be iffy, i took it from debugging Game1.cs and checking our values out
            Vector2 botRight = getSlotFromPixel(new Vector2(_screenWidth,_screenHeight), Vector2.Zero);

            //needs some actual drawing logic i guess
            if(showGrid){
                //dimensional check, draw this out at some point 
                for(int j=(int)topLeft.X; j<= botRight.X && j<slots.Count(); j++){
                    for (int k=(int)topLeft.Y; k <= botRight.Y && k<slots[0].Count(); k++){
                        
                        
                         //drawing logic goes here
                    }
                }
            }
        }
    }
}
