using System.Diagnostics;
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
        
        //TODO: Change to be relative to screen size
        readonly int SQUARE_SIZE = 100;
        
        public Vector2 rectPosition;
        public float rectSpeed = 100f;

        public Texture2D rectTexture;
        // this is supposed to represent the img in each grid square
        public Texture2D gridImg; 

        //slotDims: Size of each slot in the grid
        //gridDims: size of the entire grid. good since we use arrays
        //gridOffset helps us tremendously since reasons
        //totalphysicalDims: slotDims*gridDims
        // caching for what we hover over, prolly doesnt matter to us since this aint no game
        public Vector2 slotDims, gridDims, gridOffset, totalPhysicalDims, currentHoverSlot;

        //this is essentially our matrix for all the squares.
        public GridLocation[][] slots;
        public int gridSize = 40;


        public SquareGrid(Vector2 SLOTDIMS, Vector2 STARTPOS, Vector2 TOTALDIMS, GraphicsDevice _graphics)
        {
            showGrid = true;
            slotDims = new Vector2(_screenWidth/gridSize,_screenHeight/gridSize);
            gridOffset = new Vector2((int)STARTPOS.X, (int)STARTPOS.Y);
            totalPhysicalDims = new Vector2((int)TOTALDIMS.X, (int)TOTALDIMS.Y);

            currentHoverSlot = new Vector2(-1,-1);
            
            LoadContent(_graphics);
            setBaseGrid();

            gridImg = null;

        }

        public SquareGrid()
        {

        }

        public void LoadContent(GraphicsDevice graphics){
            rectTexture = new Texture2D(graphics, 1, 1);
            rectTexture.SetData(new[] {Color.White});
        }
        //When to update?
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
            
            return tempVec;
        }

        // size of slot divided by number of slots, i would say we just initialize it with these dims in the constructor.   
        public virtual void setBaseGrid(){

            // 40/1.6 = 25, this is aspect ratio stuff, TODO: stop magic numbering trond
            gridDims = new Vector2(gridSize,25);
            slots = new GridLocation[(int) gridDims.X][];
            //make sure our grid is clear initially
            Array.Clear(slots, 0, slots.Length);
            
            for(int i = 0; i < gridDims.X; i++){
                //this might fuck shit up, but adds to rows

                slots[i] = new GridLocation[(int)gridDims.X];
                for(int j=0; j<gridDims.Y ; j++){
                    slots[i][j] = new GridLocation(1, false);
                }
            }

        }


  

        public virtual void drawGrid(Vector2 offset, SpriteBatch spriteBatch, Texture2D texture){
            Vector2 topLeft = getSlotFromPixel(new Vector2(0,0), Vector2.Zero);
            Vector2 botRight = getSlotFromPixel(new Vector2(_screenWidth,_screenHeight), Vector2.Zero);
            var slotCounter = 0;
            var rowNum = 25;
            //needs some actual drawing logic i guess
            if(showGrid){
                spriteBatch.Begin();
                //dimensional check, draw this out at some point 
                for(int j=(int)topLeft.X; j<= botRight.X && j<slots.Count(); j++){
                    //var yOffset = offset.Y + 50*j;
                    var xOffset = offset.X + slotDims.X*j;
                    for (int k=(int)topLeft.Y; k <= botRight.Y && k < rowNum; k++){
                        //Since we're using a KVADRAT XD, the offset needs to be of same size in both Y and X direction, therefore slotDims.X*k.
                        var yOffset = offset.Y + slotDims.X*k;
                        //spriteBatch.Draw(rectTexture, topLeft, Color.Black);
                        RectangleSprite.DrawRectangle(spriteBatch, new Rectangle((int)xOffset, (int)yOffset, (int)slotDims.X, (int)slotDims.X),Color.Black,2);
                        slotCounter++;
                        //TODO Theres some true shitfuckery on the go right here, look into our array accesses.
                         //drawing logic goes here
                    }
                }
                spriteBatch.End();
                Trace.WriteLine(slotCounter);
            }
        }
           

    }
}
