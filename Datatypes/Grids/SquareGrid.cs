using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Swarms.Entities;
using System.Linq;



namespace Swarms.Datatypes.Grids
{
    public class SquareGrid
    {

        public bool showGrid;

        //UPDATE THESE IF THE HEIGHT AND WIDTH CHANGES OBVIOUSLY ;DD;D;D;D;;D;D;D DONT U... FORGET ABOUT ME.

        //DONT DONT DONT DONT
        public int _screenHeight { get; set; }
        public int _screenWidth { get; set; }

        public Vector2 rectPosition;
        public float rectSpeed = 100f;

        public GraphicsDevice _graphics { get; set; }



        //slotDims: Size of each slot in the grid
        //gridDims: size of the entire grid. good since we use arrays
        //gridOffset helps us tremendously since reasons
        //totalphysicalDims: slotDims*gridDims
        // caching for what we hover over, prolly doesnt matter to us since this aint no game
        public Vector2 _gridOffset, _currentHoverSlot;

        public float _slotDim;

        //this is essentially our matrix for all the squares.
        public GridLocation[][] _slots { get; set; }

        public List<Agent> _agentList { get; set; }
        public List<Tree> _treeList { get; set; }
        //this could just be made into a vector where X represents rows and Y represents columns but cba

        //I mean yes but wouldn't it be better to separate this from our model logic
        public int _rowNums { get; set; } // Y-dimension
        public int _columnNums { get; set; }  // X-dimension
        public SpriteFont _font { get; set; }

        public SquareGrid(Vector2 startPos, GraphicsDevice graphics, int screenWidth, int screenHeight, int columnNums, int rowNums, bool isLogging, SpriteFont font)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _font = font;
            _rowNums = rowNums;
            _columnNums = columnNums;

            showGrid = true;
            _slotDim = Math.Min(_screenWidth / _columnNums, _screenHeight / _rowNums); //We simply pick the smallest dimension of the squares to make sure all can fit on the screen
            _gridOffset = new Vector2((int)startPos.X, (int)startPos.Y);

            _currentHoverSlot = new Vector2(-1, -1); // For debugging purposes

            _agentList = new List<Agent>();
            _treeList = new List<Tree>();
            _graphics = graphics;
            LoadContent(graphics);

            setBaseGrid();
            //need to add trees and obstacles correctly.
            setSimulationGrid();
            //setBigGrid(); //works on 48/80 grid
            //setRiverGrid(); //only works on 24/40 grid
            //setDenseForest();

            //TODO: Remove this for final product
            //Weighted decision debug stuff

            /* addAgent(new Vector2(5,5));
            addTree(new Vector2(5,7), 160, 1);
            addTree(new Vector2(7,5), 380, 2);
            addTree(new Vector2(5,6), 80, 3);
            addTree(new Vector2(6,5), 30, 4); */

        }

        public SquareGrid()
        {
        }


        public void LoadContent(GraphicsDevice graphics)
        {
            // rectTexture = new Texture2D(graphics, 1, 1);
            // rectTexture.SetData(new[] {Color.White});

        }

        //When to update?
        public virtual void Update()
        {
            _currentHoverSlot = getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
        }

        //if statement simply checks if the location is within bounds.
        public virtual GridLocation getSlotFromLocation(Vector2 loc)
        {

            if (loc.X >= 0 && loc.Y >= 0 && loc.X < _slots[(int)loc.X].Count())
            {
                return _slots[(int)loc.X][(int)loc.Y];
            }
            return null;
        }

        public void addAgent(Vector2 position)
        {
            var agent = new Agent(position);
            _slots[(int)position.X][(int)position.Y] = agent;
            _agentList.Add(agent);
        }

        public void removeAgent(Agent agent)
        {
            _agentList.Remove(agent);
        }

        public void addTree(Vector2 pos, int temp = 100, int id = 0)
        {
            var (x, y) = pos;
            var tree = new Tree(new Vector2(x, y), temp, id);
            _slots[(int)x][(int)y] = tree;
            _treeList.Add(tree);
        }

        public virtual bool isFilled(Vector2 slot)
        {
            var type = _slots[(int)slot.X][(int)slot.Y].GetType();
            return type == typeof(Agent) || type == typeof(Tree) || type == typeof(Obstacle);
        }

