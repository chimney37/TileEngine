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

        int TypeIdx = 0;
        TileType tileType;

        Texture2D blankTexture;

        bool ShowPreview = true;
        bool ShowTileMap = false;
        float currentTime = 0f;
        float maxTime = 10f;
        float Alpha = 1.0f;
        int TileMapSX = 666;
        int TileMapSY = 200;
        float TileMapScale = 0.5f;

        bool IsConfigLoaded = false;

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDeviceManager graphics)
        {
            base.LoadContent(Content, graphics);

            blankTexture = new Texture2D(graphics.GraphicsDevice, Tile.TileWidth, Tile.TileHeight, false, SurfaceFormat.Color);

            Color[] color = new Color[Tile.TileWidth * Tile.TileHeight];
            for (int i = 0; i < color.Length; i++)
                color[i] = Color.White;

            blankTexture.SetData(color);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, Context context)
        {
            if (!IsConfigLoaded)
            {
                myMap.RegisterConfigurationFile();
                IsConfigLoaded = true;
            }

            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            if (ks.IsKeyUp(Keys.E) && oldState.IsKeyDown(Keys.E))
            {
                IsConfigLoaded = false;
                context.changeState(typeof(GameCore));
            }

            if (ks.IsKeyUp(Keys.V) && oldState.IsKeyDown(Keys.V))
            {
                this.Remove(this.id);
                GameText gametxt = context.getFactory().GameText("Tile Map Editor Ver 0.01", 400, 400);
                this.Add(gametxt, gametxt.ID);
                id = gametxt.ID;
                Console.WriteLine(string.Format("Process queued: ID={0}", gametxt.ID));
            }

            #region UPDATE TILE INDEX
            myMap.OldTileIdx = myMap.TileIndex;

            if (ks.IsKeyUp(Keys.A) && oldState.IsKeyDown(Keys.A))
                myMap.GetLeftTileIndex();

            if (ks.IsKeyUp(Keys.W) && oldState.IsKeyDown(Keys.W))
                myMap.GetUpTileIndex();

            if (ks.IsKeyUp(Keys.S) && oldState.IsKeyDown(Keys.S))
                myMap.GetDownTileIndex();

            if (ks.IsKeyUp(Keys.D) && oldState.IsKeyDown(Keys.D))
                myMap.GetRightTileIndex();

            if(ks.IsKeyUp(Keys.Q) && oldState.IsKeyDown(Keys.Q))
                tileType = (TileType)(++TypeIdx % Enum.GetNames(typeof(TileType)).Length);
            
            //fix tile type if required given the index
            TileType t = myMap.GetGameTileInfoList(myMap.TileIndex).ElementAt(0).TileType;
            if (t == TileType.Multi)
                tileType = t;

            //Show Tile Map for 1 seconds
            if (myMap.OldTileIdx != myMap.TileIndex)
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
            if (ks.IsKeyUp(Keys.LeftShift) && 
                oldMouseState.LeftButton == ButtonState.Pressed &&
                ms.LeftButton == ButtonState.Released)
            {
                switch (tileType)
                {
                    case TileType.Base:
                        myMap.AddBaseTile(this.hilightPoint.X, this.hilightPoint.Y, myMap.TileIndex);
                        break;
                    case TileType.Height:
                        myMap.AddHeightTile(this.hilightPoint.X, this.hilightPoint.Y, myMap.TileIndex);
                        break;
                    case TileType.Topper:
                        myMap.AddTopperTile(this.hilightPoint.X, this.hilightPoint.Y, myMap.TileIndex);
                        break;
                    case TileType.Multi:
                        myMap.AddMultiTile(this.hilightPoint.X, this.hilightPoint.Y, myMap.TileIndex);
                        break;
                    default:
                        break;
                }
            }

            if (ks.IsKeyUp(Keys.LeftShift) && 
                oldMouseState.RightButton == ButtonState.Pressed &&
                ms.RightButton == ButtonState.Released)
            {
                switch (tileType)
                {
                    case TileType.Base:
                        myMap.RemoveBaseTile(this.hilightPoint.X, this.hilightPoint.Y);
                        break;
                    case TileType.Height:
                        myMap.RemoveHeightTile(this.hilightPoint.X, this.hilightPoint.Y);
                        break;
                    case TileType.Topper:
                        myMap.RemoveTopperTile(this.hilightPoint.X, this.hilightPoint.Y);
                        break;
                    case TileType.Multi:
                        myMap.RemoveMultiTile(this.hilightPoint.X, this.hilightPoint.Y);
                        break;
                    default:
                        break;
                }
            }
            #endregion

            if (ks.IsKeyDown(Keys.LeftShift))
            {
                ShowPreview = false;
                base.UpdatePlayerByMouse(gameTime);
            }

            if (ks.IsKeyUp(Keys.LeftShift))
                ShowPreview = true;




            base.UpdateCameraFirstSquare();
            base.UpdateHilight();
            base.MapMouseScroll();

            oldState = ks;
            oldMouseState = ms;

            //string text = base.InformationalTxt;
            base.InformationalTxt = string.Format("\nTile Type: ({0}), Index={1}, ObjectName={2}", tileType.ToString(), myMap.TileIndex, myMap.GetTileMapLogicalObjName(myMap.TileIndex));

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

            if (ShowPreview)
            {
                int hilightrowOffset = ((this.hilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

                foreach (GameTileInfo t in myMap.GetGameTileInfoList(myMap.TileIndex))
                {
                    spriteBatch.Draw(
                        Tile.TileSetTexture,
                            new Vector2(
                                hilightPoint.X * Tile.TileStepX + hilightrowOffset - t.TileXOffset*Tile.MultiSizeTileOffset,
                                hilightPoint.Y * Tile.TileStepY - t.TileYOffset * Tile.MultiSizeTileOffset - myMap.GetOverallCenterHeight(hilightPoint.Y, hilightPoint.X) ),
                        Tile.GetSourceRectangle(t.TileID),
                        Color.Aquamarine * 0.8f,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0.0f);
                }
            }
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
                    0.1f);

                foreach (int id in myMap.GetTileMapHilightIndexes(myMap.TileIndex))
                {
                    spriteBatch.Draw(
                        blankTexture,
                            camera.ScreenToWorld(new Vector2(
                                TileMapSX + Tile.TileWidth * (id % myMap.MaxTileHorizontalIndex) * TileMapScale,
                                TileMapSY + Tile.TileHeight * (id / myMap.MaxTileHorizontalIndex) * TileMapScale)),
                        new Rectangle(0, 0, blankTexture.Width, blankTexture.Height),
                        Color.White * Alpha * 0.2f,
                        0.0f,
                        Vector2.Zero,
                        TileMapScale,
                        SpriteEffects.None,
                        0.0f);
                }
            }
            #endregion

            spriteBatch.End();
        }

        public enum TileType
        {
            Base,
            Height,
            Topper,
            Multi
        }
    }
}
