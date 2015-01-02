﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Diagnostics;

namespace BasicTile
{

    public class GameCore : GameProcess
    {

        #region GAME CLIENT PROPERTIES
        //Default Tile Map: defines what's in a map
        protected TileMap myMap;

        //for isometric map support
        //this isometric set, is by 64x64,but the image only occupies bottom 32 pixels of tile.
        //http://xnaresources.com/default.asp?page=Tutorial:TileEngineSeries:4
        //Base offset controls the screen coordinate offset from the top left of the screen coordinates
        int baseOffsetX = -32;
        int baseOffsetY = -64;
        float heightRowDepthMod = 0.0000001f;

        //squaresAcross/Down : define how many tiles to show on screen at once
        int SquaresAcross = 25;
        int SquaresDown = 50;

        //2D camera
        protected Camera camera;

        //for keeping the first rendering square upper left of screen according to camera view
        Vector2 firstSquare;

        //debugging tile locations on map
        protected SpriteFont pericles6;
        protected SpriteFont snippets14;
        bool EnableDebugging = false;

        //highlighting tiles
        Texture2D hilight;
        protected Vector2 hilightLoc;
        protected Point hilightPoint;

        //NPC character
        SpriteAnimation npc;

        //player character
        MobileSprite vladMobile;
        Point vladMobileMapPoint;
        Queue<float> lastframesangles = new Queue<float>();

        //Information for debugging
        public string InformationalTxt {get;set;}

        //A* PathFinding variables
        Point startMapPoint;
        Point endMapPoint;
        List<PathNode> foundPath = new List<PathNode>();
        #endregion


        public override void Initialize(Game game)
        {
            //intialize old keyboard and mouse state
            oldState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();

            //make mouse visible
            game.IsMouseVisible = true;

            InformationalTxt = "";
        }

        public override void LoadContent(ContentManager Content, GraphicsDeviceManager graphics)
        {
            //Load tiles
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part4_tileset");

            //load fonts
            pericles6 = Content.Load<SpriteFont>(@"Fonts\Pericles6");
            snippets14 = Content.Load<SpriteFont>(@"Fonts\Snippets14");

            //initialize TileMap
            myMap = new TileMap(
                Content.Load<Texture2D>(@"Textures\TileSets\mousemap"),
                Content.Load<Texture2D>(@"Textures\TileSets\part9_slopemaps"));

            //Tileset map sizes
            myMap.MaxTileHorizontalIndex = 10;
            myMap.MaxTileVerticalIndex = 16;

            //initialize camera
            camera = new Camera(
                graphics.GraphicsDevice.Viewport, 
                new Vector2(baseOffsetX, baseOffsetY), 
                new Rectangle(0,0,(myMap.MapWidth - 2) * Tile.TileStepX, (myMap.MapHeight - 2) * Tile.TileStepY),
                1.0f);

            //intiliaze highlight
            hilight = Content.Load<Texture2D>(@"Textures\TileSets\hilight");

            //create sprite and animations for character
            //add an experimental Mobile sprite character
            vladMobile = new MobileSprite(Content.Load<Texture2D>(@"Textures\Characters\T_Vlad_Sword_Walking_48x48"));
            vladMobile.Sprite.AddAnimation("WalkEast", 0, 48 * 0, 48, 48, 8, 0.1f, "IdleEast");
            vladMobile.Sprite.AddAnimation("WalkNorth", 0, 48 * 1, 48, 48, 8, 0.1f, "IdleNorth");
            vladMobile.Sprite.AddAnimation("WalkNorthEast", 0, 48 * 2, 48, 48, 8, 0.1f, "IdleNorthEast");
            vladMobile.Sprite.AddAnimation("WalkNorthWest", 0, 48 * 3, 48, 48, 8, 0.1f, "IdleNorthWest");
            vladMobile.Sprite.AddAnimation("WalkSouth", 0, 48 * 4, 48, 48, 8, 0.1f, "IdleSouth");
            vladMobile.Sprite.AddAnimation("WalkSouthEast", 0, 48 * 5, 48, 48, 8, 0.1f, "IdleSouthEast");
            vladMobile.Sprite.AddAnimation("WalkSouthWest", 0, 48 * 6, 48, 48, 8, 0.1f, "IdleSouthWest");
            vladMobile.Sprite.AddAnimation("WalkWest", 0, 48 * 7, 48, 48, 8, 0.1f, "IdleWest");
            vladMobile.Sprite.AddAnimation("IdleEast", 0, 48 * 0, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleNorth", 0, 48 * 1, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleNorthEast", 0, 48 * 2, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleNorthWest", 0, 48 * 3, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleSouth", 0, 48 * 4, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleSouthEast", 0, 48 * 5, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleSouthWest", 0, 48 * 6, 48, 48, 1, 0.2f);
            vladMobile.Sprite.AddAnimation("IdleWest", 0, 48 * 7, 48, 48, 1, 0.2f);
            vladMobile.EndPathAnimation = "IdleEast";
            vladMobile.Sprite.DrawOffset = new Vector2(-24, -38);
            vladMobile.Sprite.CurrentAnimation = "IdleEast";
            vladMobile.Sprite.AutoRotate = false;
            vladMobile.Position = camera.ScreenToWorld(myMap.MapCellToScreen(camera,new Vector2(1,9)));
            vladMobile.Target = vladMobile.Position;
            vladMobile.Speed = 2f;
            vladMobile.IsPathing = true;
            vladMobile.LoopPath = false;
            vladMobile.IsActive = false;

            //load NPC texture
            npc = new SpriteAnimation(Content.Load<Texture2D>(@"Textures\Characters\SmileyWalk"));
            npc.AddAnimation("Idle1", 0, 0, 64, 64, 4, 0.1f, "Idle2");
            npc.AddAnimation("Idle2", 0, 64 * 1, 64, 64, 4, 0.1f, "Idle3");
            npc.AddAnimation("Idle3", 0, 64 * 2, 64, 64, 4, 0.1f, "Idle4");
            npc.AddAnimation("Idle4", 0, 64 * 3, 64, 64, 4, 0.1f, "Idle1");
            npc.Position = new Vector2(200, 200);
            npc.DrawOffset = new Vector2(-32, -32);
            npc.CurrentAnimation = "Idle1";
            npc.IsAnimating = true;
        }

