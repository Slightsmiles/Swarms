using System.Diagnostics;
using System.Net;
using System;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Globalization;
using Swarms.Entities;
using System.Linq;


namespace Swarms.Datatypes.Grids
{
    public class SquareGrid
    {

        public bool showGrid;

        //UPDATE THESE IF THE HEIGHT AND WIDTH CHANGES OBVIOUSLY ;DD;D;D;D;;D;D;D DONT U... FORGET ABOUT ME.

        //DONT DONT DONT DONT
        public int _screenHeight {get; private set;}
        public int _screenWidth {get; private set;}
        
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
        public Vector2 _slotDims, _gridOffset, _currentHoverSlot;

        //this is essentially our matrix for all the squares.
        public GridLocation[][] _slots {get; set;}

        public List<Agent> _agents;

        //this could just be made into a vector where X represents rows and Y represents columns but cba
        //btw i refuse to figure out what a row is and what a column is sooooooooooo WEHU

        //I mean yes but wouldn't it be better to separate this from our model logic
        private int _rowNums; // Y-dimension
        private int _columnNums; // X-dimension


        public SquareGrid(Vector2 startPos, GraphicsDevice graphics, int screenWidth, int screenHeight, int rowNums = 24, int columnNums = 40)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _rowNums = rowNums;
            _columnNums = columnNums;

            showGrid = true;
            _slotDims = new Vector2(_screenWidth / _columnNums, _screenHeight / _rowNums);
            _gridOffset = new Vector2((int)startPos.X, (int)startPos.Y);

            _currentHoverSlot = new Vector2(-1,-1); // For debugging purposes
            
            LoadContent(graphics);
            setBaseGrid();
            Console.WriteLine(_slots.Length);

            setRiverGrid();
            gridImg = null;

        }

        public void LoadContent(GraphicsDevice graphics){
            rectTexture = new Texture2D(graphics, 1, 1);
            rectTexture.SetData(new[] {Color.White});
 
        }

        //When to update?
        public virtual void Update(){
            _currentHoverSlot = getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
        }

        //if statement simply checks if the location is within bounds.
        public virtual GridLocation getSlotFromLocation(Vector2 loc){
            
            if(loc.X >= 0 && loc.Y >= 0 && loc.X < _slots[(int)loc.X].Count()){
                return _slots[(int)loc.X][(int)loc.Y];
            }
            return null;
        }

        public virtual bool isFilled(Vector2 slot)
        {
            var type = _slots[(int) slot.X][(int) slot.Y].GetType();
            return type == typeof(Agent) || type == typeof(Tree) || type == typeof(Obstacle);
        }
        
        public virtual Vector2 getSlotFromPixel(Vector2 pix){
            Vector2 adjustedPos = pix - _gridOffset;

            Vector2 tempVec = new Vector2(Math.Min(Math.Max(0,(int)(adjustedPos.X/_slotDims.X)), _slots.Count()-1), Math.Min(Math.Max(0, (int)(adjustedPos.Y/_slotDims.X)), _slots[0].Count()-1));
            
            return tempVec;
        }

        // size of slot divided by number of _slots, i would say we just initialize it with these dims in the constructor.   
        public void setBaseGrid(){

            // 40/1.6 = 25, this is aspect ratio stuff, TODO: stop magic numbering trond
            _slots = new GridLocation[_columnNums][];
            //make sure our grid is clear initially
            Array.Clear(_slots, 0, _slots.Length);
            
            for(int i = 0; i < _columnNums; i++){
                //this might fuck shit up, but adds to rows

                _slots[i] = new GridLocation[_rowNums];
                for(int j = 0; j < _rowNums ; j++){
                    _slots[i][j] = new GridLocation(1, new Vector2 (i, j));
                }
            }

        }

        //Adds entities to a board in a structured fashion.
        public void setRiverGrid(){
            for (int i = 13; i<23; i++ ){
                _slots[i][22] = new Agent(new Vector2(i,22));
            }
            for (int i = 0; i<40; i++){
                if (i % 5 != 0){
                _slots[i][16] = new Obstacle(new Vector2(i,16));
                }
            }
            for (int i = 0; i < 40; i++){
                for (int j = 0; j<8; j++){
                    if (i % 3 == 0 && j % 2 == 0){
                        _slots[i][j] = new Tree(new Vector2(i,j));
                    } 
                }
            }

        }

