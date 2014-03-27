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
using Space.Graphics;

namespace Space
{
    public class Engine : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        Texture2D texture;
        Rectangle viewPortRect;

        Image backGround, bill, fakeBill;
        LumaAndGoombas lng;

        KeyboardState prevKeyboardState = Keyboard.GetState();
        MouseState prevMouseState = Mouse.GetState();

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            lng = new LumaAndGoombas();

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;            
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            viewPortRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            backGround = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\walk[1]"));

            bill = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\Bullet Bill"));
            fakeBill = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\Bullet Bill"));

            bill.position = new Vector2(viewPortRect.Width / 2, 100); bill.origin = bill.center; bill.scale = new Vector2(0.7f);

            lng.lumas[0] = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\Luma - Blue"));
            lng.lumas[1] = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\Luma - Green"));
            lng.lumas[2] = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\Luma - Red"));
            lng.lumas[3] = new Image(spriteBatch, Content.Load<Texture2D>(@"Texture\Luma - Yellow"));

            lng.LoadContent(spriteBatch, Content.Load<Texture2D>(@"Texture\Goomba"));

            texture = Content.Load<Texture2D>(@"Texture\Goomba");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.L) || keyboardState.IsKeyDown(Keys.Right))
                bill.rotation -= 0.06f;

            else if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                bill.rotation += 0.06f;

            else if (keyboardState.IsKeyDown(Keys.Space))
                lng.Fire(bill.position, bill.rotation);

            else if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            bill.rotation = MathHelper.Clamp(bill.rotation, 0.523f, 2.617f);

            if (gameTime.TotalRealTime.TotalSeconds > 12 && gameTime.TotalRealTime.TotalSeconds < 40)
                lng.Update(gameTime, viewPortRect);

            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            int rate = 0;
            if (lng.numberLumba > 0) rate = (lng.hitLuma * 100) / (lng.numberLumba);

            #region Start Screen 10 sec
            if (gameTime.TotalRealTime.TotalSeconds < 10)
            {
                backGround.Draw(gameTime);

                spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\writeFont"),
                    "Be ready the game will start in ",
                    new Vector2(50, 100), Color.White);

                spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\scoreFont"),
                    (9 - (int)gameTime.TotalRealTime.TotalSeconds).ToString(),
                    new Vector2(100, 200), Color.WhiteSmoke);
            }
            #endregion

            #region Play Screen 30 sec
            else if (gameTime.TotalRealTime.TotalSeconds < 40)
            {
                backGround.Draw(gameTime);

                spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\writeFont"),
                    "Score : " + rate,
                    new Vector2(10), Color.SteelBlue);

                lng.Draw(gameTime);

                if (bill.rotation > 1.57f && !bill.alive)   // false <= 90 deg  &   true > 90 deg
                {
                    bill.effects = SpriteEffects.FlipVertically;
                    bill.alive = true;
                }
                else if (bill.rotation <= 1.57f && bill.alive)
                {
                    bill.effects = SpriteEffects.None;
                    bill.alive = false;
                }
                bill.Draw(gameTime);
            }
            #endregion

            #region End Screen
            else
            {
                backGround.Draw(gameTime);
                fakeBill.position = new Vector2(470, 50);
                fakeBill.Draw(gameTime);

                spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\writeFont"),
                    "Game Over =) \nYour Score Rate:",
                    new Vector2(50, 100), Color.White);

                spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\scoreFont"),
                    rate.ToString(),
                    new Vector2(100, 200), Color.WhiteSmoke);

                spriteBatch.DrawString(Content.Load<SpriteFont>(@"Fonts\writeFont"),
                    "Hit Esc to exit",
                    new Vector2(230, 520), Color.White);
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
