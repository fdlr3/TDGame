using Microsoft.Xna.Framework;
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

        private Texture2D _background_texture;
        private Rectangle _map_location;

        private SpriteFont _damage_font;

        private int HEIGTH = 1080;
        private int WIDTH = 1920;

        private BulletManager _bullet_manager;
        private Player _player;
        private EnemyManager _enemy_manager;
        private BulletHitsEnemy _bullet_hits_enemy;
        private EnergyStorageManager _energy_storage_manager;
        private PortalManager _portal_manager;


        private double _el_time = 0;

        private double _el_spawn_enemy = 0;

        private List<Vector2> portal_location = new List<Vector2>() {
            new Vector2(200, 20),
            new Vector2(500, 20),
            new Vector2(800, 20),

            new Vector2(200, 700),
            new Vector2(500, 700),
            new Vector2(800, 700),

            new Vector2(20, 300),
            new Vector2(1000, 300)
        };

        private List<Vector2> energy_portal_locations= new List<Vector2>() {
            new Vector2(350, 300),
            new Vector2(750, 300)
        };

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = System.TimeSpan.FromSeconds(1d / 30d); //60);

            this.IsMouseVisible = true;
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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _background_texture = Content.Load<Texture2D>("map");
            _map_location = new Rectangle(0, 0, WIDTH, HEIGTH);

            /********************************************************/
            /********************INITIALIZE PORTALS******************/
            /********************************************************/

            List<Vector2> portal_location = new List<Vector2>() {
                new Vector2(750, 60),
                new Vector2(1700, 400),
                new Vector2(100, 750),
                new Vector2(800, 850)
            };

            _damage_font = Content.Load<SpriteFont>("DamageFont");


            Rectangle win_size = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            _energy_storage_manager = new EnergyStorageManager(
                Content.Load<Texture2D>("portal_vecji"),
                256,
                144
            );
            foreach(var a in energy_portal_locations)
                _energy_storage_manager.Add(a, 1000);

            _enemy_manager = new EnemyManager(
                portal_location,
                ref _energy_storage_manager, 
                Content.Load<Texture2D>("violet_robot_animated") 
            );

            _bullet_manager = new BulletManager(
                Content.Load<Texture2D>("okrogel_metek6"), 
                win_size
            );

            _player = new Player(
                Content.Load<Texture2D>("player"), 
                win_size, 
                new Vector2(WIDTH / 2, HEIGTH / 2), 
                60, 
                50, 
                40
            );

            _bullet_hits_enemy = new BulletHitsEnemy(
                ref _enemy_manager, 
                ref _bullet_manager,
                ref _player
            );

            _portal_manager = new PortalManager(
                Content.Load<Texture2D>("portal_vecji"), 
                133,
                150
            );

            foreach(var a in portal_location) {
                _portal_manager.AddPortal(a);
            }



            //bhe = new BulletHitsEnemy(ref Enemy._enemies, ref Bullet._bullets);
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
            var ks = Keyboard.GetState();
            var ms = Mouse.GetState();

            if (ks.IsKeyDown(Keys.Escape))
                Exit();

            //fire bullet logic
            _el_time += gameTime.ElapsedGameTime.TotalMilliseconds;
            _el_spawn_enemy += gameTime.ElapsedGameTime.TotalMilliseconds;
           
            if (ms.LeftButton == ButtonState.Pressed && _el_time > 180) { 
                _bullet_manager.Add(_player.GetPosition(), _player.GetDirection());
                _el_time = 0;
            }
                

            if(_el_spawn_enemy > 1000) {
                _enemy_manager.Add();
                _el_spawn_enemy = 0;
            }


            _player.Update(ks, ms);
            _bullet_manager.Update();
            _enemy_manager.Update();
            _bullet_hits_enemy.Update();
            _portal_manager.Update();
            //Debug.WriteLine($"M: ({ms.X}, {ms.Y}) -> BC: {Bullet._bullets.Count} -> Tick: {_tick}");
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(_background_texture, _map_location, Color.White);
            _portal_manager.Draw(spriteBatch);
            _bullet_manager.Draw(spriteBatch);
            _enemy_manager.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            _energy_storage_manager.Draw(spriteBatch);
            _bullet_hits_enemy.Draw(spriteBatch, _damage_font);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}