        public override void Update(GameTime gameTime, Context context)
        {
            // TODO: Add your update logic here
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            #region ZOOMING
            UpdateScreenZoom();
            #endregion

            #region SCROLLING
            //scroll if player is active and moves out of screen
            UpdateMapScrollPlayerView();
            //scroll according to mouse position
            MapMouseScroll();
            #endregion

            #region MOVE PLAYER BY MOUSE OR BY KEY
            UpdatePlayerByMouse(gameTime);
            UpdatePlayerByKey(gameTime);

            #endregion

            #region UPDATE CHARACTER POSITIONS

            //update the map cell where player is
            vladMobileMapPoint = myMap.WorldToMapCell(new Point((int)vladMobile.Position.X, (int)vladMobile.Position.Y));

            npc.Update(gameTime);

            #endregion

            #region UPDATE MOUSE HILIGHT LOCATION
            UpdateHilight();
            #endregion

            #region UPDATE THE FIRST SQUARE LOCATION
            //Camera is intiially at 0,0 and camera can move around
            //we need to enables pan to the location of the map where the camera points
            //firstX and firstY are map square coordinates of the game map that camera points at
            UpdateCameraFirstSquare();
            #endregion

            #region OTHER FUNCTIONS KEY TOGGLING
            //enable or disable debugging coordinates
            if (ks.IsKeyUp(Keys.Delete) && oldState.IsKeyDown(Keys.Delete))
                EnableDebugging = !EnableDebugging;

            //quit core to menu
            if (ks.IsKeyUp(Keys.Q) && oldState.IsKeyDown(Keys.Q))
                context.changeState(typeof(GameMenu));

            //map editor mode
            if (ks.IsKeyUp(Keys.E) && oldState.IsKeyDown(Keys.E))
                context.changeState(typeof(GameMapEditor));

            //TODO: Experimental: calculate distance and pathfinding
            //get destination point
            if (ks.IsKeyUp(Keys.End) && oldState.IsKeyDown(Keys.End))
                endMapPoint = hilightPoint;
            //get start point
            if(ks.IsKeyUp(Keys.Home) && oldState.IsKeyDown(Keys.Home))
                startMapPoint = hilightPoint;
            //calculate distance, path
            if(ks.IsKeyUp(Keys.D) && oldState.IsKeyDown(Keys.D))
            {
                GameMessageBox message = context.getFactory().GameMessageBox(
                    string.Format("({0},{1}) -> ({2},{3}) Distance={4}", 
                    startMapPoint.X,
                    startMapPoint.Y,
                    endMapPoint.X,
                    endMapPoint.Y,
                    TileMap.L0TileDistance(startMapPoint, endMapPoint)
                    ), "Debug");
                this.Push(message);
            }

            if (ks.IsKeyUp(Keys.P) && oldState.IsKeyDown(Keys.P))
            {
                PathFinder p = new PathFinder(myMap);

                if (p.Search(startMapPoint.X, startMapPoint.Y, endMapPoint.X, endMapPoint.Y, myMap))
                    foundPath = p.PathResult();
            }

            //Sub-Process Stack Operations
            if (this.IsEmptySubProcessStack())
            {
                //Show MessageBox 1
                if (ks.IsKeyDown(Keys.S))
                {
                    GameMessageBox message = context.getFactory().GameMessageBox("タイルエンジンへようこそ。このメッセージを消す場合はBを押してください。", "メッセージ");
                    this.Push(message);
                }

                //Show 2 MessageBoxes
                if (ks.IsKeyDown(Keys.A))
                {
                    GameMessageBox message2 = context.getFactory().GameMessageBox("タイルエンジン著作者：大朏　哲明", "メッセージ", 100, 150);
                    GameMessageBox message = context.getFactory().GameMessageBox("タイルエンジンへようこそ。このメッセージを消す場合はEnterを押してください。", "メッセージ", 100, 150);

                    this.Push(message2);
                    this.Push(message);
                }
            }
            #endregion


            oldState = ks;
            oldMouseState = ms;


            InformationalTxt = "Tile Engine Ver 0.1" +
                            string.Format("\nMouse Position(W): ({0},{1})", camera.ScreenToWorld(ms.Position).X, camera.ScreenToWorld(ms.Position).Y) +
                            string.Format("\nMouse Position(S): ({0},{1})", ms.Position.X, ms.Position.Y) +
                            string.Format("\nMouse Cell Position(Cell): ({0},{1})", hilightPoint.X, hilightPoint.Y) +
                            string.Format("\nMouse Cell Position(W): ({0})", camera.ScreenToWorld(myMap.MapCellToScreen(camera, new Vector2(hilightPoint.X, hilightPoint.Y)))) +
                            string.Format("\nCamera Position(W): ({0})", camera.Location) +
                            string.Format("\nWorld Bounds: ({0},{1})", camera.WorldWidth, camera.WorldHeight) +
                            string.Format("\nPlayer Position(W): ({0},{1})", vladMobile.Position.X, vladMobile.Position.Y);

            base.Update(gameTime, context);
        }


