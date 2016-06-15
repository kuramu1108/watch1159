using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace Watch1159
{
    class cButton
    {
		GraphicsDevice device;
		SpriteBatch spriteBatch;
		public Texture2D texture;
        public Vector2 position;

        // Location and size of the button
        public Rectangle area;

        // Gamma is working to change the button color 04.05.16 Dongyeop
        public Color colour = new Color(255, 255, 255, 255);

		public cButton(GraphicsDevice device, SpriteBatch spriteBatch, Texture2D texture, Rectangle area)
        {
			this.device = device;
			this.spriteBatch = spriteBatch;
            this.texture = texture;
			this.area = area;
        }

		public Boolean isPressed(Rectangle touchPoint) {
			if (touchPoint.Intersects (area))
				return true;
			else
				return false;
		}

        public void Draw () // Because of Rectangle, can not draw so, get the rectangle location as parameter from Game1.cs 04.05.16 Dongyeop
        {
            spriteBatch.Draw(texture, area, colour);
        }

        // Change Gamma to display like on /off on button // Dongyeop
        public void TurnOn()
        {
            colour.A = 240;
        }

        public void TurnOff()
        {
            colour.A = 255;
        }

    }
}