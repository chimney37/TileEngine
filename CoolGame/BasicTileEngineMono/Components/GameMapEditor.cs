using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using BasicTileEngineMono.Components;

namespace BasicTileEngineMono
{
    public class GameMapEditor: GameCore
    {
        GameInput mapEditorInput;

        int TypeIdx = 0;
        public int TileTypeIndex
        {
            get { return TypeIdx; }
            set { TypeIdx = value; }
        }
        TileType tileType;
        public TileType CurrentTileType
        {
            get { return tileType; }
            set { tileType = value; }
        }

        Texture2D blankTexture;

        bool ShowPreview = true;
        public bool ShowPreviewMode
        {
            get { return ShowPreview; }
            set { ShowPreview = value; }
        }

        bool ShowTileMap = false;
        float currentTime = 0f;
        float maxTime = 10f;
        float Alpha = 1.0f;
        int TileMapSX = 666;
        int TileMapSY = 200;
        float TileMapScale = 0.5f;

        public bool IsConfigLoaded { get; set; }

        public override void Initialize(Game game)
        {
            base.Initialize(game);

            IsConfigLoaded = false;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDeviceManager graphics)
        {
            base.LoadContent(Content, graphics);

            blankTexture = new Texture2D(graphics.GraphicsDevice, Tile.TileWidth, Tile.TileHeight, false, SurfaceFormat.Color);

            Color[] color = new Color[Tile.TileWidth * Tile.TileHeight];
            for (int i = 0; i < color.Length; i++)
                color[i] = Color.White;

            blankTexture.SetData(color);


            //rebind input
            mapEditorInput = new GameInput();
            mapEditorInput._buttonE_PR = new StateChangeToCommand<GameCore>(this);
            mapEditorInput._buttonR_PR = new LoadConfigFileCommand();
            mapEditorInput._buttonA_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Left);
            mapEditorInput._buttonD_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Right);
            mapEditorInput._buttonW_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Up);
            mapEditorInput._buttonS_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Down);
            mapEditorInput._buttonQ_PR = new IncrementTileTypeIndexCmd();
            mapEditorInput._mouseLeft_P_NoLShf = new AddTileToMapCmd();
            mapEditorInput._mouseLeft_P_LShf = new MoveGameActorToPositionCommand(this.Player);
            mapEditorInput._buttonLeftShift_P = new SetPreviewCursor(false);
            mapEditorInput._mouseRight_PR = new RemoveTileMapCmd();
        }

        public override void Update(GameTime gameTime, IContext context)
        {
            this.GameTime = gameTime;
            this.ShowPreviewMode = true;

            this.mapEditorInput.HandleInput(ref CommandQueue);
            while (CommandQueue.Count() > 0)
            {
                Command cmd = CommandQueue.Dequeue();

                if (cmd != null)
                {
                    cmd.Execute(_camera);
                    cmd.Execute(this);
                    cmd.Execute(context);
                }
            }

            if (!IsConfigLoaded)
            {
                myMap.RegisterConfigurationFile();
                IsConfigLoaded = true;
            }

            UpdateMiniTileMap(gameTime);

            base.UpdateActors(gameTime);
            base.gameInput.HandleInput();
            base.UpdateCameraFirstSquare();
            base.UpdateHilight();
            base.UpdateMapMouseScroll();

            //string text = base.InformationalTxt;
            base.InformationalTxt = string.Format("Tile Map Editor Ver 0.01\nTile Type: ({0}), Index={1}, ObjectName={2}", tileType.ToString(), myMap.TileIndex, myMap.GetTileMapLogicalObjName(myMap.TileIndex));

            //base.Update(gameTime, context);
        }

        private void UpdateMiniTileMap(GameTime gameTime)
        {
            //fix tile type if required given the index
            TileType t = myMap.GetGameTileInfoList(myMap.TileIndex).ElementAt(0).TileType;

            switch (t)
            {
                case TileType.Multi:
                case TileType.Height:
                case TileType.Topper:
                    tileType = t;
                    break;
            }
            //Show Tile Map for 1 seconds
            //
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
        }

        public override void Render(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, IContext context)
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
                this._camera.GetTransformation());

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
                        _camera.ScreenToWorld(new Vector2(
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
                            _camera.ScreenToWorld(new Vector2(
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
    }
}
