﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TDGame.Managers;

namespace TDGame {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private HealthBar _hp_root;

        private int HEIGTH = 1080;
        private int WIDTH = 1920;

        /****************************/
        /*********BACKGROUND*********/
        /****************************/
        private Texture2D _background_texture;
        private Rectangle _map_location;

        /****************************/
        /********FONT SPRITES********/
        /****************************/
        private SpriteFont _damage_font;

        /****************************/
        /**********MANAGERS**********/
        /****************************/
        private Dictionary<string, EnemyManager> _enemy_managers;


        private BulletManager _bullet_manager;
        private Player _player;
        
        private BulletHitsEnemy _bullet_hits_enemy;
        private EnergyStorageManager _energy_storage_manager;
        private PortalManager _portal_manager;
        private WaveManager _wave_manager;

        /****************************/
        /***********TIMERS***********/
        /****************************/
        private double _fire_bullet_time = 0;


        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = System.TimeSpan.FromSeconds(1d / 30d);
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGTH;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            _enemy_managers = new Dictionary<string, EnemyManager>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Rectangle win_size = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            _background_texture = Content.Load<Texture2D>("map");
            _map_location = new Rectangle(0, 0, WIDTH, HEIGTH);
            _damage_font = Content.Load<SpriteFont>("DamageFont");

            _hp_root = new HealthBar(
                Content.Load<Texture2D>("hp_50"),
                50,
                4
            );

            /********************************************************/
            /********************INITIALIZE PORTALS******************/
            /********************************************************/

            List<Vector2> portal_location = new List<Vector2>() {
                new Vector2(550, 60),
                new Vector2(1700, 400),
                new Vector2(100, 750),
                new Vector2(800, 850)
            };

            _portal_manager = new PortalManager(
                Content.Load<Texture2D>("portal_vecji"),
                133,
                150
            );

            foreach (var a in portal_location)
                _portal_manager.AddPortal(a);

            /********************************************************/
            /*****************INITIALIZE ENERGY POINTS***************/
            /********************************************************/

            List<Vector2> energy_point_locations = new List<Vector2>() {
                new Vector2(270, 450),
                new Vector2(1050, 550)
            };

            _energy_storage_manager = new EnergyStorageManager(
                Content.Load<Texture2D>("energy_point"),
                100,
                100
            );

            foreach(var a in energy_point_locations)
                _energy_storage_manager.Add(a, 1000);


            /********************************************************/
            /********************INITIALIZE ENEMIES******************/
            /********************************************************/

            _enemy_managers.Add("enemy1", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager, 
                Content.Load<Texture2D>("enemy_1"),
                _hp_root, 48, 50, 30, 150, 7, 4
            ));

            _enemy_managers.Add("enemy2", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                Content.Load<Texture2D>("enemy_2"),
                _hp_root, 50, 50, 30, 200, 8, 5
            ));

            _enemy_managers.Add("enemy3", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                Content.Load<Texture2D>("enemy_3"),
                _hp_root, 104, 110, 30, 500, 2, 2
            ));

            _enemy_managers.Add("enemy4", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                Content.Load<Texture2D>("enemy_4"),
                _hp_root, 55, 100, 30, 300, 8, 5
            ));

            /********************************************************/
            /********************INITIALIZE BULLETS******************/
            /********************************************************/

            _bullet_manager = new BulletManager(
                Content.Load<Texture2D>("okrogel_metek6"), 
                win_size
            );

            /********************************************************/
            /********************INITIALIZE PLAYER*******************/
            /********************************************************/

            _player = new Player(
                Content.Load<Texture2D>("player"), 
                win_size, 
                new Vector2(700, 500), 
                60, 
                50, 
                40
            );

            /********************************************************/
            /***************INITIALIZE BULLET HITS ENEMY*************/
            /********************************************************/

            _bullet_hits_enemy = new BulletHitsEnemy(
                ref _enemy_managers, 
                ref _bullet_manager,
                ref _player
            );

            /********************************************************/
            /******************INITIALIZE WAVE MANAGER***************/
            /********************************************************/

            _wave_manager = new WaveManager(ref _enemy_managers, 5000, 2000);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            _fire_bullet_time += gameTime.ElapsedGameTime.TotalMilliseconds;

            var ks = Keyboard.GetState();
            var ms = Mouse.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            //fire bullet logic
            if (ms.LeftButton == ButtonState.Pressed && _fire_bullet_time > 180) { 
                _bullet_manager.Add(_player.GetPosition(), _player.GetDirection());
                _fire_bullet_time = 0;
            }


            /***********************************/
            /***************UPDATES*************/
            /***********************************/
            _wave_manager           .Update(gameTime);
            _player                 .Update(ks, ms);
            _bullet_manager         .Update();
            _bullet_hits_enemy      .Update();
            _portal_manager         .Update();
            _energy_storage_manager .Update();
            _enemy_managers.Values
                .ToList()
                .ForEach(x => x     .Update());



            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch             .Draw(_background_texture, _map_location, Color.White);
            _portal_manager         .Draw(spriteBatch);
            _bullet_manager         .Draw(spriteBatch);
            _enemy_managers.Values
                .ToList()
                .ForEach(x => x     .Draw(spriteBatch));
            _player                 .Draw(spriteBatch);
            _energy_storage_manager .Draw(spriteBatch);
            _bullet_hits_enemy      .Draw(spriteBatch, _damage_font);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}