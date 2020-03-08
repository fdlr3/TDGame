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

    

        private BulletManager _bullet_manager;
        private Player _player;
        private EnemyManager _enemy_manager;
        private BulletHitsEnemy _bullet_hits_enemy;
        private EnergyStorageManager _energy_storage_manager;

        private double _el_time = 0;

        private double _el_spawn_enemy = 0;

        private List<Vector2> vec1 = new List<Vector2>() {
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
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
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

            Rectangle win_size = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            _energy_storage_manager = new EnergyStorageManager(
                Content.Load<Texture2D>("portal"),
                256,
                144
            );
            foreach(var a in energy_portal_locations)
                _energy_storage_manager.Add(a, 1000);

            _enemy_manager = new EnemyManager(
                vec1,
                ref _energy_storage_manager, 
                Content.Load<Texture2D>("Enemy")
            );

            _bullet_manager = new BulletManager(
                Content.Load<Texture2D>("CustomBullet"), 
                win_size
            );

            _player = new Player(
                Content.Load<Texture2D>("playa"), 
                win_size, 
                new Vector2(0, 0), 
                70, 
                70, 
                40
            );

            _bullet_hits_enemy = new BulletHitsEnemy(
                ref _enemy_manager, 
                ref _bullet_manager,
                ref _player
            );

            

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

            //fire bullet logic
            _el_time += gameTime.ElapsedGameTime.TotalMilliseconds;
            _el_spawn_enemy += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_el_time > 200) {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed) { //Keyboard.GetState().IsKeyDown(Keys.Space)
                    _bullet_manager.Add(_player.GetPosition(), _player.GetDirection());
                }
                _el_time = 0;
            }

            if(_el_spawn_enemy > 1000) {
                _enemy_manager.Add();
                _el_spawn_enemy = 0;
            }


            _player.Update(Keyboard.GetState(), Mouse.GetState());
            _bullet_manager.Update();
            _enemy_manager.Update();
            _bullet_hits_enemy.Update();
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
            _bullet_manager.Draw(spriteBatch);
            _enemy_manager.Draw(spriteBatch);
            _player.Draw(spriteBatch);
            _energy_storage_manager.Draw(spriteBatch);
            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}