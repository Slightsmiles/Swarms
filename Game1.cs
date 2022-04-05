using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
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
        public int _screenWidth {get; private set;}
        public int _screenHeight {get; private set;}

        public SquareGrid _grid;

        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _screenHeight = DEFAULT_SCREEN_HEIGHT;
            _screenWidth = DEFAULT_SCREEN_WIDTH;

            // TODO: Add your initialization logic here
            initSize();
            initGrid();
            
            LoadContent();
            base.Initialize();
        }

        protected void initGrid()
        {
            _grid = new SquareGrid(new Vector2(0, 0), GraphicsDevice, _screenWidth, _screenHeight);

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


        public bool isSquareOccupied(Vector2 position) {

            var gLType = _grid._slots[(int)position.X][(int)position.Y].GetType();
            return     gLType == typeof(Agent)
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
            if (_currentKeyboardState.IsKeyDown(Keys.LeftControl))
            {

                if (_currentKeyboardState.IsKeyDown(Keys.T))
                {
                    _grid.addTree(position);
                    //_grid._slots[(int)posX][(int)posY] = _grid.addTree(new Tree(position));
                }

                if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    if(isSquareOccupied(position)) return;
                    else _grid.addAgent(position);
                    
                }

                if (_currentKeyboardState.IsKeyDown(Keys.R))
                {
                    _grid._slots[(int)posX][(int)posY] = new Obstacle(position);

                }

                //used for clearing
                if (_currentKeyboardState.IsKeyDown(Keys.Space))
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
                    writeXML2(_grid);
                }
                if (_currentKeyboardState.IsKeyDown(Keys.LeftShift) && _currentKeyboardState.IsKeyDown(Keys.L))
                {
                    Console.WriteLine("reading from xml");
                    var newgrid = readXML2();
                    _grid = newgrid;

                }
            }

            if(_currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
            {
                
                    _grid = _grid.TickOnce();


            } 
        }
/*
        private SquareGrid readXML()
        {
            XmlSerializer reader = new XmlSerializer(typeof(SquareGrid));
            StreamReader file = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//someGrid.xml");
            var _slots = (SquareGrid) reader.Deserialize(file);
            
            return _slots;
        }

        public void writeXML(SquareGrid grid)
        {
            
            System.Xml.Serialization.XmlSerializer writer =  new System.Xml.Serialization.XmlSerializer(typeof(SquareGrid));

            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//someGrid.xml";
            StreamWriter file = System.IO.File.Create(path);
            
            writer.Serialize(file, grid);
            file.Close();
        }

*/
        public void writeXML2(SquareGrid grid)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SquareGrid));

            StreamWriter writer = new StreamWriter("testGrid.xml");
            serializer.Serialize(writer, grid);
            writer.Close();
        }

        public SquareGrid readXML2()
        {
            var mySerializer = new XmlSerializer(typeof(SquareGrid));
            using var myFileStream = new FileStream("testGrid.xml", FileMode.Open);

            var grid = (SquareGrid) mySerializer.Deserialize(myFileStream);

            return grid;
        }
    }
    
}