        #region UPDATE SUB
        private void UpdateScreenZoom()
        {
            //TODO: mouse scroll for zooming in and out
            if (ms.ScrollWheelValue < oldMouseState.ScrollWheelValue)
                camera.Scale += 0.1f;
            else if (ms.ScrollWheelValue > oldMouseState.ScrollWheelValue)
                camera.Scale -= 0.1f;
        }
        private string UpdatePlayerByKey(GameTime gameTime)
        {
            string animation = "";
            Vector2 moveVector = Vector2.Zero;
            Vector2 moveDir = Vector2.Zero;
            if (ks.IsKeyDown(Keys.NumPad7))
            {
                moveDir = new Vector2(-2, -1);
                animation = "WalkNorthWest";
                moveVector += new Vector2(-2, -1);
            }

            if (ks.IsKeyDown(Keys.NumPad8))
            {
                moveDir = new Vector2(0, -1);
                animation = "WalkNorth";
                moveVector += new Vector2(0, -1);
            }

            if (ks.IsKeyDown(Keys.NumPad9))
            {
                moveDir = new Vector2(2, -1);
                animation = "WalkNorthEast";
                moveVector += new Vector2(2, -1);
            }

            if (ks.IsKeyDown(Keys.NumPad4))
            {
                moveDir = new Vector2(-2, 0);
                animation = "WalkWest";
                moveVector += new Vector2(-2, 0);
            }

            if (ks.IsKeyDown(Keys.NumPad6))
            {
                moveDir = new Vector2(2, 0);
                animation = "WalkEast";
                moveVector += new Vector2(2, 0);
            }

            if (ks.IsKeyDown(Keys.NumPad1))
            {
                moveDir = new Vector2(-2, 1);
                animation = "WalkSouthWest";
                moveVector += new Vector2(-2, 1);
            }

            if (ks.IsKeyDown(Keys.NumPad2))
            {
                moveDir = new Vector2(0, 1);
                animation = "WalkSouth";
                moveVector += new Vector2(0, 1);
            }

            if (ks.IsKeyDown(Keys.NumPad3))
            {
                moveDir = new Vector2(2, 1);
                animation = "WalkSouthEast";
                moveVector += new Vector2(2, 1);
            }

            if (!vladMobile.IsActive)
            {
                vladMobile.Position += moveVector;
                if (vladMobile.Sprite.CurrentAnimation != animation)
                    vladMobile.Sprite.CurrentAnimation = animation;

                vladMobile.Sprite.Update(gameTime);
            }
            return animation;
        }
        protected string UpdatePlayerByMouse(GameTime gameTime)
        {
            if (oldMouseState.LeftButton == ButtonState.Pressed &&
                ms.LeftButton == ButtonState.Released)
            {
                //activate mobile sprite
                vladMobile.IsActive = true;

                //obtain start and end coordinates
                Point start = myMap.WorldToMapCell(vladMobile.Position);
                Point end = myMap.WorldToMapCell(camera.ScreenToWorld(new Vector2(ms.X, ms.Y)));

                Debug.WriteLine(string.Format("s:({0},{1})", start.X, start.Y));
                Debug.WriteLine(string.Format("e:({0},{1})", end.X, end.Y));

                //clear current existing path (if navigating in progress)
                vladMobile.ClearPathNodes();

                //set up new path finder
                PathFinder p = new PathFinder(myMap);

                if (p.Search(start.X, start.Y, end.X, end.Y, myMap))
                {
                    foundPath = p.PathResult();

                    foreach (PathNode n in foundPath)
                    {
                        Vector2 nodevec = camera.ScreenToWorld(myMap.MapCellToScreen(camera,n.X, n.Y));

                        Debug.WriteLine(string.Format("({0},{1})", nodevec.X, nodevec.Y));
                        //TODO: A probable issue with mapcell -> screen coordinates 
                        vladMobile.AddPathNode(nodevec);
                    }
                    vladMobile.DeactivateAfterPathing = true;
                }
            }

            //calculate angle between vladMobile heading direction and a vector facing N
            Double vladmobileAngRad = MobileSprite.signedAngle(vladMobile.Delta, new Vector2(0, -1));
            string animation = "";
            string endanimation = "";


            //smoothing function (average over a few frames, so we won't get jaggy animations)
            if (!Double.IsNaN(vladmobileAngRad))
            {
                if (lastframesangles.Count < 10)
                    lastframesangles.Enqueue((float)vladmobileAngRad);
                else
                {
                    lastframesangles.Dequeue();
                    lastframesangles.Enqueue((float)vladmobileAngRad);
                    vladmobileAngRad = lastframesangles.Average();
                }
            }

            if (Math.PI / 8 > vladmobileAngRad && vladmobileAngRad > -Math.PI / 8)
            {
                animation = "WalkNorth";
                endanimation = "IdleNorth";
            }

            //isometric angles is "flatter" than it looks
            //eaxctly 67.5 deg won't work so make it abit bigger than (3/8) * pi
            if (-3.5 * Math.PI / 8 < vladmobileAngRad && vladmobileAngRad < -Math.PI / 8)
            {
                animation = "WalkNorthEast";
                endanimation = "IdleNorthEast";
            }

            //exactly (5/8) * pi won't work so make it a bit smaller
            if (-4.9 * Math.PI / 8 < vladmobileAngRad && vladmobileAngRad < -3.5 * Math.PI / 8)
            {
                animation = "WalkEast";
                endanimation = "IdleEast";
            }

            if (-7.5 * Math.PI / 8 < vladmobileAngRad && vladmobileAngRad < -4.9 * Math.PI / 8)
            {
                animation = "WalkSouthEast";
                endanimation = "IdleSouthEast";
            }

            if (7 * Math.PI / 8 < vladmobileAngRad || vladmobileAngRad < -7.5 * Math.PI / 8)
            {
                animation = "WalkSouth";
                endanimation = "IdleSouth";

            }

            if (7 * Math.PI / 8 > vladmobileAngRad && vladmobileAngRad > 4.9 * Math.PI / 8)
            {
                animation = "WalkSouthWest";
                endanimation = "IdleSouthWest";
            }

            if (4.9 * Math.PI / 8 > vladmobileAngRad && vladmobileAngRad > 3.5 * Math.PI / 8)
            {
                animation = "WalkWest";
                endanimation = "IdleWest";
            }

            if (3.5 * Math.PI / 8 > vladmobileAngRad && vladmobileAngRad > Math.PI / 8)
            {
                animation = "WalkNorthWest";
                endanimation = "IdleNorthWest";
            }

            if (vladMobile.Sprite.CurrentAnimation != animation)
            {
                vladMobile.Sprite.CurrentAnimation = animation;
                vladMobile.EndPathAnimation = endanimation;
            }

            vladMobile.Update(gameTime);
            return animation;
        }
        protected void UpdateCameraFirstSquare()
        {
            firstSquare = myMap.GetCameraFirstSquare(camera);
        }
        protected void UpdateHilight()
        {
            hilightLoc = camera.ScreenToWorld(new Vector2(ms.X, ms.Y));
            //get map cell coordinates of mouse point in Update
            hilightPoint = myMap.WorldToMapCell(new Point((int)hilightLoc.X, (int)hilightLoc.Y));
        }
        protected void UpdateMapScrollPlayerView()
        {
            Vector2 testPos = camera.WorldToScreen(vladMobile.Position);
            //only moves the camera when player is active
            if (vladMobile.IsActive)
            {
                if (testPos.X < 100)
                    camera.Move(new Vector2(testPos.X - 100, 0));

                if (testPos.X > (camera.ViewWidth - 100))
                    camera.Move(new Vector2(testPos.X - (camera.ViewWidth - 100), 0));

                if (testPos.Y < 100)
                    camera.Move(new Vector2(0, testPos.Y - 100));

                if (testPos.Y > (camera.ViewHeight - 100))
                    camera.Move(new Vector2(0, testPos.Y - (camera.ViewHeight - 100)));
            }
        }
        protected void MapMouseScroll()
        {
            int Multiplier = 1;
            int Margin = 5;
            Point testPosMouse = ms.Position;
            Vector2 moveVec = Vector2.Zero;

            if (testPosMouse.X < Margin)
                moveVec = new Vector2(testPosMouse.X - Margin, 0);

            if (testPosMouse.X > (camera.ViewWidth - Margin))
                moveVec = new Vector2(testPosMouse.X - camera.ViewWidth + Margin, 0);

            if (testPosMouse.Y < Margin)
                moveVec = new Vector2(0, testPosMouse.Y - Margin);

            if (testPosMouse.Y > (camera.ViewHeight - Margin))
                moveVec = new Vector2(0, testPosMouse.Y - camera.ViewHeight + Margin);

            moveVec.Y = MathHelper.Clamp(moveVec.Y, -5, 5);
            moveVec.X = MathHelper.Clamp(moveVec.X, -10, 10);

            moveVec *= Multiplier;

            camera.Move(moveVec);
        }
        #endregion

