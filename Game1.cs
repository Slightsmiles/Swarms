using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Swarms
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        private int _screenWidth;
        private int _screenHeight;
        private int _rows;
        private int _columns;
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
 
            base.Initialize();
        }
        protected void initGrid(){
            _columns = 10;
            _rows = 10;
        }
        protected void initSize(){
            _screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            _screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
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
            blackRectangle.Dispose();
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

            _spriteBatch.Begin();
               // TODO: Add your drawing code here
            for (int i = 0; i < _columns; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    _spriteBatch.Draw(blackRectangle, new Rectangle(i * 2, j * 2, 1, 1), Color.Black);
                }
            }
            _spriteBatch.End();
        }
        
    }
}
