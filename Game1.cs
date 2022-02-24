using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Swarms.Datatypes.Grids;

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
            initGrid();
            initSize();
            LoadContent();
            base.Initialize();
        }

        public int getWidth(){
            return _screenWidth;
        }
        public int getHeight(){
            return _screenHeight;
        }
        protected void initGrid(){
            _columns = 5;
            _rows = 5;
            _gridSize = 10;
        }
        protected void initSize(){
            _screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            Trace.WriteLine(_screenWidth);
            _screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            Trace.WriteLine(_screenHeight);
            
        }
        Texture2D blackRectangle;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            blackRectangle = new Texture2D(GraphicsDevice, 1, 1);
            blackRectangle.SetData(new Color[] { Color.Black });
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            _spriteBatch.Dispose();
    // If you are creating your texture (instead of loading it with
    // Content.Load) then you must Dispose of it
           // blackRectangle.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
        

            base.Draw(gameTime);

            //TODO, look into spritebatch and drawing, i think the grid data structure is bueno, we just need to be able to draw it.
        }
        
    }
}
