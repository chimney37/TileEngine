using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTile
{
    public class GameMapEditor: GameCore
    {
        int id;

        int TileHoriIdx = 0;
        int TileVertIdx = 0;
        int TileIdx = 0;
        int OldTileIdx = 0;



        bool ShowTileMap = false;
        float currentTime = 0f;
        float maxTime = 5f;
        float Alpha = 1.0f;
        int TileMapSX = 666;
        int TileMapSY = 200;
        float TileMapScale = 0.5f;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Context context)
        {
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            if (ks.IsKeyUp(Keys.E) && oldState.IsKeyDown(Keys.E))
                context.changeState(typeof(GameCore));

            if (ks.IsKeyUp(Keys.V) && oldState.IsKeyDown(Keys.V))
            {
                this.Remove(this.id);
                GameText gametxt = context.getFactory().GameText("Tile Map Editor Ver 0.01", 400, 400);
                this.Add(gametxt, gametxt.ID);
                id = gametxt.ID;
                Console.WriteLine(string.Format("Process queued: ID={0}", gametxt.ID));
            }

            #region UPDATE TILE INDEX
            OldTileIdx = TileIdx;

            if (ks.IsKeyUp(Keys.A) && oldState.IsKeyDown(Keys.A))
                TileHoriIdx = (TileHoriIdx - 1) % Tile.MaxTileHorizontalIndex;

            if (ks.IsKeyUp(Keys.W) && oldState.IsKeyDown(Keys.W))
                TileVertIdx = (TileVertIdx - 1) % Tile.MaxTileVerticalIndex;

            if (ks.IsKeyUp(Keys.S) && oldState.IsKeyDown(Keys.S))
                TileVertIdx = (TileVertIdx + 1) % Tile.MaxTileVerticalIndex;

            if (ks.IsKeyUp(Keys.D) && oldState.IsKeyDown(Keys.D))
                TileHoriIdx = (TileHoriIdx + 1) % Tile.MaxTileHorizontalIndex;

            TileIdx = TileHoriIdx + TileVertIdx * Tile.MaxTileHorizontalIndex;

            //Show Tile Map for 1 seconds
            if (OldTileIdx != TileIdx)
            {
                ShowTileMap = true;
                currentTime = 0;
            }

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            Alpha = ((currentTime <= maxTime - 1f) ? 1.0f : -currentTime + maxTime);

            if (currentTime >= maxTime)
            {
                ShowTileMap = false;
                currentTime = 0f;
            }

            #endregion

            #region TILES ADDITION AND REMOVAL OPERATIONS WITH MOUSE CLICKS
            if (oldMouseState.LeftButton == ButtonState.Pressed &&
                ms.LeftButton == ButtonState.Released)
            {
                myMap.AddBaseTile(this.hilightPoint.X, this.hilightPoint.Y, TileIdx);
            }

            if (oldMouseState.RightButton == ButtonState.Pressed &&
                ms.LeftButton == ButtonState.Released)
            {
                myMap.RemoveBaseTile(this.hilightPoint.X, this.hilightPoint.Y);
            }
            #endregion

            base.UpdateCameraFirstSquare();
            base.UpdateHilight();
            base.MapMouseScroll();

            oldState = ks;
            oldMouseState = ms;



            //base.Update(gameTime, context);
        }

        public override void Render(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Context context)
        {
            base.Render(gameTime, spriteBatch, context);


            //Set up sprite batch. Tells XNA/Monogame that we are going to specify layer depth (sorted from back(1.0f) to front(0.0f))
            spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.LinearWrap,
                null,
                null,
                null,
                this.camera.GetTransformation());

            #region DRAW TILE PART (FROM MOUSE)

            //get hilight row offset
            int hilightrowOffset = ((this.hilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

            spriteBatch.Draw(
                Tile.TileSetTexture,
                    new Vector2(
                        hilightPoint.X * Tile.TileStepX + hilightrowOffset,
                        hilightPoint.Y * Tile.TileStepY),
                Tile.GetSourceRectangle(TileIdx),
                Color.Aquamarine * 0.8f,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            #endregion

            #region DRAW TILE SET MAP
            if (ShowTileMap)
            {
                spriteBatch.Draw(
                    Tile.TileSetTexture,
                        camera.ScreenToWorld(new Vector2(
                            TileMapSX,
                            TileMapSY)),
                    Tile.GetSourceTileSet(),
                    Color.Aquamarine * Alpha ,
                    0.0f,
                    Vector2.Zero,
                    TileMapScale,
                    SpriteEffects.None,
                    0.0f);

                spriteBatch.Draw(
                    Tile.TileSetTexture,
                        camera.ScreenToWorld(new Vector2(
                            TileMapSX + Tile.TileWidth*TileHoriIdx*TileMapScale,
                            TileMapSY + Tile.TileHeight*TileVertIdx*TileMapScale)),
                    Tile.GetSourceRectangle(74),
                    Color.Black,
                    0.0f,
                    Vector2.Zero,
                    TileMapScale,
                    SpriteEffects.None,
                    0.0f);
            }
            #endregion

            spriteBatch.End();
        }
    }
}