        public override void Render(GameTime gameTime, SpriteBatch spriteBatch, Context context)
        {
            #region PREPARATION
            //get the first rendering square map cell coordinates
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            //compute the max depth count to draw any map (for 50x50 map this would be (51 + 51*64)*10=33150)
            float maxdepth = ((myMap.MapWidth + 1) + ((myMap.MapHeight + 1) * Tile.TileWidth)) * 10;
            #endregion


            //Set up sprite batch. Tells XNA/Monogame that we are going to specify layer depth (sorted from back(1.0f) to front(0.0f))
            spriteBatch.Begin(
                SpriteSortMode.BackToFront, 
                BlendState.AlphaBlend,
                SamplerState.LinearWrap,
                null,
                null,
                null,
                camera.GetTransformation());

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
                    if (mapx >= myMap.MapWidth || mapy >= myMap.MapHeight)
                        continue;

                    //Clipping: if mapx hits view port size, don't render tiles
                    Vector2 renderpoint = camera.WorldToScreen(new Vector2(mapx * Tile.TileStepX + rowOffset, mapy * Tile.TileStepY));
                    if( renderpoint.X >= camera.ViewWidth || renderpoint.Y >= camera.ViewHeight) 
                        continue;

                    #region DRAW BASE TILES
                    
                    //draw base tiles
                    foreach (int tileID in myMap.Rows[mapy].Columns[mapx].BaseTiles)
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
                    foreach (int tileID in myMap.Rows[mapy].Columns[mapx].HeightTiles)
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
                            depthOffset - ((float)heightRow * heightRowDepthMod));
                        heightRow++;
                    }
                    #endregion

