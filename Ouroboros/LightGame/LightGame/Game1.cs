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

namespace LightGame
{
    
    /// <summary>
    /// The Different states of the game
    /// </summary>
    enum GameState { MENU, GAME, WIN, BACKGROUND, INSTRUCTIONS, RESOURCES, CREDITS }


    /// <summary>
    /// mass above x = certain plate
    /// 1000+ = Garbage Plate
    /// 800+ = Cheese Burger
    /// 600+ = Steak
    /// 400+ = Mac n Cheese
    /// 200+ = cookie
    /// 199- = Truffle
    /// DEFAULT is there for debugging purposes
    /// </summary>
    enum Food { GARBAGEPLATE, CHEESEBURGER, STEAK, MAC, COOKIE, TRUFFLE, DOUGHNUT, DEFAULT }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        //strings for resource/research for littering and hunger
        string cigarettWebsite = "http://www.cigarettelitter.org/";
        string litterFactsWebsite = "http://extra.mdc.mo.gov/nomoretrash/facts/";
        string hungerFundWebsite = "http://www.chfus.org/";

        Rectangle cigarrettWebsiteRectangle = new Rectangle(270,127, 271,12);
        Rectangle litterFactsWebsiteRectangle = new Rectangle(208,221, 391,60);
        Rectangle hungerFundWebsiteRectangle = new Rectangle(332,374, 148,12);


        bool cigBool = false;
        bool hunBool = false;
        bool litBool = false;

        //used for spawning items
        Random rand = new Random();


        //used for winning screen display
        Food winningFood = Food.DEFAULT;
        float winningMass = 0.0f;


        //GameState: Whatever state we are currently in
        //either the MENU, Game, or WIN state will do different UPDATES and DRAW methods
        GameState gameState = GameState.MENU;

        //MainMenuFont: when we draw Strings to the screen in the main menu, we will use this font
        //Loaded in LoadContent
        SpriteFont mainMenuFont;


        //we will begin pulling a "Player", "Garbage" and other game objects from here?
        List<GameObject> gameObjects = new List<GameObject>();

        //cool down boolean
        bool isEnterDown = false;

        /// <summary>
        /// Static keyboard state
        /// </summary>
        public static KeyboardState keyState;

        /// <summary>
        /// Static Mouse State for better player movement
        /// </summary>
        public static MouseState mState;

        /// <summary>
        /// true for keyboard Input, false for mouse Input
        /// </summary>
        public static bool keyboardInput = true;

        /// <summary>
        /// The area to keep the player contained to
        /// </summary>
        public static Rectangle bounds;

        //player sprite
        Texture2D[] playerAnim;
        //player
        Player player;

        public static float scale;

        /// <summary>
        /// 
        /// </summary>
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        #region background

        //used in drawing the background's rectangle
        Vector2 backgroundPipeVelocity = new Vector2();

        #endregion background

        #region Garbage

        const int NUM_GARBAGE_PIECES = 10;

        #endregion Garbage

        #region EMField

        const int NUM_EMFIELDS = 3;

        #endregion EMField

        #region Sounds

        SoundEffect backgroundMusic;
        SoundEffect garbageSound;
        SoundEffect swooshSound;


        SoundEffectInstance backgroundInstance;
        SoundEffectInstance garbageInstance;
        SoundEffectInstance swooshInstance;
        #endregion Sounds

        #region UI

        Vector2 needleOrigin = new Vector2(9, 26);
        float rotation = 135.0f;
        float variation = 0.0f;
        float variationSpeed = .05f;

        #endregion


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// 
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //for debugging purposes for now
            this.Window.AllowUserResizing = true;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            bounds = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            //Loading the Main Menu Font for ability to print strings for Menu as well as prototype UI
            mainMenuFont = Content.Load<SpriteFont>("Content/Fonts/MainMenuFont");

            #region player

