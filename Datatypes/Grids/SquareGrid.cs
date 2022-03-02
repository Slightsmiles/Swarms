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
        public GridLocation[][] slots = new GridLocation[40][];
        


        public SquareGrid(Vector2 SLOTDIMS, Vector2 STARTPOS, Vector2 TOTALDIMS, GraphicsDevice _graphics)
        {
            showGrid = true;
            slotDims = SLOTDIMS;

            gridOffset = new Vector2((int)STARTPOS.X, (int)STARTPOS.Y);
            totalPhysicalDims = new Vector2((int)TOTALDIMS.X, (int)TOTALDIMS.Y);

            currentHoverSlot = new Vector2(-1,-1);
            LoadContent(_graphics);
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

        public void LoadContent(GraphicsDevice graphics){
            rectTexture = new Texture2D(graphics, 1, 1);
            rectTexture.SetData(new[] {Color.White});
        }
        //this won't work until we figure out how to get current mouse position and remove that stupid Global shit
        public virtual void Update(Vector2 offset){
            currentHoverSlot = getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), -offset);
            Trace.WriteLine(currentHoverSlot);
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


            gridDims = new Vector2((int)(totalPhysicalDims.X/slotDims.X),totalPhysicalDims.X/slotDims.X);
            
            //make sure our grid is clear initially
            Array.Clear(slots, 0, slots.Length);
            
            for(int i = 0; i < gridDims.X; i++){
                //this might fuck shit up, but adds to rows

                slots[i] = new GridLocation[(int)gridDims.Y];
                for(int j=0; j<gridDims.Y ; j++){
                    slots[i][j] = new GridLocation(1, false);
                }
            }

            // FROM BELOW IS COPYPASTED AND PART OF SMIMONS SMOLUTION:
          //  rectPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
           // rectSpeed = 100f;
           // rectPosition = new Vector2(0,0);
        }


  

        public virtual void drawGrid(Vector2 offset, SpriteBatch spriteBatch, Texture2D texture){
            Vector2 topLeft = getSlotFromPixel(new Vector2(0,0), Vector2.Zero);
            //botRight MIGHT be iffy, i took it from debugging Game1.cs and checking our values out
            Vector2 botRight = getSlotFromPixel(new Vector2(_screenWidth,_screenHeight), Vector2.Zero);


            //needs some actual drawing logic i guess
            if(showGrid){
                spriteBatch.Begin();
                //dimensional check, draw this out at some point 
                for(int j=(int)topLeft.X; j<= botRight.X && j<slots.Count(); j++){
                    //var yOffset = offset.Y + 50*j;
                    var yOffset = offset.Y + SQUARE_SIZE*j;
                    for (int k=(int)topLeft.Y; k <= botRight.Y && k < slots[0].Count(); k++){
                        var xOffset = offset.X + SQUARE_SIZE*k;

                        spriteBatch.Draw(rectTexture, topLeft, Color.Black);
                        RectangleSprite.DrawRectangle(spriteBatch, new Rectangle((int)yOffset, (int)xOffset, SQUARE_SIZE, SQUARE_SIZE),Color.Black,10);
                        
                        //TODO Theres some true shitfuckery on the go right here, look into our array accesses.
                         //drawing logic goes here
                    }
                }
                spriteBatch.End();
            }
        }
        public virtual void drawRectangle(Vector2 pos, Vector2 offset){


        }   

    }
}