                    #region DRAW TOPPER TILES (SKINS) 
                    int topperCount = 0;
                    //draw topper tiles
                    foreach (int tileID in myMap.Rows[y + firstY].Columns[x + firstX].TopperTiles)
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
                            depthOffset - ((float)(heightRow + ++topperCount) * heightRowDepthMod));
                    }
                    #endregion

                    #region DETERMINE DRAW DEPTH OF PLAYER
                    if((mapx == vladMobileMapPoint.X) && (mapy == vladMobileMapPoint.Y))
                        vladMobile.Sprite.DrawDepth = depthOffset - (float)(heightRow + 2) * heightRowDepthMod;

                    #endregion

                    #region DRAW MULTI SIZE TILES
                    //draw multi size tiles
                    int multidrawcount = 1;
                    foreach (Tuple<int, int, int, int> tile in myMap.Rows[mapy].Columns[mapx].MultiSizeTiles)
                    {
                        spriteBatch.Draw(
                            Tile.TileSetTexture,
                            //use Camera functions for offsetting and global baseoffset
                            new Vector2(
                                mapx * Tile.TileStepX + rowOffset - (tile.Item2 * Tile.MultiSizeTileOffset),
                                mapy * Tile.TileStepY - (tile.Item3 * Tile.MultiSizeTileOffset) - myMap.GetOverallCenterHeight(mapy,mapx)),
                            Tile.GetSourceRectangle(tile.Item1),
                            Color.White,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            //Every time we draw a height tile, we will move the layer depth 0.0000001f closer to the screen
                            depthOffset - ((float)myMap.GetOverallCenterHeight(mapy,mapx) + topperCount + multidrawcount) * heightRowDepthMod);
                    }
                    multidrawcount++;
                    
                    #endregion

                  
                    #region DEBUGGING
                    //debugging tile draw location
                    if (EnableDebugging)
                    {
                        //draw isometric staggered coordinates
                        spriteBatch.DrawString(
                            pericles6,
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
                            pericles6,
                            TileMap.L0TileDistance(startMapPoint, new Point(mapx, mapy)).ToString(),
                            new Vector2(
                                mapx * Tile.TileStepX + rowOffset + 24,
                                mapy * Tile.TileStepY + 32),
                            Color.Aqua,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);
                    }
                    #endregion
                }
            }
            #endregion

            #region DRAW PLAYER
            //draw player according to where he's standing on
            vladMobile.Draw(spriteBatch, 0, -myMap.GetOverallHeight(vladMobile.Position));
            #endregion

            #region DRAW HILIGHT LOCATION (FROM MOUSE)

            //get hilight row offset
            int hilightrowOffset = ((hilightPoint.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

            spriteBatch.Draw(
                hilight,
                    new Vector2(
                        hilightPoint.X * Tile.TileStepX + hilightrowOffset,
                //add 2 as image is only half of actual tiles
                        (hilightPoint.Y + 2) * Tile.TileStepY),
                new Rectangle(0, 0, 64, 32),
                Color.White * 0.3f,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            #endregion


            spriteBatch.DrawString(
                            pericles6,
                            InformationalTxt,
                            camera.ScreenToWorld(new Vector2(10,560)),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.0f);


            #region DRAW SEARCHED PATH

            if (EnableDebugging)
            {
                for (int i = 0; i < foundPath.Count(); i++)
                {
                    PathNode n = foundPath[i];

                    int pathHilightrowOffset = ((n.Y) % 2 == 1) ? Tile.OddRowXOffset : 0;

                    Color pathBaseColor = Color.Blue;
                    float alpha = 0.5f;
                    if (i == 0 || i == foundPath.Count() - 1)
                        alpha = 0.8f;

                    spriteBatch.Draw(
                        hilight,
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
            npc.Draw(spriteBatch, 0, 0);
            #endregion

            spriteBatch.End();

            #region DRAW SUB PROCESS RENDERS
            base.Render(gameTime, spriteBatch, context);
            #endregion
        }
    }
}
