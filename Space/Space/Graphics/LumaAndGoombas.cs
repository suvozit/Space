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
    class LumaAndGoombas
    {
        const int maxGoombas = 4;
        const int maxLumas = 4;
        public Image[] goombas, lumas;

        public int hitLuma = 0;
        public int numberLumba = 0;

        float maxHeight = 0.6f, minHeight = 0.9f;
        float maxVelocity = .5f, minVelocity = 5;
        Random random = new Random();

        public LumaAndGoombas()
        {
            goombas = new Image[maxGoombas];
            lumas = new Image[maxLumas];
        }

        public void LoadContent(SpriteBatch spriteBatch, Texture2D texture)
        {
            for (int i = 0; i < maxGoombas; i++)
            {
                goombas[i] = new Image(spriteBatch, texture);
                goombas[i].scale = new Vector2(0.4f); goombas[i].origin = goombas[i].center;
            }

            for (int i = 0; i < maxLumas; i++)
            {
                lumas[i].scale = new Vector2(0.5f); lumas[i].origin = lumas[i].center;
            }
        }

        public void Update(GameTime gameTime, Rectangle viewPortRect)
        {
            #region Goomba Update
            float goombaHeight = goombas[0].source.Height * goombas[0].scale.X * 0.5f;
            float goombaWidth = goombas[0].source.Width * goombas[0].scale.Y * 0.5f;

            float lumaHeight = lumas[0].source.Height * lumas[0].scale.X * .8f;
            float lumaWidth = lumas[0].source.Width * lumas[0].scale.Y * .8f;

            foreach (Image goomba in goombas)
            {
                if (goomba.alive)
                {
                    goomba.position += goomba.velocity;
                    if (!viewPortRect.Contains(new Point(
                        (int)goomba.position.X,
                        (int)goomba.position.Y)))
                    {
                        goomba.alive = false;
                        continue;
                    }

                    Rectangle goombaRect = new Rectangle(
                            (int)(goomba.position.X - (goombaWidth / 2)),
                            (int)(goomba.position.Y - (goombaHeight / 2)), (int)goombaWidth, (int)goombaHeight);

                    foreach (Image luma in lumas)
                    {
                        Rectangle lumaRect = new Rectangle(
                            (int)(luma.position.X - (lumaWidth / 2)),
                            (int)(luma.position.Y - (lumaHeight / 2)), (int)lumaWidth, (int)lumaHeight);

                        if (luma.alive && lumaRect.Intersects(goombaRect))
                        {
                            luma.alive = goomba.alive = false; hitLuma += 1;
                            break;
                        }
                    }
                }
            }
            #endregion

            #region Luma Update
            for (int i = 0; i < maxLumas; i++)
            {
                Image luma = lumas[i];
                if (luma.alive)
                {
                    luma.position += i % 2 == 0 ? luma.velocity : -luma.velocity;
                    if (!viewPortRect.Contains(new Point(
                        (int)luma.position.X, (int)luma.position.Y)))
                    {
                        luma.alive = false;
                        continue;
                    }
                }
                else
                {
                    if (gameTime.TotalRealTime.TotalSeconds > 35) continue;
                    luma.alive = true;
                    numberLumba += 1;
                    luma.position = new Vector2(
                        i % 2 == 0 ? viewPortRect.Left : viewPortRect.Right, MathHelper.Lerp(
                        maxHeight * (float)viewPortRect.Height,
                        minHeight * (float)viewPortRect.Height,
                        (float)random.NextDouble()));
                    luma.velocity = new Vector2(MathHelper.Lerp(
                        (float)maxVelocity,
                        (float)minVelocity,
                        (float)random.NextDouble()), 0);
                }
            }
            #endregion
        }

        public void Fire(Vector2 billPosition, float billRotation)
        {
            foreach (Image goomba in goombas)
            {
                if (!goomba.alive)
                {
                    goomba.alive = true;
                    goomba.position = billPosition;
                    goomba.rotation = billRotation - MathHelper.PiOver2;
                    goomba.velocity = new Vector2(
                        (float)Math.Cos(billRotation), (float)Math.Sign(billRotation)) * 6;
                    break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Image goomba in goombas)
                if (goomba.alive) goomba.Draw(gameTime);

            foreach (Image luma in lumas)
                if (luma.alive) luma.Draw(gameTime);
        }
    }
}