            textures.Add("playerSprite0", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame1"));
            textures.Add("playerSprite1", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame2"));
            textures.Add("playerSprite2", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame3"));
            textures.Add("playerSprite3", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame4"));
            textures.Add("playerSprite4", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame5"));
            textures.Add("playerSprite5", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame6"));
            textures.Add("playerSprite6", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame7"));
            textures.Add("playerSprite7", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame8"));
            textures.Add("playerSprite8", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame9"));
            textures.Add("playerSprite9", Content.Load<Texture2D>("Content/Sprites/Player/playerFrame10"));

            scale =1.0f;
            //create player

            playerAnim = new Texture2D[] { textures["playerSprite0"], textures["playerSprite1"], textures["playerSprite2"], textures["playerSprite3"], textures["playerSprite4"], textures["playerSprite5"], textures["playerSprite6"], textures["playerSprite7"], textures["playerSprite8"], textures["playerSprite9"] };
            
            player = new Player(playerAnim, new Vector2(0.0f, 0.0f), 0.0f, playerAnim[0].Width/2 * scale, new Vector2(10.0f, 0.0f));
            gameObjects.Add(player);

            #endregion player

            #region background

            //using this cause apparently bounds is being dickish for placing items on the screen
            int screenX = graphics.GraphicsDevice.Viewport.Width;
            int screenY = graphics.GraphicsDevice.Viewport.Height;

            //set background to "beginning" (off screen to the right)
            backgroundPipeVelocity.X = 0;

            //backgound items
            textures.Add("backgroundPipeConnection", Content.Load<Texture2D>("Content/Sprites/insidePipe"));


            #region UI
            textures.Add("glass", Content.Load<Texture2D>("Content/Sprites/UI/glass"));
            textures.Add("needle", Content.Load<Texture2D>("Content/Sprites/UI/speedometerDial"));
            textures.Add("speedometer", Content.Load<Texture2D>("Content/Sprites/UI/speedometer"));
            textures.Add("UI", Content.Load<Texture2D>("Content/Sprites/UI/UI"));

            #region backgrounds
            textures.Add("mainMenuBackground", Content.Load<Texture2D>("Content/Sprites/UI/Menus/titleScreen"));
            #endregion backgrounds
            #endregion UI

            #region Sounds

            backgroundMusic = Content.Load<SoundEffect>("Content/Sounds/backgroundMusic");
            garbageSound = Content.Load<SoundEffect>("Content/Sounds/garbageSoundEffect");
            swooshSound = Content.Load<SoundEffect>("Content/Sounds/swoosh");

            backgroundInstance = backgroundMusic.CreateInstance();
            backgroundInstance.IsLooped = true;
            backgroundInstance.Volume = .25f;

            swooshInstance = swooshSound.CreateInstance();
            swooshInstance.Volume = .5f;

            garbageInstance = garbageSound.CreateInstance();
            garbageInstance.Volume = 1f;

            #endregion Sounds


            #region WinScreens
            textures.Add("garbagePlate", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/garbagePlateWinScreen"));
            textures.Add("cheeseburger", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/cheeseburgerWinScreen"));
            textures.Add("steak", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/steakWinScreen"));
            textures.Add("mac", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/macCheeseWinScreen"));
            textures.Add("cookie", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/cookieWinScreen"));
            textures.Add("doughnut", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/chocolateDonutWinScreen"));
            textures.Add("truffle", Content.Load<Texture2D>("Content/Sprites/UI/Menus/WinScreens/chocolateTruffleWinScreen"));
            #endregion WinScreens

            #region OtherScreens
            textures.Add("instructions", Content.Load<Texture2D>("Content/Sprites/UI/Menus/instructionsScreen"));
            textures.Add("backgroundScreen", Content.Load<Texture2D>("Content/Sprites/UI/Menus/backgroundScreen"));
            textures.Add("resources", Content.Load<Texture2D>("Content/Sprites/UI/Menus/Resources"));
            textures.Add("credits", Content.Load<Texture2D>("Content/Sprites/UI/Menus/creditsScreen"));
            #endregion OtherScreens

            #endregion background

            #region garbage

            //garbage pieces
            textures.Add("defaultGarbageTexture", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/Default"));


            textures.Add("cigarettTexture", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/cigarette"));
            textures.Add("cigarettTexture1", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/cigaretteFilter"));
            textures.Add("cigarettTexture2", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/ash"));

            textures.Add("metalTexture",Content.Load<Texture2D>("Content/Sprites/GarbageTypes/computer"));


            textures.Add("plasticTexture", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/plasticCup"));
            textures.Add("plasticTexture1", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/soccerCleets"));
            textures.Add("plasticTexture2", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/sodaPlasticRings"));



            textures.Add("styrofomeTexture", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/packingPeanut"));
            textures.Add("styrofomeTexture1", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/takeOutBox"));
            textures.Add("styrofomeTexture2", Content.Load<Texture2D>("Content/Sprites/GarbageTypes/styrofoamCup"));


            //Garbage temp;
            /*
             * When we are adding these garbage pieces we need to:
             * create random X
             * create random GarbageType
             * 
             * WHAT WILL THEY DO?!?!? (AI wise?)
             */
            /*for (int i = 0; i < numOfGarbagePieces; i++)
            {
                temp = new Garbage(textures["cigarettTexture"], new Vector2(rand.Next(screenX, screenX * 2), rand.Next(0, screenY)), 0.1f, textures["cigarettTexture"].Width / 2, GarbageType.PLASTIC);
                gameObjects.Add(temp);
            }*/

            #endregion garbage

            #region EMField

            //EMFIELD
            textures.Add("fieldPlaceholderSprite", Content.Load<Texture2D>("Content/Sprites/electricCloud"));

            //EMField tempField;
            /*
             * When we are adding these garbage pieces we need to:
             * create random X between screen width and 2xscreenwidth... we will need to keep creating these items over time in the game...
             */
            /*for (int i = 0; i < numOfFields; i++)
            {
                tempField = new EMField(textures["fieldPlaceholderSprite"], new Vector2(screenX + textures["fieldPlaceholderSprite"].Width * i, 10), 0.1f, textures["fieldPlaceholderSprite"].Width / 2);
                gameObjects.Add(tempField);
            }*/

            #endregion EMField


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            keyState = Keyboard.GetState();
            mState = Mouse.GetState();

            if (keyState.IsKeyUp(Keys.Enter))
            {
                isEnterDown = true;
            }

            if (mState.LeftButton.Equals(ButtonState.Released))
            {
                hunBool = true;
                cigBool = true;
                litBool = true;
            }

            switch (gameState)
            {
                case GameState.MENU: MenuUpdate(keyState); break;
                case GameState.GAME: GameUpdate(gameTime); break;
                case GameState.WIN: WinUpdate();  break;
                case GameState.BACKGROUND: BackgroundScreenUpdate(); break;
                case GameState.INSTRUCTIONS: InstructionsUpdate(); break;
                case GameState.RESOURCES: ResourcesUpdate(); break;
                case GameState.CREDITS: CreditsUpdate(); break;
            }


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void CreditsUpdate()
        {
            if (keyState.IsKeyDown(Keys.Enter) || mState.LeftButton.Equals(ButtonState.Pressed))
                changeGameState(GameState.MENU);


            #region background

            //move background
            backgroundPipeVelocity -= Player.Velocity;

            //if background is offscreen, reset it
            if (backgroundPipeVelocity.X < -graphics.GraphicsDevice.Viewport.Width)
                backgroundPipeVelocity.X = 0;

            #endregion background
        }

        private void ResourcesUpdate()
        {
            if (keyState.IsKeyDown(Keys.Enter))
                changeGameState(GameState.MENU);

            if (mState.LeftButton.Equals(ButtonState.Pressed) && hungerFundWebsiteRectangle.Contains(new Point(mState.X, mState.Y))&& hunBool)
            {
                System.Diagnostics.Process.Start("IEXPLORE.EXE", hungerFundWebsite);
                hunBool = false;
            }
            //goto hungerwebsite
            if (mState.LeftButton.Equals(ButtonState.Pressed) && cigarrettWebsiteRectangle.Contains(new Point(mState.X, mState.Y)) && cigBool)
            {
                System.Diagnostics.Process.Start("IEXPLORE.EXE", cigarettWebsite);
                cigBool = false;
            }
            //goto cigarett website
            if (mState.LeftButton.Equals(ButtonState.Pressed) && litterFactsWebsiteRectangle.Contains(new Point(mState.X, mState.Y))&& litBool)
            {
                System.Diagnostics.Process.Start("IEXPLORE.EXE", litterFactsWebsite);
                litBool = false;
            }
            //goto litter website
            #region background

            //move background
            backgroundPipeVelocity -= Player.Velocity;

            //if background is offscreen, reset it
            if (backgroundPipeVelocity.X < -graphics.GraphicsDevice.Viewport.Width)
                backgroundPipeVelocity.X = 0;

            #endregion background
        }

        private void InstructionsUpdate()
        {
            if (keyState.IsKeyDown(Keys.Enter) && isEnterDown)
            {
                changeGameState(GameState.MENU);
            }

            #region background

            //move background
            backgroundPipeVelocity -= Player.Velocity;

            //if background is offscreen, reset it
            if (backgroundPipeVelocity.X < -graphics.GraphicsDevice.Viewport.Width)
                backgroundPipeVelocity.X = 0;

            #endregion background
        }

        private void BackgroundScreenUpdate()
        {
            if (keyState.IsKeyDown(Keys.Enter) && isEnterDown)
            {
                changeGameState(GameState.MENU);
            }
            #region background

            //move background
            backgroundPipeVelocity -= Player.Velocity;

            //if background is offscreen, reset it
            if (backgroundPipeVelocity.X < -graphics.GraphicsDevice.Viewport.Width)
                backgroundPipeVelocity.X = 0;

            #endregion background
        }

        /// <summary>
        /// Update called from the win screen
        /// </summary>
        private void WinUpdate()
        {
            //Update and say "YO PRESS [insert key here] TO BE AWESOME/goto menu"
            if (keyState.IsKeyDown(Keys.Enter) && isEnterDown)
            {
                changeGameState(GameState.MENU);
            }
        }

        /// <summary>
        /// Update called from the game screen
        /// </summary>
        /// <param name="gameTime">The object keeping track of time in the game</param>
        private void GameUpdate(GameTime gameTime)
        {
            // Change the state if the enter key is down
            if (keyState.IsKeyDown(Keys.Enter) && isEnterDown)
                keyboardInput = true;

            if (mState.LeftButton.Equals(ButtonState.Pressed))
                keyboardInput = false;



           //Update the game here
            for (int i = 0; i < gameObjects.Count; i++ )
            {
                gameObjects[i].Update(gameTime);
                collisionCheck(gameObjects[i]);
                if (gameObjects[i].Dead)
                    gameObjects.RemoveAt(i);
            }
            //Check for spawn time
            //float multiplier = (Player.Velocity.Length() > 10.0f) ? (float)((Player.Velocity.Length() / 10.0f) / 5.0f) + 2.0f : 2.0f;
            if ((int) gameTime.TotalGameTime.TotalMilliseconds % (int) (5000) == 0)
            {
                for(int i =0; i<(int)(Player.Velocity.Length() > 10 ? Player.Velocity.Length()/10: 1); i++)
                    SpawnMoreGarbage();        

            }
            if (gameTime.TotalGameTime.Milliseconds % 1000 == 0 && gameTime.TotalGameTime.Seconds % 10 == 0)
            {
                SpawnEMFields();
            }
            #region background

            //move background
            backgroundPipeVelocity -= Player.Velocity;

            //if background is offscreen, reset it
            if (backgroundPipeVelocity.X < -graphics.GraphicsDevice.Viewport.Width)
                backgroundPipeVelocity.X = 0;

            #endregion background

            #region winCondition

            if (Player.Velocity.X < 0)
                changeGameState(GameState.WIN);

            #endregion winCondition
        }

        /// <summary>
        /// Creates a new Garbage object based on the percentage given
        /// </summary>
        /// <param name="percentage">The value that determines the type of garbage</param>
        /// <returns>A new Garbage object</returns>
        private Garbage CreateGarbage(int percentage)
        {
            float multiplier = 40.0f;
            Garbage temp = null;

            int screenX = graphics.GraphicsDevice.Viewport.Width;
            int screenY = graphics.GraphicsDevice.Viewport.Height;

            if (percentage < 30)
            {
                int texture = rand.Next(0, 3);
                switch(texture)
                {
                    case 0: temp = new Garbage(textures["styrofomeTexture"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 2.5f, textures["styrofomeTexture"].Width / 2, GarbageType.STYROPHOME); break;
                    case 1: temp = new Garbage(textures["styrofomeTexture1"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 2.5f, textures["styrofomeTexture1"].Width / 2, GarbageType.STYROPHOME); break;
                    case 2: temp = new Garbage(textures["styrofomeTexture2"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 2.5f, textures["styrofomeTexture2"].Width / 2, GarbageType.STYROPHOME); break;
                }
            }
            else if (percentage < 60)
            {
                int texture = rand.Next(0, 3);
                switch(texture)
                {
                    case 0: temp = new Garbage(textures["cigarettTexture"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 1.5f, textures["cigarettTexture"].Width / 2, GarbageType.CIGARETTEBUTTS); break;
                    case 1: temp = new Garbage(textures["cigarettTexture1"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 1.5f, textures["cigarettTexture1"].Width / 2, GarbageType.CIGARETTEBUTTS); break;
                    case 2: temp = new Garbage(textures["cigarettTexture2"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 1.5f, textures["cigarettTexture2"].Width / 2, GarbageType.CIGARETTEBUTTS); break;
                }
            }
            else if (percentage < 80)
            {
                temp = new Garbage(textures["metalTexture"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 4.5f, textures["metalTexture"].Width / 2, GarbageType.METAL);
            }
            else /*if (percentage < 100)*/
            {
                int texture = rand.Next(0, 3);
                switch(texture)
                {
                    case 0: temp = new Garbage(textures["plasticTexture"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 3.0f, textures["plasticTexture"].Width / 2, GarbageType.PLASTIC); break;
                    case 1: temp = new Garbage(textures["plasticTexture1"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 3.0f, textures["plasticTexture1"].Width / 2, GarbageType.PLASTIC); break;
                    case 2: temp = new Garbage(textures["plasticTexture2"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 3.0f, textures["plasticTexture2"].Width / 2, GarbageType.PLASTIC); break;
                }
            }
            

            return temp;
        }

        /// <summary>
        /// add more garbage to our game as time goes on
        /// </summary>
        private void SpawnMoreGarbage(int numToSpawn = NUM_GARBAGE_PIECES)
        {
            float multiplier = /*(Player.Velocity.Length() > 10.0f) ? (float)((Player.Velocity.Length() / 10.0f) / 5.0f) + 2.0f :*/ 40.0f;
            int screenX = graphics.GraphicsDevice.Viewport.Width;
            int screenY = graphics.GraphicsDevice.Viewport.Height;

            Garbage temp;
            
            // for however many garbage pieces on the screen
            for (int i = 0; i < numToSpawn; i++)
            {
                temp = CreateGarbage(rand.Next(0, 100));
                bool hitting = true;                        // used to tell if the object is overlapping another object

                // repeat cycle till not overlapping anything
                do
                {
                    hitting = false;

                    // for every game object on the screen
                    for (int j = 0; j < gameObjects.Count; j++)
                    {
                        // check intersection with a game object (might change to radial check)
                        if (/*temp.Bounds.Intersects(gameObjects[j].Bounds)*/Vector2.Distance(temp.Position, gameObjects[j].Position) < temp.Radius + gameObjects[j].Radius)
                        {
                            hitting = true;
                            j = gameObjects.Count;
                        }
                    }

                    // reposition the garbage if necessary
                    if (hitting)
                        temp.Position = new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY));
                }
                while (hitting);
                gameObjects.Add(temp);
                temp = null;
            }
        }

        /// <summary>
        /// add more fields to the game as time goes on
        /// </summary>
        private void SpawnEMFields(int numToSpawn = NUM_EMFIELDS)
        {
            float multiplier = (Player.Velocity.Length() > 10.0f) ? (float)((Player.Velocity.Length() / 10.0f) / 5.0f) + 2.0f : 2.0f;
            int screenX = graphics.GraphicsDevice.Viewport.Width;
            int screenY = graphics.GraphicsDevice.Viewport.Height;

            EMField temp;

            // for however many garbage pieces on the screen
            for (int i = 0; i < numToSpawn; i++)
            {
                //temp =  new EMField(textures["fieldPlaceholderSprite"], new Vector2(screenX + textures["fieldPlaceholderSprite"].Width * i, 10), 0.1f, textures["fieldPlaceholderSprite"].Width / 2);
                temp = new EMField(textures["fieldPlaceholderSprite"], new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY)), 0.1f, textures["fieldPlaceholderSprite"].Width / 2);
                bool hitting = true;                        // used to tell if the object is overlapping another object

                // repeat cycle till not overlapping anything
                do
                {
                    hitting = false;

                    // for every game object on the screen
                    for (int j = 0; j < gameObjects.Count; j++)
                    {
                        // check intersection with a game object (might change to radial check)
                        if (/*temp.Bounds.Intersects(gameObjects[j].Bounds)*/ Vector2.Distance(temp.Position, gameObjects[j].Position) < temp.Radius + gameObjects[j].Radius)
                        {
                            hitting = true;
                            j = gameObjects.Count;
                        }
                    }

                    // reposition the garbage if necessary
                    if (hitting)
                        temp.Position = new Vector2(rand.Next(screenX, (int)(screenX * multiplier)), rand.Next(0, screenY));
                }
                while (hitting);

                gameObjects.Add(temp);
                temp = null;
            }
        }

        /// <summary>6
        /// Despawns "dead" stuff
        /// </summary>
        private void Despawn(bool all = false)
        {
            for (int i = gameObjects.Count - 1; i >= 0; --i)
            {
                if (gameObjects[i].Dead || all)
                    gameObjects.RemoveAt(i);
            }
        }

        /// <summary>
        /// Remove the current objects from the screen and re create the beginning of the level
        /// </summary>
        private void removeObjects()
        {
            int screenX = graphics.GraphicsDevice.Viewport.Width;

            gameObjects = new List<GameObject>();
            SpawnMoreGarbage();
            SpawnEMFields();
            //EMField tempField;
            /*
             * When we are adding these garbage pieces we need to:
             * create random X between screen width and 2xscreenwidth... we will need to keep creating these items over time in the game...
             */
            /*for (int i = 0; i < NUM_EMFIELDS; i++)
            {
                tempField = new EMField(textures["fieldPlaceholderSprite"], new Vector2(screenX + textures["fieldPlaceholderSprite"].Width * i, 10), 0.1f, textures["fieldPlaceholderSprite"].Width / 2);
                gameObjects.Add(tempField);
            }*/
        }


        /*
        Update Function: MenuUpdate
         * Args:
         * kState - Keyboard state (what key is down at this moment?)
         */
        private void MenuUpdate(KeyboardState kState)
        {
            //if the user presses enter, go to game!
            if (kState.IsKeyDown(Keys.Enter) && isEnterDown)
            {
                keyboardInput = true;
                changeGameState(GameState.GAME);
            }

            if (kState.IsKeyDown(Keys.B))
                changeGameState(GameState.BACKGROUND);

            if (kState.IsKeyDown(Keys.I))
                changeGameState(GameState.INSTRUCTIONS);

            if (kState.IsKeyDown(Keys.R))
                changeGameState(GameState.RESOURCES);

            if (kState.IsKeyDown(Keys.C))
                changeGameState(GameState.CREDITS);

            if (mState.LeftButton.Equals(ButtonState.Pressed))
            {
                keyboardInput = false;
                changeGameState(GameState.GAME);
            }

            #region background

            //move background
            backgroundPipeVelocity -= Player.Velocity;

            //if background is offscreen, reset it
            if (backgroundPipeVelocity.X < -graphics.GraphicsDevice.Viewport.Width)
                backgroundPipeVelocity.X = 0;

            #endregion background
        }

        /// <summary>
        /// Changes the current state of the game
        /// </summary>
        /// <param name="state">The state to change to</param>
        private void changeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.MENU:
                    break;

                case GameState.BACKGROUND:
                    break;
                
                case GameState.INSTRUCTIONS:
                    break;

                case GameState.GAME:
                    backgroundInstance.Play();
                    player.SetPosition(new Vector2((bounds.Width / 2) - 300, bounds.Height / 2));

                    SpawnInitialEMFields();
                    SpawnMoreGarbage();

                    Player.Velocity = new Vector2(10.0f, 0.0f);
                    break;

                case GameState.WIN:
                    winningMass = player.Mass;
                    if (winningMass > 1000)
                        winningFood = Food.GARBAGEPLATE;
                    else if (winningMass > 800)
                        winningFood = Food.STEAK;
                    else if (winningMass > 600)
                        winningFood = Food.CHEESEBURGER;
                    else if (winningMass > 400)
                        winningFood = Food.MAC;
                    else if (winningMass > 200)
                        winningFood = Food.COOKIE;
                    else if (winningMass > 100)
                        winningFood = Food.DOUGHNUT;
                    else
                        winningFood = Food.TRUFFLE;

                    removeObjects();
                    scale = 1.0f;
                    player = new Player(playerAnim, new Vector2(0.0f, 0.0f), 0.0f, playerAnim[0].Width / 2 * scale, new Vector2(10.0f, 0.0f));
                    gameObjects.Add(player);
                    
                    break;

                case GameState.RESOURCES:
                    break;
            }
            gameState = state;
            isEnterDown = false;
        }

        private void SpawnInitialEMFields()
        {
            float multiplier = (Player.Velocity.Length() > 10.0f) ? (float)((Player.Velocity.Length() / 10.0f) / 5.0f) + 2.0f : 2.0f;
            int screenX = graphics.GraphicsDevice.Viewport.Width;
            int screenY = graphics.GraphicsDevice.Viewport.Height;

            EMField temp;
            // for however many garbage pieces on the screen
            for (int i = 0; i < NUM_EMFIELDS*3; i++)
            {
                temp = new EMField(textures["fieldPlaceholderSprite"], new Vector2(bounds.Width, textures["fieldPlaceholderSprite"].Width*i), 0.1f, textures["fieldPlaceholderSprite"].Width / 2);
                gameObjects.Add(temp);
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin();
            switch (gameState)
            {
                case GameState.MENU: MenuDraw(gameTime); break;
                case GameState.GAME: GameDraw(gameTime); break;
                case GameState.WIN: WinDraw(gameTime); break;
                case GameState.INSTRUCTIONS: InstructionsDraw(gameTime); break;
                case GameState.BACKGROUND: BackgroundDraw(gameTime); break;
                case GameState.RESOURCES: ResourcesDraw(gameTime); break;
                case GameState.CREDITS: CreditsDraw(gameTime); break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void CreditsDraw(GameTime gameTime)
        {
            spriteBatch.Draw(textures["backgroundPipeConnection"], new Vector2((int)backgroundPipeVelocity.X, (int)backgroundPipeVelocity.Y), Color.Gray);
            spriteBatch.Draw(textures["credits"], new Vector2(0, 0), Color.White);
        }

        private void ResourcesDraw(GameTime gameTime)
        {
            spriteBatch.Draw(textures["backgroundPipeConnection"], new Vector2((int)backgroundPipeVelocity.X, (int)backgroundPipeVelocity.Y), Color.Gray);
            spriteBatch.Draw(textures["resources"], new Vector2(0,0), Color.White);
        }

        /// <summary>
        /// Drawing the Background Screen
        /// </summary>
        /// <param name="gameTime">GAME TIME!</param>
        private void BackgroundDraw(GameTime gameTime)
        {
            spriteBatch.Draw(textures["backgroundPipeConnection"], new Vector2((int)backgroundPipeVelocity.X, (int)backgroundPipeVelocity.Y), Color.Gray);
            spriteBatch.Draw(textures["backgroundScreen"], new Vector2(), Color.White);
            
        }

        /// <summary>
        /// Draw the Instructions Screen
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        private void InstructionsDraw(GameTime gameTime)
        {
            spriteBatch.Draw(textures["backgroundPipeConnection"], new Vector2((int)backgroundPipeVelocity.X, (int)backgroundPipeVelocity.Y), Color.Gray);
            spriteBatch.Draw(textures["instructions"], new Vector2(), Color.White);
            
        }

        /// <summary>
        /// Draw called when in the win state
        /// </summary>
        /// <param name="gameTime">The change in time since the last update</param>
        private void WinDraw(GameTime gameTime)
        {
            //When the player wins, What will be drawn?
            float textXOrigin = 50;
            switch (winningFood)
            {
                case Food.GARBAGEPLATE:
                    spriteBatch.Draw(textures["garbagePlate"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["garbagePlate"].Width / textXOrigin, textures["garbagePlate"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);
                    break;
                case Food.CHEESEBURGER:
                    spriteBatch.Draw(textures["cheeseburger"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["cheeseburger"].Width / textXOrigin, textures["cheeseburger"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);
                    break;
                case Food.STEAK:
                    spriteBatch.Draw(textures["steak"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["steak"].Width / textXOrigin, textures["steak"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);
                    break;
                case Food.MAC:
                    spriteBatch.Draw(textures["mac"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["mac"].Width / textXOrigin, textures["mac"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);
                    break;
                case Food.TRUFFLE:
                    spriteBatch.Draw(textures["truffle"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["truffle"].Width / textXOrigin, textures["truffle"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);
                    break;
                case Food.DOUGHNUT:
                    spriteBatch.Draw(textures["doughnut"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["doughnut"].Width / textXOrigin, textures["doughnut"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);
                   
                    break;
                case Food.COOKIE:
                    spriteBatch.Draw(textures["cookie"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["cookie"].Width / textXOrigin, textures["cookie"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black);

                    break;
                default:
                    spriteBatch.Draw(textures["doughnut"], new Vector2(), Color.White);
                    spriteBatch.DrawString(mainMenuFont, "g: " + winningMass, new Vector2(textures["doughnut"].Width / textXOrigin, textures["doughnut"].Height - (int)(mainMenuFont.MeasureString("g: " + winningMass).Y * 1.5)), Color.Black); 
                    break;
            }
        }

        /// <summary>
        /// Draw called when in the game state
        /// </summary>
        /// <param name="gameTime">The change in time since the last update</param>
        private void GameDraw(GameTime gameTime)
        {
            //During the game, draw here

            GraphicsDevice.Clear(Color.LightGray);

            spriteBatch.Draw(textures["backgroundPipeConnection"], new Vector2((int)backgroundPipeVelocity.X, (int)backgroundPipeVelocity.Y), Color.Gray);

            for (int i = 0; i < gameObjects.Count; i++ )
            {
                gameObjects[i].Draw(spriteBatch, gameTime);
            }
            drawUI();
        }

        
        
        /// <summary>
        /// Draw UI items here:
        ///     speedometer
        ///     mass
        /// </summary>
        private void drawUI()
        {
            //spriteBatch.DrawString(mainMenuFont, "current mass: " + player.Mass + " \nc: " + Player.Velocity.X, new Vector2(), Color.Red);
            spriteBatch.Draw(textures["UI"], new Vector2(), Color.White);

            float percent = (Player.Velocity.Length() < 100.0f) ? Player.Velocity.Length() / 100.0f : 1.0f;
            float currRotation = 270.0f * percent;
            variation += variationSpeed;
            float currVar = (float)Math.Sin(variation*2)/10;
            Vector2 needlePos = new Vector2(16 + textures["glass"].Width / 2, 17 + textures["glass"].Width / 2);

            spriteBatch.Draw(textures["needle"], needlePos, null, /*Color.White*/new Color(255,255,255,50), MathHelper.ToRadians(currRotation+ rotation)  + currVar,
                needleOrigin, new Vector2(1,1), SpriteEffects.None, 0);
            spriteBatch.Draw(textures["glass"], new Vector2(16,17), Color.White);
            spriteBatch.DrawString(mainMenuFont, "g: " + player.Mass, new Vector2(textures["UI"].Width/2+10, 12), Color.Black);
        }


        /// <summary>
        /// Handle Collisions here
        /// if you collide with garbage:
        /// 1. add mass
        /// 2. decrease speed
        /// 3. flag garbage for removal
        /// 
        /// if you collide with acceleration field
        /// 1. lose mass?
        /// 2. speed up
        /// 
        /// </summary>
        /// <param name="gObj"></param>
        private void collisionCheck(GameObject gObj)
        {
            if ((gObj is Garbage))//if we are colliding with Garbage
            {
                if (Vector2.Distance(player.Position, gObj.Position) < player.Radius + gObj.Radius)
                {
                    garbageInstance.Play();
                    Player.Velocity -= new Vector2(gObj.Mass, 0.0f);
                    player.Mass += gObj.Mass;
                    scale += .01f;
                    player.Radius += player.Sprite.Width*.005f;
                    player.Bounds = new Rectangle((int) (player.Position.X - player.Radius), (int) (player.Position.Y - player.Radius), (int) (player.Radius * 2),(int) player.Radius * 2);
                    //We may need to flag a collision and remove the object from gameObjects later...
                    //gameObjects.Remove(gObj);
                    gObj.Dead = true;
                }
              
            }
            if ((gObj is EMField))
            {
                if (Vector2.Distance(player.Position, gObj.Position) < player.Radius + gObj.Radius)
                {
                    
                    //Multiply gObj.Mass here to make player accelerate faster... divide otherwise...
                    Player.Velocity += Player.Velocity.X < 100 ? new Vector2(gObj.Mass*100, 0.0f) : new Vector2(0.0f, 0.0f);

                    for (int i = 1; i < gameObjects.Count; i++)
                    {
                        if (gameObjects[i].Bounds.Intersects(bounds))
                        {
                            gameObjects[i].Velocity -= (Player.Velocity.X < 100) ? new Vector2(gObj.Mass * 100, 0.0f) : Vector2.Zero;
                            swooshInstance.Play();
                        }
                    }


                    //for now (just for possible balance?) take away "mass" of EMField (so the faster you go, you will lose mass... not a lot of mass, but some)
                    //player.Mass -= gObj.Mass;
                }
            }
        }


        /// <summary>
        /// Draw called when in the menu state
        /// </summary>
        /// <param name="gameTime">The change in time since the last update</param>
        private void MenuDraw(GameTime gameTime)
        {
            //Draw menu instructions here
            //FOR REBECCA: draw an instructions screen
            //depict up and down arrow keys to move the player

            //give instructions also on what the fuck to do...
            

            //the -100 gets approximately centered text (due to string size starting from the middle it will look off center)
            spriteBatch.Draw(textures["backgroundPipeConnection"], new Vector2((int)backgroundPipeVelocity.X, (int)backgroundPipeVelocity.Y), Color.Gray);
            spriteBatch.Draw(textures["mainMenuBackground"], new Vector2(0,0), Color.White);
            //spriteBatch.DrawString(mainMenuFont, "PRESS ENTER TO PLAY!", new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - mainMenuFont.MeasureString("PRESS ENTER TO PLAY!").X / 2, graphics.GraphicsDevice.Viewport.Height / 2), Color.White);
        }
    }
}