        public virtual Vector2 getSlotFromPixel(Vector2 pix)
        {
            Vector2 adjustedPos = pix - _gridOffset;

            Vector2 tempVec = new Vector2(Math.Min(Math.Max(0, (int)(adjustedPos.X / _slotDim)), _slots.Count() - 1), Math.Min(Math.Max(0, (int)(adjustedPos.Y / _slotDim)), _slots[0].Count() - 1));

            return tempVec;
        }

        // size of slot divided by number of _slots, i would say we just initialize it with these dims in the constructor.   
        public void setBaseGrid()
        {

            _slots = new GridLocation[_columnNums][];

            Array.Clear(_slots, 0, _slots.Length);

            for (int i = 0; i < _columnNums; i++)
            {

                _slots[i] = new GridLocation[_rowNums];
                for (int j = 0; j < _rowNums; j++)
                {
                    _slots[i][j] = new GridLocation(1, new Vector2(i, j));
                }
            }

        }
        public void setSimulationGrid()
        {

            //initTenAgents();
              initTwentyAgents(23);
            //These two initTwentyAgents call will make 40 agents in total :)
            // initTwentyAgents(22);
            //  initTwentyAgents(21);
             InitTopLeftTreeCluster();
            //  initBotMidTreeCluster();
             // initTopRightTreeCluster();

            //initSurrounded();



        }


        public void initSurrounded()
        {
            InitTopLeftTreeCluster();
            initBotMidTreeCluster();
            initTopRightTreeCluster();
            initSurroundedAgents();



        }

        private void initSurroundedAgents()
        {
            addAgent(new Vector2(18, 9));
            addAgent(new Vector2(18, 8));
            addAgent(new Vector2(18, 7));
            addAgent(new Vector2(19, 6));
            addAgent(new Vector2(19, 7));
            addAgent(new Vector2(19, 8));
            addAgent(new Vector2(19, 9));
            addAgent(new Vector2(20, 9));
            addAgent(new Vector2(20, 8));
            addAgent(new Vector2(20, 7));

        }

        public void InitTopLeftTreeCluster()
        {

            addTree(new Vector2(3, 3));
            addTree(new Vector2(4, 3));
            addTree(new Vector2(5, 3));
            addTree(new Vector2(5, 4));
            addTree(new Vector2(5, 5));
            addTree(new Vector2(4, 5));
            addTree(new Vector2(3, 5));
            addTree(new Vector2(3, 4));
        }
        private void initTopRightTreeCluster()
        {
            addTree(new Vector2(34, 3));
            addTree(new Vector2(35, 3));
            addTree(new Vector2(36, 3));
            addTree(new Vector2(36, 4));
            addTree(new Vector2(36, 5));
            addTree(new Vector2(35, 5));
            addTree(new Vector2(34, 5));
            addTree(new Vector2(34, 4));
        }

        private void initBotMidTreeCluster()
        {
            addTree(new Vector2(18, 18));
            addTree(new Vector2(19, 18));
            addTree(new Vector2(20, 18));
            addTree(new Vector2(20, 19));
            addTree(new Vector2(20, 20));
            addTree(new Vector2(19, 20));
            addTree(new Vector2(18, 20));
            addTree(new Vector2(18, 19));
        }



        public void initTenAgents()
        {
            for (int i = 14; i < 24; i++)
            {
                addAgent(new Vector2(i, 23));
            }
        }
        public void initTwentyAgents(int yPos)
        {
            for (int i = 9; i < 29; i++)
            {
                addAgent(new Vector2(i, yPos));
            }
        }



        private void setBigGrid()
        {

            for (int i = (_columnNums / 3); i < _columnNums - 10; i++)
            {
                addAgent(new Vector2(i, 34));
            }

            for (int i = 0; i < _columnNums; i++)
            {
                if (i % 4 != 0) _slots[i][24] = new Obstacle(new Vector2(i, 24));
            }

            for (int i = 0; i < _columnNums; i++)
            {
                for (int j = 0; j < 24; j++)
                    if (i % 3 == 0 && j % 2 == 0)
                    {
                        addTree(new Vector2(i, j));
                    }
            }
        }

