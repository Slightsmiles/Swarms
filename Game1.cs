using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
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

        private SquareGrid _grid;

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

            //change the offset here

            _grid.drawGrid(new Vector2(0, 0), _spriteBatch, rectTexture);

            base.Draw(gameTime);

            //TODO, look into spritebatch and drawing, i think the grid data structure is bueno, we just need to be able to draw it.
        }


        public bool isSquareOccupied(Vector2 position) {

            var gLType = _grid.slots[(int)position.X][(int)position.Y].GetType();
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
                    _grid.slots[(int)posX][(int)posY] = new Tree(position);
                }

                if (_currentKeyboardState.IsKeyDown(Keys.A))
                {
                    if(isSquareOccupied(position)) return;
                    else _grid.addAgent(position);
                    
                }

                if (_currentKeyboardState.IsKeyDown(Keys.R))
                {
                    _grid.slots[(int)posX][(int)posY] = new Obstacle(position);

                }

                //used for clearing
                if (_currentKeyboardState.IsKeyDown(Keys.Space))
                {
                    _grid.slots[(int)posX][(int)posY] = new Boardentity(1, true, position);
                }

                if (_currentKeyboardState.IsKeyDown(Keys.LeftShift) && _currentKeyboardState.IsKeyDown(Keys.R))
                {
                    initGrid();
                }
            }

            if(_currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space)) {
                foreach (var agent in _grid._agentList)
                {
                    agent.autoMove(_grid);
                }
            }
        }

    }
}
