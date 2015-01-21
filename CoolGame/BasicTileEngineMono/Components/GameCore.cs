using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace BasicTileEngineMono.Components
{

    public class GameCore : GameProcess
    {

        #region GAME CLIENT PROPERTIES
        //Input Handler
        public GameInput GameInput;

        public GameCoreEventSystem GameCoreEventSys;
        protected Achievements GameAchievements;


        //Default Tile Map: defines what's in a map
        protected TileMap MyMap;
        public TileMap GameMap
        {
            get { return MyMap; }
        }

        //for isometric map support
        //this isometric set, is by 64x64,but the image only occupies bottom 32 pixels of tile.
        //http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:4
        //Base offset controls the screen coordinate offset from the top left of the screen coordinates
        const int BaseOffsetX = -32;
        const int BaseOffsetY = -64;
        const float HeightRowDepthMod = 0.0000001f;

        //squaresAcross/Down : define how many tiles to show on screen at once
        const int SquaresAcross = 25;
        const int SquaresDown = 50;

        //2D camera
        protected Camera Camera;
        public Camera GameCamera
        {
            get { return Camera; }
        }

        //for keeping the first rendering square upper left of screen according to camera view
        Vector2 _firstRenderSquare;

        //debugging tile locations on map
        protected SpriteFont Pericles6;
        protected SpriteFont Snippets14;
        public bool EnableDebugging { get; set; }

        //highlighting tiles
        Texture2D _hilight;
        protected Vector2 HilightLoc;
        protected Point HilightPoint;
        public Point HiLightPoint
        {
            get { return HilightPoint; }
        }

        //NPC character
        SpriteAnimation _npc;

        //player actor;
        public GameActor Player;


        //Information for debugging
        public Queue<Point> MapPoints = new Queue<Point>();
        public string InformationalTxt { get; set; }

        #endregion


        public override void Initialize(Game game)
        {
            base.Initialize(game);

            //debugging and others
            InformationalTxt = "";
            EnableDebugging = false;

            MapPoints.Enqueue(Point.Zero);
            MapPoints.Enqueue(Point.Zero);

            GameCoreEventSys = new GameCoreEventSystem();
            GameAchievements = new Achievements(this);

            GameCoreEventSys.AddObserver(GameAchievements);
        }

        public override void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            //Load tiles
            Tile.TileSetTexture = content.Load<Texture2D>(@"Textures\TileSets\part4_tileset");

            //load fonts
            Pericles6 = content.Load<SpriteFont>(@"Fonts\Pericles6");
            Snippets14 = content.Load<SpriteFont>(@"Fonts\Snippets14");

            //initialize TileMap
            MyMap = new TileMap(
                content.Load<Texture2D>(@"Textures\TileSets\mousemap"),
                content.Load<Texture2D>(@"Textures\TileSets\part9_slopemaps"))
            {
                //Tileset map sizes
                MaxTileHorizontalIndex = 10,
                MaxTileVerticalIndex = 16
            };

            //Load TileMap information
            MyMap.RegisterConfigurationFile();

            //initialize camera
            Camera = new Camera(
                graphics.GraphicsDevice.Viewport, 
                new Vector2(BaseOffsetX, BaseOffsetY), 
                new Rectangle(0,0,(MyMap.MapWidth - 2) * Tile.TileStepX, (MyMap.MapHeight - 2) * Tile.TileStepY),
                1.0f);

            //intiliaze highlight
            _hilight = content.Load<Texture2D>(@"Textures\TileSets\hilight");

            //create sprite and animations for character
            //add an experimental Mobile sprite character
            Texture2D playerTexture = content.Load<Texture2D>(@"Textures\Characters\T_Vlad_Sword_Walking_48x48");

            Player = new GameActor(playerTexture);
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkEast", 0, 48 * 0, 48, 48, 8, 0.1f, "IdleEast");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkNorth", 0, 48 * 1, 48, 48, 8, 0.1f, "IdleNorth");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkNorthEast", 0, 48 * 2, 48, 48, 8, 0.1f, "IdleNorthEast");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkNorthWest", 0, 48 * 3, 48, 48, 8, 0.1f, "IdleNorthWest");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkSouth", 0, 48 * 4, 48, 48, 8, 0.1f, "IdleSouth");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkSouthEast", 0, 48 * 5, 48, 48, 8, 0.1f, "IdleSouthEast");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkSouthWest", 0, 48 * 6, 48, 48, 8, 0.1f, "IdleSouthWest");
            Player.ActorMobileSprite.Sprite.AddAnimation("WalkWest", 0, 48 * 7, 48, 48, 8, 0.1f, "IdleWest");
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleEast", 0, 48 * 0, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleNorth", 0, 48 * 1, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleNorthEast", 0, 48 * 2, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleNorthWest", 0, 48 * 3, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleSouth", 0, 48 * 4, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleSouthEast", 0, 48 * 5, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleSouthWest", 0, 48 * 6, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.Sprite.AddAnimation("IdleWest", 0, 48 * 7, 48, 48, 1, 0.2f);
            Player.ActorMobileSprite.EndPathAnimation = "IdleEast";
            Player.ActorMobileSprite.Sprite.DrawOffset = new Vector2(-24, -38);
            Player.ActorMobileSprite.Sprite.CurrentAnimation = "IdleEast";
            Player.ActorMobileSprite.Sprite.AutoRotate = false;
            Player.ActorMobileSprite.Position = Camera.ScreenToWorld(MyMap.MapCellToScreen(Camera,new Vector2(1,9)));
            Player.ActorMobileSprite.Target = Player.ActorMobileSprite.Position;
            Player.ActorMobileSprite.Speed = 2f;
            Player.ActorMobileSprite.IsPathing = true;
            Player.ActorMobileSprite.LoopPath = false;
            Player.ActorMobileSprite.IsActive = false;

            //load NPC texture
            _npc = new SpriteAnimation(content.Load<Texture2D>(@"Textures\Characters\SmileyWalk"));
            _npc.AddAnimation("Idle1", 0, 0, 64, 64, 4, 0.1f, "Idle2");
            _npc.AddAnimation("Idle2", 0, 64 * 1, 64, 64, 4, 0.1f, "Idle3");
            _npc.AddAnimation("Idle3", 0, 64 * 2, 64, 64, 4, 0.1f, "Idle4");
            _npc.AddAnimation("Idle4", 0, 64 * 3, 64, 64, 4, 0.1f, "Idle1");
            _npc.Position = new Vector2(200, 200);
            _npc.DrawOffset = new Vector2(-32, -32);
            _npc.CurrentAnimation = "Idle1";
            _npc.IsAnimating = true;

            //Initialize InputHandler
            // ReSharper disable once UseObjectOrCollectionInitializer
            GameInput = new GameInput();
            GameInput._buttonE_PR = new StateChangeToCommand<GameMapEditor>(this);
            GameInput._buttonQ_PR = new StateChangeToCommand<GameMenu>(this);
            GameInput._buttonEnd_PR = new MarkWorldPointCommand(GameInput);
            GameInput._buttonHome_PR = new MarkWorldPointCommand(GameInput);
            GameInput._buttonD_PR = new CalculateDistanceDebuggerCommand(this);
            GameInput._buttonNum1_P = new MoveGameActorCommand(this, new Vector2(-2, 1), "WalkSouthWest");
            GameInput._buttonNum2_P = new MoveGameActorCommand(this, new Vector2(0, 1), "WalkSouth");
            GameInput._buttonNum3_P = new MoveGameActorCommand(this, new Vector2(2, 1), "WalkSouthEast");
            GameInput._buttonNum4_P = new MoveGameActorCommand(this, new Vector2(-2, 0), "WalkWest");
            GameInput._buttonNum6_P = new MoveGameActorCommand(this, new Vector2(2, 0), "WalkEast");
            GameInput._buttonNum7_P = new MoveGameActorCommand(this, new Vector2(-2, -1), "WalkNorthWest");
            GameInput._buttonNum8_P = new MoveGameActorCommand(this, new Vector2(0, -1), "WalkNorth");
            GameInput._buttonNum9_P = new MoveGameActorCommand(this, new Vector2(2, -1), "WalkNorthEast");
            GameInput._mouseLeft_PR = new MoveGameActorToPositionCommand(this.Player);
        }

        public override void Update(GameTime gameTime, IContext context)
        {
            base.Update(gameTime, context);

            #region COMMAND HANDLING
            GameInput.HandleInput(ref CommandQueue);


            while (CommandQueue.Any())
            {
                var cmd = CommandQueue.Dequeue();

                if (cmd == null) continue;
                cmd.Execute(Camera);

                //if something is waiting on the stack, such as a dialog, must process(pop) before the following can execute
                if (!IsEmptySubProcessStack()) continue;
                cmd.Execute(context);
                cmd.Execute(this);
                cmd.Execute(Player);
            }
            
            #endregion

            #region UPDATES

            //update subjects that notify observers
            this.GameCoreEventSys.UpdateEntity(this);
            //scroll if player is active and moves out of screen
            UpdateMapScrollPlayerView();
            //scroll according to mouse position
            UpdateMapMouseScroll();
            //update all actors
            UpdateActors(gameTime);
            //update hilight location
            UpdateHilight();
            //Camera is intiially at 0,0 and camera can move around
            //we need to enables pan to the location of the map where the camera points
            //firstX and firstY are map square coordinates of the game map that camera points at
            UpdateCameraFirstSquare();

            InformationalTxt = "Tile Engine Ver 0.1" +
                            string.Format("\nMouse Position(W): ({0})", Camera.ScreenToWorld(new Vector2(GameInput.MousePosition.X, GameInput.MousePosition.Y))) +
                            string.Format("\nMouse Position(S): ({0})", GameInput.MousePosition) +
                            string.Format("\nMouse Cell Position(Cell): ({0},{1})", HilightPoint.X, HilightPoint.Y) +
                            string.Format("\nMouse Cell Position(W): ({0})", Camera.ScreenToWorld(MyMap.MapCellToScreen(Camera, new Vector2(HilightPoint.X, HilightPoint.Y)))) +
                            string.Format("\nCamera Position(W): ({0})", Camera.Location) +
                            string.Format("\nWorld Bounds: ({0},{1})", Camera.WorldWidth, Camera.WorldHeight) +
                            string.Format("\nPlayer Position(W): ({0},{1})", Player.ActorMobileSprite.Position.X, Player.ActorMobileSprite.Position.Y);
            #endregion

        }

        #region UPDATE SUB ROUTINES

        protected void UpdateActors(GameTime gameTime)
        {
            //update main player
            Player.Update(gameTime,MyMap);

            //update NPC
            _npc.Update(gameTime);
        }
        protected void UpdateCameraFirstSquare()
        {
            _firstRenderSquare = MyMap.GetCameraFirstSquare(Camera);
        }
        protected void UpdateHilight()
        {
            HilightLoc = Camera.ScreenToWorld(new Vector2(GameInput.MousePosition.X, GameInput.MousePosition.Y));
            //get map cell coordinates of mouse point in Update
            HilightPoint = MyMap.WorldToMapCell(new Point((int)HilightLoc.X, (int)HilightLoc.Y));
        }
        protected void UpdateMapScrollPlayerView()
        {
            Vector2 testPos = Camera.WorldToScreen(Player.ActorMobileSprite.Position);
            //only moves the camera when player is active
            if (!Player.ActorMobileSprite.IsActive) return;

            if (testPos.X < 100)
                Camera.Move(new Vector2(testPos.X - 100, 0));

            if (testPos.X > (Camera.ViewWidth - 100))
                Camera.Move(new Vector2(testPos.X - (Camera.ViewWidth - 100), 0));

            if (testPos.Y < 100)
                Camera.Move(new Vector2(0, testPos.Y - 100));

            if (testPos.Y > (Camera.ViewHeight - 100))
                Camera.Move(new Vector2(0, testPos.Y - (Camera.ViewHeight - 100)));
        }
        protected void UpdateMapMouseScroll()
        {
            int Multiplier = 1;
            int Margin = 5;
            Point testPosMouse = GameInput.MousePosition;
            Vector2 moveVec = Vector2.Zero;

            if (testPosMouse.X < Margin)
                moveVec = new Vector2(testPosMouse.X - Margin, 0);

            if (testPosMouse.X > (Camera.ViewWidth - Margin))
                moveVec = new Vector2(testPosMouse.X - Camera.ViewWidth + Margin, 0);

            if (testPosMouse.Y < Margin)
                moveVec = new Vector2(0, testPosMouse.Y - Margin);

            if (testPosMouse.Y > (Camera.ViewHeight - Margin))
                moveVec = new Vector2(0, testPosMouse.Y - Camera.ViewHeight + Margin);

            moveVec.Y = MathHelper.Clamp(moveVec.Y, -5, 5);
            moveVec.X = MathHelper.Clamp(moveVec.X, -10, 10);

            moveVec *= Multiplier;

            Camera.Move(moveVec);
        }
        #endregion

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, IContext context)
        {
            #region PREPARATION
            //get the first rendering square map cell coordinates
            int firstX = (int)_firstRenderSquare.X;
            int firstY = (int)_firstRenderSquare.Y;

            //compute the max depth count to draw any map (for 50x50 map this would be (51 + 51*64)*10=33150)
            float maxdepth = ((MyMap.MapWidth + 1) + ((MyMap.MapHeight + 1) * Tile.TileWidth)) * 10;
            #endregion


            //Set up sprite batch. Tells XNA/Monogame that we are going to specify layer depth (sorted from back(1.0f) to front(0.0f))
            spriteBatch.Begin(
                SpriteSortMode.BackToFront, 
                BlendState.AlphaBlend,
                SamplerState.LinearWrap,
                null,
                null,
                null,
                Camera.GetTransformation());

            #region DRAW TILES
            //zig-zag rendering approach
            //http://stackoverflow.com/questions/892811/drawing-isometric-game-worlds


            //loop through map cells given current screenview
            for (int y = 0; y < SquaresDown; y++)
            {
                //for supporting hexagonal and isometric maps, odd row must be offset
                int rowOffset = ((firstY + y) % 2 == 1) ? rowOffset = Tile.OddRowXOffset : 0;

                for (int x = 0; x < SquaresAcross; x++)
                {
                    //controlling the depth of isometric tiles depending on the x,y location
                    int mapx = (firstX + x);
                    int mapy = (firstY + y);
                    //get the depth offset depending on the mapx and mapy
                    //increasing mapy primarily decreases depthOffset, followed by increasing mapx decreasing depthOffset slightly
                    float depthOffset = 0.7f - ((mapx + (mapy * Tile.TileWidth)) / maxdepth);

                    //if mapx hits map size don't render tiles
                    if (mapx >= MyMap.MapWidth || mapy >= MyMap.MapHeight)
                        continue;

                    //Clipping: if mapx hits view port size, don't render tiles
                    Vector2 renderpoint = Camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY));
                    if( renderpoint.X >= Camera.ViewWidth || renderpoint.Y >= Camera.ViewHeight) 
                        continue;

                    #region DRAW BASE TILES
                    
                    //draw base tiles
                    foreach (int tileID in MyMap.Rows[mapy].Columns[mapx].BaseTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY),
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            1.0f);
                    }
                    
                    #endregion

                    #region STACKED HEIGHT TILES
                    //draw height tiles
                    //keep track of how many heigh tiles drawn
                    int heightRow = 0;
                    foreach (int tileID in MyMap.Rows[mapy].Columns[mapx].HeightTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset)),
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen (the value of heightRowDepthMod
                            depthOffset - (heightRow * HeightRowDepthMod));
                        heightRow++;
                    }
                    #endregion

                    #region DRAW TOPPER TILES (SKINS) 
                    int topperCount = 0;
                    //draw topper tiles
                    foreach (int tileID in MyMap.Rows[y + firstY].Columns[x + firstX].TopperTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY - (heightRow * Tile.HeightTileOffset)),
                            //get source tile
                            Tile.GetSourceRectangle(tileID),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen
                            depthOffset - ((heightRow + ++topperCount) * HeightRowDepthMod));
                    }
                    #endregion

                    #region DETERMINE DRAW DEPTH OF PLAYER
                    if ((mapx == Player.ActorMapCellPosition.X) && (mapy == Player.ActorMapCellPosition.Y))
                        Player.ActorMobileSprite.Sprite.DrawDepth = depthOffset - (heightRow + 2) * HeightRowDepthMod;

                    #endregion

                    #region DRAW MULTI SIZE TILES
                    //draw multi size tiles
                    int multidrawcount = 1;
                    foreach (var tile in MyMap.Rows[mapy].Columns[mapx].MultiSizeTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            new Vector2(
                                mapx * Tile.TileStepX + rowOffset - (tile.Item2 * Tile.MultiSizeTileOffset),
                                mapy * Tile.TileStepY - (tile.Item3 * Tile.MultiSizeTileOffset) - MyMap.GetOverallCenterHeight(mapy,mapx)),
                            Tile.GetSourceRectangle(tile.Item1),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen
                            depthOffset - ((float)MyMap.GetOverallCenterHeight(mapy,mapx) + topperCount + multidrawcount) * HeightRowDepthMod);
                    }
                    
                    #endregion

                  
                    #region DEBUGGING
                    //debugging tile draw location
                    if (!EnableDebugging) continue;

                    //draw isometric staggered coordinates
                    spriteBatch.DrawString(
                        Pericles6,
                        mapx.ToString() + ", " + mapy.ToString(),
                        new Vector2(
                            mapx * Tile.TileStepX + rowOffset + 24,
                            mapy * Tile.TileStepY + 48),
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0.0f);

                    //draw L0 distance from Source cell
                    spriteBatch.DrawString(
                        Pericles6,
                        TileMap.L0TileDistance(MapPoints.ElementAt(0), new Point(mapx, mapy)).ToString(),
                        new Vector2(
                            mapx * Tile.TileStepX + rowOffset + 24,
                            mapy * Tile.TileStepY + 32),
                        Color.Aqua,
                        0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0.0f);

                    #endregion
                }
            }
            #endregion

            #region DRAW PLAYER
            //draw player according to where he's standing on
            Player.ActorMobileSprite.Draw(spriteBatch, 0, -MyMap.GetOverallHeight(Player.ActorMobileSprite.Position));
            #endregion

            #region DRAW HILIGHT LOCATION (FROM MOUSE)

            //get hilight row offset
            int hilightrowOffset = ((HilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

            spriteBatch.Draw(
                _hilight,
                    new Vector2(
                        HilightPoint.X * Tile.TileStepX + hilightrowOffset,
                //add 2 as image is only half of actual tiles
                        (HilightPoint.Y + 2) * Tile.TileStepY),
                new Rectangle(0, 0, 64, 32),
                Color.White * 0.3f,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            #endregion


            spriteBatch.DrawString(
                            Pericles6,
                            InformationalTxt,
                            Camera.ScreenToWorld(new Vector2(10,560)),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);


            #region DRAW SEARCHED PATH

            if (EnableDebugging)
            {
                for (int i = 0; i < Player.SearchPath.Count(); i++)
                {
                    PathNode n = Player.SearchPath[i];

                    int pathHilightrowOffset = ((n.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

                    Color pathBaseColor = Color.Blue;
                    float alpha = 0.5f;
                    if (i == 0 || i == Player.SearchPath.Count() - 1)
                        alpha = 0.8f;

                    spriteBatch.Draw(
                        _hilight,
                        new Vector2(
                            n.X * Tile.TileStepX + pathHilightrowOffset,
                        //add 2 as image is only half of actual tiles
                            (n.Y + 2) * Tile.TileStepY),
                        new Rectangle(0, 0, 64, 32),
                        pathBaseColor * alpha,
                        0.0f,
                        Vector2.Zero,
                        1.0f,
                        SpriteEffects.None,
                        0.0f);
                }
            }
            #endregion

            #region DRAW NPCS
            _npc.Draw(spriteBatch, 0, 0);
            #endregion

            spriteBatch.End();

            #region DRAW SUB PROCESS RENDERS
            base.Render(gameTime, spriteBatch, context);
            #endregion
        }
    }
}
