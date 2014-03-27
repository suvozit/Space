using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Space.Graphics
{
    class Image
    {
        Texture2D texture;
        SpriteBatch spriteBatch;

        public float rotation, layer;
        public Vector2 scale, position, origin, center;
        public Color colour;
        public SpriteEffects effects;
        public Rectangle source;

        public bool alive;
        public Vector2 velocity;

        public Image(SpriteBatch spriteBatch, Texture2D texture)
        {
            rotation = layer = 0f;
            scale = Vector2.One;
            velocity = position = origin = Vector2.Zero;
            colour = Color.White;
            effects = SpriteEffects.None;
            alive = false;

            this.spriteBatch = spriteBatch;
            this.texture = texture;

            source = new Rectangle(0, 0, texture.Width, texture.Height);
            center = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, source, colour, rotation, origin, scale, effects, layer);
        }
    }
}
