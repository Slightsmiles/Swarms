﻿
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using System.IO;

using System.Xml.Serialization;
using Swarms.Datatypes.Grids;
using Swarms.Entities;

namespace Swarms
{
    public class Game1 : Game
    {
        private const int DEFAULT_SCREEN_HEIGHT = 480;
        private const int DEFAULT_SCREEN_WIDTH = 800;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public int _screenWidth { get; private set; }
        public int _screenHeight { get; private set; }

        public SquareGrid _grid;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public int _gridSizeX { get; set; }
        public int _gridSizeY { get; set; }


        //Logging variables
        public int _tickCounter { get; set; }
        public Logger _logger { get; set; }
        public int lowTest { get; set; }
        public int midTest { get; set; }
        public int highTest { get; set; }
        public bool IsLogging { get; set; }
        public SquareGrid startingGrid { get; set; }

        public int totalSims = 1;

        public Game1(int gridSizeX, int gridSizeY, int screenHeight, int screenWidth, bool logging, int lower, int mid, int high)
        {
            _gridSizeX = gridSizeX;
            _gridSizeY = gridSizeY;

            _screenHeight = screenHeight;
            _screenWidth = screenWidth;

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Logging
            IsLogging = logging;
            lowTest = 40;
            midTest = 80;
            highTest = 120;
            _logger = new Logger(lowTest, midTest, highTest);

        }

        protected override void Initialize()
        {
            initSize();
            // TODO: Add your initialization logic here

            initGrid();


            LoadContent();
            base.Initialize();
        }


        protected void initGrid()
        {
            _grid = new SquareGrid(new Vector2(0, 0), GraphicsDevice, _screenWidth, _screenHeight, _gridSizeX, _gridSizeY, IsLogging);


        }

        protected void initSize()
        {
            GraphicsDevice.PresentationParameters.BackBufferWidth = _screenWidth;
            GraphicsDevice.PresentationParameters.BackBufferHeight = _screenHeight;

        }
        //currently unused in SquareGrid class, even though we pass it as argument
        Texture2D rectTexture;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            rectTexture = new Texture2D(GraphicsDevice, 1, 1);
            rectTexture.SetData(new[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            _spriteBatch.Dispose();
            // If you are creating your texture (instead of loading it with
            // Content.Load) then you must Dispose of it
            rectTexture.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            HandleKeyInput();

            _grid.Update();
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);


            _grid.drawGrid(new Vector2(0, 0), _spriteBatch, rectTexture);

            base.Draw(gameTime);

        }


        public bool isSquareOccupied(Vector2 position)
        {

            var gLType = _grid._slots[(int)position.X][(int)position.Y].GetType();
            return gLType == typeof(Agent)
                    || gLType == typeof(Tree)
                    || gLType == typeof(Obstacle);
        }

        private void HandleKeyInput()
        {

            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            var position = _grid.getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
            var posX = position.X;
            var posY = position.Y;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || _currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //debugging purposes, just wanted to know if we could get the correct slot, which we do. this could potentially be used to place things?
            if (_currentKeyboardState.IsKeyDown(Keys.Tab))
            {
                Trace.WriteLine(_grid.getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y)));
            }
            // Below is for adding and removing stuff
            if (_currentKeyboardState.IsKeyDown(Keys.LeftControl) || _currentKeyboardState.IsKeyDown(Keys.RightControl))
            {
                if (_currentKeyboardState.IsKeyDown(Keys.Space))
                {

                    handleSpacebar();
                }

                if (_currentKeyboardState.IsKeyDown(Keys.T))
                {
                    if (isSquareOccupied(position)) return;
                    _grid.addTree(position);
                }

                if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    if (isSquareOccupied(position)) return;
                    else _grid.addAgent(position);

                }

                if (_currentKeyboardState.IsKeyDown(Keys.R))
                {
                    _grid._slots[(int)posX][(int)posY] = new Obstacle(position);

                }

                if (_currentKeyboardState.IsKeyDown(Keys.B))
                {
                    if (isSquareOccupied(position)) return;
                    _grid.addTree(position, 80);
                }

                //used for clearing
                if (_currentKeyboardState.IsKeyDown(Keys.Back))
                {
                    _grid._slots[(int)posX][(int)posY] = new Boardentity(1, true, position);
                }

                if (_currentKeyboardState.IsKeyDown(Keys.LeftShift) && _currentKeyboardState.IsKeyDown(Keys.R))
                {
                    initGrid();
                }

                if (_currentKeyboardState.IsKeyDown(Keys.LeftShift) && _currentKeyboardState.IsKeyDown(Keys.S))
                {
                    Console.WriteLine("writing to xml");
                    writeXML(_grid);
                }
                if (_currentKeyboardState.IsKeyDown(Keys.LeftShift) && _currentKeyboardState.IsKeyDown(Keys.L))
                {
                    Console.WriteLine("reading from xml");
                    var newgrid = readXML();
                    _grid = newgrid;

                }
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
            {

                handleSpacebar();
            }
        }

        public void handleSpacebar()
        {
            if (IsLogging)
            {
                for (int x = 0; x < totalSims; x++)
                {
                    for (int i = 0; i < lowTest; i++)
                    {
                        _grid = _grid.TickOnce();

                    }

                    _logger.logLocations(lowTest, _grid);

                    for (int i = lowTest; i < midTest; i++)
                    {
                        _grid = _grid.TickOnce();

                    }

                    _logger.logLocations(midTest, _grid);


                    for (int i = midTest; i < highTest; i++)
                    {
                        _grid = _grid.TickOnce();


                    }
                    _logger.logLocations(highTest, _grid);
                    Initialize();
                }
                _logger.serialize();
            }
            else
            {
                _grid = _grid.TickOnce();
                _tickCounter++;
            }
        }
        public void writeXML(SquareGrid grid)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SquareGrid));

            StreamWriter writer = new StreamWriter("testGrid.xml");
            serializer.Serialize(writer, grid);
            writer.Close();
        }

        public SquareGrid readXML()
        {

            var mySerializer = new XmlSerializer(typeof(SquareGrid));
            using var myFileStream = new FileStream("testGrid.xml", FileMode.Open);

            var grid = (SquareGrid)mySerializer.Deserialize(myFileStream);

            return grid;
        }


    }

}