        public virtual void drawGrid(Vector2 offset, SpriteBatch spriteBatch, Texture2D texture){
            //needs some actual drawing logic i guess
            if(showGrid){
                spriteBatch.Begin();
                //dimensional check, draw this out at some point 
                for(int j = 0; j < _columnNums; j++){
                    //var yOffset = offset.Y + 50*j;
                    var xOffset = (int)(offset.X + _slotDims.X * j);

                    for (int k = 0; k < _rowNums; k++){
                        //Since we're using a KVADRAT XD, the offset needs to be of same size in both Y and X direction, therefore slotDims.X*k.
                        var yOffset = (int)(offset.Y + _slotDims.X * k);

                        var color = _slots[j][k]._color;
                        RectangleSprite.DrawRectangle(spriteBatch, new Rectangle(xOffset, yOffset, (int)_slotDims.X, (int)_slotDims.X),Color.White,2);
                        switch(_slots[j][k].GetType().Name){
                            case nameof(Agent):
                                RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDims.X, (int)_slotDims.X), color);
                                break;
                            case nameof(Tree):
                                RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDims.X, (int)_slotDims.X),color);
                                break;
                            case nameof(Obstacle):
                                RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDims.X, (int)_slotDims.X),color);
                                break;  

                        }
                        
                    }
                }
                spriteBatch.End();
            }
        }

        private void move(Agent agent)
        {
            var newPos = decideMove(agent);
            var from = agent._location;
            agent._location = newPos;
            agent.prevLocation = from;
            //_slots[(int)newPos.X][(int)newPos.Y] = agent;
            //_slots[(int)from.X][(int)from.Y] = new Boardentity(1, true, from);

        }

        private Vector2 decideMove(Agent agent)
        {
            var possibleDirections = checkAvailable(agent);
            return possibleDirections.First();

        }

        private List<Vector2> getAdjacent(Vector2 loc)
        {

            var up = new Vector2(loc.X - 1, loc.Y);
            var down = new Vector2(loc.X + 1, loc.Y);
            var left = new Vector2(loc.X , loc.Y -1);
            var right = new Vector2(loc.X , loc.Y +1 );
            var adjacent = new List<Vector2>();
            
            adjacent.Add(up);
            adjacent.Add(down);
            adjacent.Add(left);
            adjacent.Add(right);
            return adjacent;
        }
        private List<Vector2> checkAvailable(Agent agent)
        {
            var available = new List<Vector2>();
            

            foreach (var position in getAdjacent(agent._location))
            {
                
                if (isWithinBounds(position))
                {
                    if (_slots[(int) position.X][(int) position.Y]._traversable)
                    {
                        available.Add(position);
                    }
                    
                }
            }
            
            return available;
        }


        private bool isWithinBounds(Vector2 loc) {
            return      loc.X >= 0 
                        &&  loc.X < _slots.Length
                        &&  loc.Y >= 0 
                        &&  loc.Y < _slots[0].Length;
        }
        public SquareGrid autoMove()
        {
            _agents = new List<Agent>();
            _agents.Clear();
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    switch (_slots[i][j].GetType().Name)
                    {
                        case nameof(Agent):
                            var agent = (Agent)_slots[i][j];
                            _agents.Add(agent);
                            move(agent);
                            break;
                            
                    }
                }
            }

            UpdateGrid();
            return this;
        }


        public void UpdateGrid()
        {
            foreach (var agent in _agents)
            {
                Console.WriteLine("yeet");
                Console.WriteLine("location is: " + agent._location.X + " , " + agent._location.Y);
                _slots[(int) agent.prevLocation.X][(int) agent.prevLocation.Y] = new Boardentity(1, true, agent.prevLocation);
                _slots[(int) agent._location.X][(int) agent._location.Y] = agent;
            }
        }
           
    
    }
}
