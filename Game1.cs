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

        private HealthBar _hp50_root;
        private HealthBar _hp250_root;

        private int HEIGTH = 1080;
        private int WIDTH = 1920;

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
        private BackgroundManager _background_manager;

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
            _damage_font = Content.Load<SpriteFont>("DamageFont");

            /********************************************************/
            /********************INITIALIZE HEALTHBAR****************/
            /********************************************************/

            _hp50_root = new HealthBar(
                Content.Load<Texture2D>("hp_50"),
                50,
                4
            );

            _hp250_root = new HealthBar(
                Content.Load<Texture2D>("hp_250"),
                250,
                6
            );

            /********************************************************/
            /*******************INITIALIZE BACKGROUND****************/
            /********************************************************/

            _background_manager = new BackgroundManager
                (
                    Content.Load<SpriteFont>("hs_wave"), 
                    new Vector2(1680, 75), 
                    new Vector2(1770, 28)
                );

            _background_manager.AddTexture
                (
                    Content.Load<Texture2D>("map"),
                    new Vector2(0, 0)
                );
            _background_manager.AddTexture
                (
                    Content.Load<Texture2D>("highscore_wave"),
                    new Vector2(WIDTH - 360, 0)
                );
            _background_manager.AddTexture
               (
                   Content.Load<Texture2D>("left_energy_hp"),
                   new Vector2(WIDTH / 4, 0)
               );
            _background_manager.AddTexture
               (
                   Content.Load<Texture2D>("right_energy_hp"),
                   new Vector2(WIDTH / 4 * 2, 0)
               );

            /********************************************************/
            /********************INITIALIZE PORTALS******************/
            /********************************************************/

            List<Vector2> portal_location = new List<Vector2>() {
                new Vector2(700, 60),
                new Vector2(1750, 300),
                new Vector2(80, 850),
                new Vector2(750, 850)
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

            _energy_storage_manager = new EnergyStorageManager(
                Content.Load<Texture2D>("energy_point"),
                214,
                200, 
                _hp250_root
            );

            _energy_storage_manager.Add
                (
                new Vector2(250, 300), 
                new Vector2((WIDTH / 4) + 35, 40), 
                2000
                );

            _energy_storage_manager.Add
                (
                new Vector2(1000, 500), 
                new Vector2((WIDTH / 4 * 2) + 35, 40), 
                2000
                );

            /********************************************************/
            /********************INITIALIZE ENEMIES******************/
            /********************************************************/

            _enemy_managers.Add("enemy1", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager, 
                Content.Load<Texture2D>("enemy_1"),
                _hp50_root, 48, 50, 30, 150, 7, 4
            ));

            _enemy_managers.Add("enemy2", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                Content.Load<Texture2D>("enemy_2"),
                _hp50_root, 50, 50, 30, 200, 8, 5
            ));

            _enemy_managers.Add("enemy3", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                Content.Load<Texture2D>("enemy_3"),
                _hp50_root, 104, 110, 30, 500, 2, 2
            ));

            _enemy_managers.Add("enemy4", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                Content.Load<Texture2D>("enemy_4"),
                _hp50_root, 55, 100, 30, 300, 8, 5
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

            _wave_manager = new WaveManager(ref _enemy_managers, 5000, 1000);

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
            int Highscore = 0;

            var ks = Keyboard.GetState();
            var ms = Mouse.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            //fire bullet logic
            if (ms.LeftButton == ButtonState.Pressed && _fire_bullet_time > 180) { 
                _bullet_manager.Add(_player.GetPosition(), _player.GetDirection());
                _fire_bullet_time = 0;
            }

            //GAME OVER
            if(_energy_storage_manager._energy_storages.Count == 0) {
                Exit();
            }


            /***********************************/
            /***************UPDATES*************/
            /***********************************/
            
            _player                 .Update(ks, ms);
            _background_manager     .Update(_wave_manager.WaveCount, _wave_manager.Highscore);
            _bullet_manager         .Update();
            _bullet_hits_enemy      .Update();
            _portal_manager         .Update();
            _energy_storage_manager .Update();
            _enemy_managers.Values
                .ToList()
                .ForEach(x => x     .Update(ref Highscore, gameTime));
            _wave_manager           .Update(gameTime, ref Highscore);

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            _background_manager     .Draw(spriteBatch);
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