        //Adds entities to a board in a structured fashion.
        public void setRiverGrid()
        {

            for (int i = 13; i < 23; i++)
            {
                addAgent(new Vector2(i, 22));
            }
            for (int i = 0; i < 40; i++)
            {
                if (i % 5 != 0)
                {
                    _slots[i][16] = new Obstacle(new Vector2(i, 16));
                }
            }
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i % 3 == 0 && j % 2 == 0)
                    {
                        addTree(new Vector2(i, j));
                    }
                }
            }

        }
        private void setDenseForest()
        {
            for (int i = 13; i < 23; i++)
            {
                addAgent(new Vector2(i, 22));
            }
            for (int i = 0; i < 40; i++)
            {
                if (i % 5 != 0)
                {
                    _slots[i][16] = new Obstacle(new Vector2(i, 16));
                }
            }
            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    addTree(new Vector2(i, j));
                }
            }
        }

        public virtual void drawGrid(Vector2 offset, SpriteBatch spriteBatch, Texture2D texture)
        {
            //needs some actual drawing logic i guess

            if (showGrid)
            {
                spriteBatch.Begin();
                //dimensional check, draw this out at some point 
                for (int j = 0; j < _columnNums; j++)
                {
                    //var yOffset = offset.Y + 50*j;
                    var xOffset = (int)(offset.X + _slotDim * j);

                    for (int k = 0; k < _rowNums; k++)
                    {
                        //Since we're using a KVADRAT XD, the offset needs to be of same size in both Y and X direction, therefore slotDims.X*k.
                        var yOffset = (int)(offset.Y + _slotDim * k);

                        var color = _slots[j][k]._color;
                        RectangleSprite.DrawRectangle(spriteBatch, new Rectangle(xOffset, yOffset, (int)_slotDim, (int)_slotDim), Color.White, 2);
                        switch (_slots[j][k].GetType().Name)
                        {
                            case nameof(Agent):
                                RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDim, (int)_slotDim), color);
                                break;
                            case nameof(Tree):
                                RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDim, (int)_slotDim), color);
                                break;
                            case nameof(Obstacle):
                                RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDim, (int)_slotDim), color);
                                break;

                        }

                    }
                }
                spriteBatch.End();
            }
        }
        private Heatmapper heatmapper = new Heatmapper();
        public virtual void drawHeatMap(Vector2 offset, SpriteBatch spriteBatch, int[][] array, int totalSims)
        {
            spriteBatch.Begin();
            for (int i = 0; i < _columnNums; i++)
            {
                var xOffset = (int)(offset.X + _slotDim * i);
                for (int j = 0; j < _rowNums; j++)
                {
                    var yOffset = (int)(offset.Y + _slotDim * j);
                    var color = heatmapper.HeatMap(array[i][j], 0, totalSims);

                    RectangleSprite.DrawRectangle(spriteBatch, new Rectangle(xOffset, yOffset, (int)_slotDim, (int)_slotDim), Color.White, 2);

                    RectangleSprite.FillRectangle(spriteBatch, new Rectangle(xOffset + 2, yOffset + 2, (int)_slotDim, (int)_slotDim), color);
                    decimal percentOfAgents = Math.Round((((decimal)array[i][j]) / ((decimal)_agentList.Count * (decimal)totalSims)) * 100, 1);
                    if(array[i][j] < 0) percentOfAgents = (array[i][j] + 1) * -1;
                    if (array[i][j] != 0 && percentOfAgents > (Decimal) 0.1) spriteBatch.DrawString(_font, $"{percentOfAgents}%".ToString(), new Vector2(xOffset + 2, yOffset + 2), Color.Magenta);

                }
            }
            spriteBatch.End();
        }
        public static Random rand = new Random();
        public SquareGrid TickOnce()
        {

            foreach (var agent in _agentList.OrderBy(a => rand.Next(_agentList.Count())))
            {
                agent.toRuleThemAll(this);
                //UpdateGrid();
            }

            foreach (var tree in _treeList.OrderBy(a => rand.Next(_treeList.Count())))
            {
                tree.tickTemp(_slots, _treeList);
                // UpdateGrid();
            }
            UpdateGrid();
            return this;
        }


        public void UpdateGrid()
        {
            foreach (var agent in _agentList)
            {
                _slots[(int)agent._location.X][(int)agent._location.Y] = agent;
            }

            foreach (var tree in _treeList)
            {
                _slots[(int)tree._location.X][(int)tree._location.Y] = tree;
            }
        }
    }
}
