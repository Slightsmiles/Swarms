using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Swarms.Datatypes.Grids;
using Swarms.entities;

namespace Swarms
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int _gridSize;
        private int _screenWidth;
        private int _screenHeight;
        private int _rows;
        private int _columns;

        private SquareGrid _grid;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            initSize();
            initGrid();
            LoadContent();
            base.Initialize();
        }

        public int getWidth()
        {
            return _screenWidth;
        }
        public int getHeight()
        {
            return _screenHeight;
        }
        protected void initGrid()
        {
            _grid = new SquareGrid(new Vector2(25, 25), new Vector2(0, 0), new Vector2(_screenWidth, _screenHeight), GraphicsDevice);
        }
        protected void initSize()
        {
            _screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            _screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

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

            _grid.Update(Vector2.Zero);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //change the offset here

            _grid.drawGrid(new Vector2(0, 0), _spriteBatch, rectTexture);

            base.Draw(gameTime);

            //TODO, look into spritebatch and drawing, i think the grid data structure is bueno, we just need to be able to draw it.
        }



        private void HandleKeyInput()
        {

            var state = Keyboard.GetState();
            var position = _grid.getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), -Vector2.Zero);
            var posX = position.X;
            var posY = position.Y;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape))
                Exit();

            //debugging purposes, just wanted to know if we could get the correct slot, which we do. this could potentially be used to place things?
            if (state.IsKeyDown(Keys.Tab))
            {
                Trace.WriteLine(_grid.getSlotFromPixel(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), -Vector2.Zero));
            }
            // Below is for adding and removing stuff
            if (state.IsKeyDown(Keys.LeftControl))
            {

                if (state.IsKeyDown(Keys.T))
                {
                    _grid.slots[(int)posX][(int)posY] = new Tree(position);
                }

                if (state.IsKeyDown(Keys.A))
                {
                    _grid.slots[(int)posX][(int)posY] = new Agent(position);

                }

                if (state.IsKeyDown(Keys.R))
                {
                    _grid.slots[(int)posX][(int)posY] = new Obstacle(position);

                }

                //used for clearing
                if (state.IsKeyDown(Keys.Space))
                {
                    _grid.slots[(int)posX][(int)posY] = new Boardentity();
                }

                if (state.IsKeyDown(Keys.LeftShift) && state.IsKeyDown(Keys.R))
                {
                    initGrid();
                }

            }
        }

    }
}
