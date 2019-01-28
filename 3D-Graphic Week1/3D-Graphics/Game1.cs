using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sample;
using System.Collections.Generic;
using System.Diagnostics;

namespace _3D_Graphics
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont sfont;
        InputEngine input;
        DebugEngine debug;
        ImmediateShapeDrawer shapeDrawer;
        List<GameObject3D> gameObjects = new List<GameObject3D>();
        Camera mainCamera;
        OcclusionQuery occQuery;
        QuadTree quadTree;
        Stopwatch timer = new Stopwatch();
        long totalTime = 0;
        int objectsDrawn;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.ApplyChanges();

            input = new InputEngine(this);
            debug = new DebugEngine();
            shapeDrawer = new ImmediateShapeDrawer();

            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameUtilities.Content = Content;
            GameUtilities.GraphicsDevice = GraphicsDevice;

            debug.Initialize();
            shapeDrawer.Initialize();

            mainCamera = new Camera("cam", new Vector3(0, 5, 10), new Vector3(0, 0, -1));
            mainCamera.Initialize();

            quadTree = new QuadTree(100, Vector2.Zero, 5);

            occQuery = new OcclusionQuery(GraphicsDevice);
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected void AddModel(SimpleModel model)
        {
            model.Initialize();
            //gameObjects.Add(model);
            quadTree.AddObject(model);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sfont = Content.Load<SpriteFont>("debug");

            gameObjects.Add(new SimpleModel("wall0", "wall", new Vector3(0, 0, -10)));
            gameObjects.Add(new SimpleModel("ball0", "ball", new Vector3(0, 2.5f, -15)));
            // TODO: use this.Content to load your game content here
            gameObjects.ForEach(go => go.LoadContent());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameUtilities.Time = gameTime;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            mainCamera.Update();


            quadTree.ProcessTree(mainCamera.Frustum, ref gameObjects);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            debug.Draw(mainCamera);
            foreach (SimpleModel go in gameObjects)
            {
                if (FrustumContains(go))
                {
                    if (!IsOccluded(go))
                    {
                        go.Draw(mainCamera);
                        objectsDrawn++;
                    }
                   
                }
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(sfont, "Objects Drawn:" + objectsDrawn, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(sfont, "Occlusion Time Drawn:" + totalTime, new Vector2(10, 40), Color.White);
            spriteBatch.End();
            objectsDrawn = 0;
            timer.Reset();
            totalTime = 0;
            GameUtilities.SetGraphicsDeviceFor3D();
            base.Draw(gameTime);
        }

         bool FrustumContains(SimpleModel model)
        {
            if (mainCamera.Frustum.Contains(model.AABB)!= ContainmentType.Disjoint)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool IsOccluded(SimpleModel model)
        {
            timer.Start();
            
            occQuery.Begin();
            shapeDrawer.DrawBoundingBox(model.AABB, mainCamera);
            occQuery.End();

            while (!occQuery.IsComplete)
            {
                
            }

            timer.Stop();

            totalTime += timer.ElapsedMilliseconds;

            if (occQuery.IsComplete && occQuery.PixelCount > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }
    }
}
