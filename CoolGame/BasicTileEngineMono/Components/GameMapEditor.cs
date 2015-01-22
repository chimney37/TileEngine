using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BasicTileEngineMono.Components
{
    public class GameMapEditor: GameCore
    {
        GameInput _mapEditorInput;

        public int TileTypeIndex { get; set; }

        public TileType CurrentTileType { get; set; }

        Texture2D _blankTexture;

        bool _showPreview = true;
        public bool ShowPreviewMode
        {
            get { return _showPreview; }
            set { _showPreview = value; }
        }

        bool _showTileMap = false;
        float _currentTime = 0f;
        private const float MaxTime = 10f;
        float _alpha = 1.0f;
        private const int TileMapSx = 666;
        private const int TileMapSy = 200;
        private const float TileMapScale = 0.5f;

        public GameMapEditor()
        {
            TileTypeIndex = 0;
        }

        public bool IsConfigLoaded { get; set; }

        public override void Initialize(Game game)
        {
            base.Initialize(game);

            IsConfigLoaded = false;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, GraphicsDeviceManager graphics)
        {
            base.LoadContent(Content, graphics);

            _blankTexture = new Texture2D(graphics.GraphicsDevice, Tile.TileWidth, Tile.TileHeight, false, SurfaceFormat.Color);

            Color[] color = new Color[Tile.TileWidth * Tile.TileHeight];
            for (int i = 0; i < color.Length; i++)
                color[i] = Color.White;

            _blankTexture.SetData(color);


            //rebind input
            _mapEditorInput = new GameInput();
            _mapEditorInput._buttonE_PR = new StateChangeToCommand<GameCore>(this);
            _mapEditorInput._buttonR_PR = new LoadConfigFileCommand();
            _mapEditorInput._buttonA_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Left);
            _mapEditorInput._buttonD_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Right);
            _mapEditorInput._buttonW_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Up);
            _mapEditorInput._buttonS_PR = new MoveTileIndexCmd(MoveTileIndexCmd.TileIndexDir.Down);
            _mapEditorInput._buttonQ_PR = new IncrementTileTypeIndexCmd();
            _mapEditorInput._mouseLeft_P_NoLShf = new AddTileToMapCmd();
            _mapEditorInput._mouseLeft_P_LShf = new MoveGameActorToPositionCommand(this.Player);
            _mapEditorInput._buttonLeftShift_P = new SetPreviewCursor(false);
            _mapEditorInput._mouseRight_PR = new RemoveTileMapCmd();
        }

        public override void Update(GameTime gameTime, IContext context)
        {
            this.GameTime = gameTime;
            this.ShowPreviewMode = true;

            this._mapEditorInput.HandleInput(ref CommandQueue);
            while (CommandQueue.Any())
            {
                var cmd = CommandQueue.Dequeue();

                if (cmd == null) continue;
                cmd.Execute(Camera);
                cmd.Execute(this);
                cmd.Execute(context);
            }

            if (!IsConfigLoaded)
            {
                MyMap.RegisterConfigurationFile();
                IsConfigLoaded = true;
            }

            UpdateMiniTileMap(gameTime);

            base.UpdateActors(gameTime);
            base.GameInput.HandleInput();
            base.UpdateCameraFirstSquare();
            base.UpdateHilight();
            base.UpdateMapMouseScroll();

            //string text = base.InformationalTxt;
            base.InformationalTxt = string.Format("Tile Map Editor Ver 0.01\nTile Type: ({0}), Index={1}, ObjectName={2}", CurrentTileType.ToString(), MyMap.TileIndex, MyMap.GetTileMapLogicalObjName(MyMap.TileIndex));
        }

        private void UpdateMiniTileMap(GameTime gameTime)
        {
            //fix tile type if required given the index
            TileType t = MyMap.GetGameTileInfoList(MyMap.TileIndex).ElementAt(0).TileType;

            switch (t)
            {
                case TileType.Multi:
                case TileType.Height:
                case TileType.Topper:
                    CurrentTileType = t;
                    break;
            }
            //Show Tile Map for 1 seconds
            //
            if (MyMap.OldTileIdx != MyMap.TileIndex)
            {
                _showTileMap = true;
                _currentTime = 0;
            }

            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            _alpha = ((_currentTime <= MaxTime - 1f) ? 1.0f : -_currentTime + MaxTime);

            if (_currentTime >= MaxTime)
            {
                _showTileMap = false;
                _currentTime = 0f;
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
                this.Camera.GetTransformation());

            #region DRAW TILE PART (FROM MOUSE)

            //get hilight row offset

            if (_showPreview)
            {
                int hilightrowOffset = ((this.HilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

                foreach (GameTileInfo t in MyMap.GetGameTileInfoList(MyMap.TileIndex))
                {
                    spriteBatch.Draw(
                        Tile.TileSetTexture,
                            new Vector2(
                                HilightPoint.X * Tile.TileStepX + hilightrowOffset - t.TileXOffset*Tile.MultiSizeTileOffset,
                                HilightPoint.Y * Tile.TileStepY - t.TileYOffset * Tile.MultiSizeTileOffset - MyMap.GetOverallCenterHeight(HilightPoint.Y, HilightPoint.X) ),
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
            if (_showTileMap)
            {
                spriteBatch.Draw(
                    Tile.TileSetTexture,
                        Camera.ScreenToWorld(new Vector2(
                            TileMapSx,
                            TileMapSy)),
                    Tile.GetSourceTileSet(),
                    Color.Aquamarine * _alpha ,
                    0.0f,
                    Vector2.Zero,
                    TileMapScale,
                    SpriteEffects.None,
                    0.1f);

                foreach (int id in MyMap.GetTileMapHilightIndexes(MyMap.TileIndex))
                {
                    spriteBatch.Draw(
                        _blankTexture,
                            Camera.ScreenToWorld(new Vector2(
                                TileMapSx + Tile.TileWidth * (id % MyMap.MaxTileHorizontalIndex) * TileMapScale,
                                TileMapSy + Tile.TileHeight * (id / MyMap.MaxTileHorizontalIndex) * TileMapScale)),
                        new Rectangle(0, 0, _blankTexture.Width, _blankTexture.Height),
                        Color.White * _alpha * 0.2f,
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
