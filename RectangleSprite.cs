

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
class RectangleSprite
{
    //Remember copy pasted from StackOverflow SOMEWHERE!!!
    //TODO I JUST MAKE IT ORANGE ALSO SO WE SEE
    static Texture2D _pointTexture;
    public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
    {
        if (_pointTexture == null)
        {
            _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _pointTexture.SetData<Color>(new Color[]{Color.White});
        }

        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
        spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
    }     

    public static void FillRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color){

         var _fillTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        

            _fillTexture.SetData<Color>(new Color[]{color});

            spriteBatch.Draw(_fillTexture, rect, color);
        }

        

    
}