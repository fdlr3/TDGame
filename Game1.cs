using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TDGame.Managers;

namespace TDGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum CGameState { MainScreen, StartGame, Playing, Gameover, Quit, CreditsScreen };

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public CGameState _state = CGameState.MainScreen;

        /****************************/
        /***********WINDOW***********/
        /****************************/
        private int HEIGTH = 1080;
        private int WIDTH = 1920;
        private Rectangle win_size;

        /****************************/
        /*******TEXTURE STORAGE******/
        /****************************/
        private Dictionary<string, Texture2D> _tstorage;
        private Dictionary<string, SpriteFont> _fstorage;

        /****************************/
        /**********MANAGERS**********/
        /****************************/
        private GameOverScreen _gameoverscreen;
        private MainScreen _mainscreen;
        private HealthBar _hp50_root;
        private HealthBar _hp250_root;
        private Dictionary<string, EnemyManager> _enemy_managers;
        private BulletManager _bullet_manager;
        private Player _player;
        private BulletHitsEnemy _bullet_hits_enemy;
        private EnergyStorageManager _energy_storage_manager;
        private PortalManager _portal_manager;
        private WaveManager _wave_manager;
        private BackgroundManager _background_manager;
        private PowerupManager _powerup_manager;
        private CreditsScreen _creditsScreen;

        /****************************/
        /***********TIMERS***********/
        /****************************/
        private double _fire_bullet_time = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsFixedTimeStep = true;
            TargetElapsedTime = System.TimeSpan.FromSeconds(1d / 30d);
            IsMouseVisible = true;
        }

        public void LoadCreditsScreen()
        {
            _creditsScreen = null;

            _creditsScreen = new CreditsScreen();

            _creditsScreen.AddTexture
               (
                   _tstorage["ozadje"],
                   new Vector2(0, 0)
               );

            _creditsScreen.AddText
                (
                    _fstorage["RetroFont"],
                    new Vector2(670, 100),
                    "CREDITS"
                );

            _creditsScreen.AddText
                (
                    _fstorage["font"],
                    new Vector2(795, 300),
                    "Masa Hober - Design"
                );

            _creditsScreen.AddText
                (
                    _fstorage["font"],
                    new Vector2(720, 350),
                    "Filip Duler - Lead Programmer"
                );

            _creditsScreen.AddText
                (
                    _fstorage["font"],
                    new Vector2(840, 400),
                    "Teja Kolar - UI"
                );

            _creditsScreen.AddText
                (
                    _fstorage["font"],
                    new Vector2(920, 600),
                    "FERI"
                );

            _creditsScreen.AddText
                (
                    _fstorage["font"],
                    new Vector2(795, 700),
                    "Maribor, marec 2020"
                );

            _creditsScreen.AddButton(new Button
                (
                    _tstorage["exitpng"],
                    new Vector2(760, 800),
                    400, 110,
                    CGameState.MainScreen
                ));
        }

        public void LoadMainScreen()
        {
            _mainscreen = null;
            _gameoverscreen = null;

            _mainscreen = new MainScreen();
            _mainscreen.AddTitle(_tstorage["background_intro"], new Vector2(450, 100), 2, 1024, 600);

            _mainscreen.AddButton(new Button
                (
                    _tstorage["playpng"],
                    new Vector2(760, 500),
                    400, 110,
                    CGameState.StartGame
                ));

            _mainscreen.AddButton(new Button
                (
                    _tstorage["exitpng"],
                    new Vector2(760, 650),
                    400, 110,
                    CGameState.Quit
                ));

            _mainscreen.AddButton(new Button
                (
                    _tstorage["credits"],
                    new Vector2(760, 800),
                    400, 110,
                    CGameState.CreditsScreen
                ));

        }

        public void LoadGameOver(int highscore)
        {
            _gameoverscreen = null;

            _gameoverscreen = new GameOverScreen();

            _gameoverscreen.AddTexture
                 (
                     _tstorage["map"],
                     new Vector2(0, 0)
                 );

            _gameoverscreen.AddButton(new Button
                (
                    _tstorage["exit2"],
                    new Vector2(1020, 700),
                    400, 110,
                    CGameState.Quit
                ));

            _gameoverscreen.AddButton(new Button
                (
                    _tstorage["tryagain"],
                    new Vector2(500, 698),
                    400, 110,
                    CGameState.StartGame
                ));

            _gameoverscreen.AddTexture
               (
                   _tstorage["highscore"],
                   new Vector2(555, 500)
               );

            _gameoverscreen.AddTexture
               (
                   _tstorage["gameover"],
                   new Vector2(560, 200)
               );

            _gameoverscreen.AddText
                (
                    _fstorage["gameover_font"],
                    new Vector2(990, 510),
                    highscore.ToString()
                );
        }

        public void LoadGame()
        {
            _enemy_managers.Clear();
            _gameoverscreen = null;
            _hp50_root = null;
            _hp250_root = null;
            _bullet_manager = null;
            _player = null;
            _bullet_hits_enemy = null;
            _energy_storage_manager = null;
            _portal_manager = null;
            _wave_manager = null;
            _background_manager = null;
            _powerup_manager = null;

            #region HealthBar LoadContent
            _hp50_root = new HealthBar(
                _tstorage["hp_50"],
                50,
                4
            );

            _hp250_root = new HealthBar(
                _tstorage["hp_250"],
                250,
                6
            );
            #endregion

            #region BackgroundManager LoadContent
            _background_manager = new BackgroundManager
                (
                    _fstorage["hs_wave"],
                    new Vector2(1680, 72),
                    new Vector2(1770, 23)
                );

            _background_manager.AddTexture
                (
                    _tstorage["map"],
                    new Vector2(0, 0)
                );
            _background_manager.AddTexture
                (
                    _tstorage["highscore_wave"],
                    new Vector2(WIDTH - 360, 0)
                );
            _background_manager.AddTexture
               (
                   _tstorage["left_energy_hp"],
                   new Vector2(WIDTH / 4, 0)
               );
            _background_manager.AddTexture
               (
                   _tstorage["right_energy_hp"],
                   new Vector2(WIDTH / 4 * 2, 0)
               );
            #endregion

            #region MainScreen PortalManager
            List<Vector2> portal_location = new List<Vector2>() {
                new Vector2(700, 60),
                new Vector2(1750, 300),
                new Vector2(80, 850),
                new Vector2(750, 850)
            };

            _portal_manager = new PortalManager(
                _tstorage["portal_vecji"],
                133,
                150
            );

            foreach (var a in portal_location)
                _portal_manager.AddPortal(a);
            #endregion

            #region EnergyStorageManager LoadContent
            _energy_storage_manager = new EnergyStorageManager(
                _tstorage["energy_point"],
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
            #endregion

            #region EnemyManager LoadContent
            _enemy_managers.Add("enemy1", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                _tstorage["enemy_1"],
                _hp50_root, 48, 50, 30, 150, 7, 4
            ));

            _enemy_managers.Add("enemy2", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                _tstorage["enemy_2"],
                _hp50_root, 50, 50, 30, 200, 8, 5
            ));

            _enemy_managers.Add("enemy3", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                _tstorage["enemy_3"],
                _hp50_root, 104, 110, 30, 500, 2, 2
            ));

            _enemy_managers.Add("enemy4", new EnemyManager(
                ref _portal_manager,
                ref _energy_storage_manager,
                _tstorage["enemy_4"],
                _hp50_root, 55, 100, 30, 300, 8, 5
            ));
            #endregion

            #region BulletManager LoadContent
            _bullet_manager = new BulletManager(
                _tstorage["okrogel_metek6"],
                win_size
            );
            #endregion

            #region Player LoadContent
            _player = new Player(
                _tstorage["player"],
                win_size,
                new Vector2(700, 500),
                60,
                50,
                40
            );
            #endregion

            #region BulletHitsEnemy LoadContent
            _bullet_hits_enemy = new BulletHitsEnemy(
                ref _enemy_managers,
                ref _bullet_manager,
                ref _player,
                _fstorage["DamageFont"],
                _tstorage["mini_explosion"]
            );
            #endregion

            #region WaveManager LoadContent
            _wave_manager = new WaveManager(ref _enemy_managers, 5000, 1000);
            #endregion

            #region PowerupManager LoadContent
            List<Rectangle> spawn_locs = new List<Rectangle>()
            {
                new Rectangle(100, 100, 250, 150),
                new Rectangle(100, 600, 800, 200),
                new Rectangle(900, 200, 600, 250),
                new Rectangle(1300, 700, 400, 200)
            };

            _powerup_manager = new PowerupManager(10000, spawn_locs);
            _powerup_manager.AddPowerup
                (
                _tstorage["damage_pu"],
                34,
                50,
                10,
                PowerupManager.PowerUpType.PUT_PWR
                );
            _powerup_manager.AddPowerup
                (
                _tstorage["health_pu"],
                35,
                50,
                8,
                PowerupManager.PowerUpType.PUT_HP
                );
            #endregion

            _state = CGameState.Playing;
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
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGTH;
            graphics.IsFullScreen = false;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            win_size = new Rectangle(0, 0, WIDTH, HEIGTH);
            _enemy_managers = new Dictionary<string, EnemyManager>();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _tstorage = new Dictionary<string, Texture2D>()
            {
                { "enemy_1",            Content.Load<Texture2D>("enemy_1") },
                { "enemy_2",            Content.Load<Texture2D>("enemy_2") },
                { "enemy_3",            Content.Load<Texture2D>("enemy_3") },
                { "enemy_4",            Content.Load<Texture2D>("enemy_4") },
                { "energy_point",       Content.Load<Texture2D>("energy_point") },
                { "exit",               Content.Load<Texture2D>("exit") },
                { "explosion_1",        Content.Load<Texture2D>("explosion_1") },
                { "highscore_wave",     Content.Load<Texture2D>("highscore_wave") },
                { "hp_250",             Content.Load<Texture2D>("hp_250") },
                { "hp_50",              Content.Load<Texture2D>("hp_50") },
                { "left_energy_hp",     Content.Load<Texture2D>("left_energy_hp") },
                { "map",                Content.Load<Texture2D>("map") },
                { "mini_explosion",     Content.Load<Texture2D>("mini_explosion") },
                { "okrogel_metek6",     Content.Load<Texture2D>("okrogel_metek6") },
                { "play",               Content.Load<Texture2D>("play") },
                { "player",             Content.Load<Texture2D>("player") },
                { "portal_vecji",       Content.Load<Texture2D>("portal_vecji") },
                { "right_energy_hp",    Content.Load<Texture2D>("right_energy_hp") },
                { "title",              Content.Load<Texture2D>("title") },
                { "damage_pu",          Content.Load<Texture2D>("damage_pu") },
                { "health_pu",          Content.Load<Texture2D>("health_pu") },
                { "playpng",            Content.Load<Texture2D>("playpng") },
                { "exitpng",            Content.Load<Texture2D>("exitpng") },
                { "exit2",              Content.Load<Texture2D>("exit2") },
                { "tryagain",           Content.Load<Texture2D>("tryagain") },
                { "highscore",          Content.Load<Texture2D>("highscore") },
                { "gameover",           Content.Load<Texture2D>("gameover") },
                { "naslov",             Content.Load<Texture2D>("naslov") },
                { "credits",            Content.Load<Texture2D>("credits") },
                { "ozadje",             Content.Load<Texture2D>("ozadje") },
                { "background_intro",   Content.Load<Texture2D>("background_intro") }
            };

            _fstorage = new Dictionary<string, SpriteFont>()
            {
                { "DamageFont",     Content.Load<SpriteFont>("DamageFont") },
                { "hs_wave",        Content.Load<SpriteFont>("hs_wave") },
                { "gameover_font",  Content.Load<SpriteFont>("gameover_font") },
                { "font",           Content.Load<SpriteFont>("font") },
                { "FontCredits",    Content.Load<SpriteFont>("FontCredits") },
                { "RetroFont",      Content.Load<SpriteFont>("RetroFont") }
            };

            LoadMainScreen();
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
            _fire_bullet_time += gameTime.ElapsedGameTime.TotalMilliseconds;
            int Highscore = 0;

            var ks = Keyboard.GetState();
            var ms = Mouse.GetState();

        hack:
            switch (_state)
            {
                case CGameState.MainScreen:
                    var ret = _mainscreen.Update(ms);
                    if (ret.HasValue && (ret.Value == CGameState.Quit || ret.Value == CGameState.StartGame || ret.Value == CGameState.CreditsScreen))
                    {
                        _state = ret.Value;
                        goto hack;
                    }
                    break;

                case CGameState.CreditsScreen:
                    if (_creditsScreen == null)
                    {
                        LoadCreditsScreen();
                    }

                    ret = _creditsScreen.Update(ms);
                    if (ret.HasValue && ret.Value == CGameState.MainScreen)
                    {
                        _state = ret.Value;
                        goto hack;
                    }
                    break;

                case CGameState.StartGame:
                    LoadGame();
                    break;

                case CGameState.Playing:
                    if (ms.LeftButton == ButtonState.Pressed && _fire_bullet_time > 180)
                    {
                        _bullet_manager.Add(_player.GetPosition(), _player.GetDirection());
                        _fire_bullet_time = 0;
                    }

                    _player                 .Update(ks, ms);
                    _background_manager     .Update(_wave_manager.WaveCount, _wave_manager.Highscore);
                    _bullet_manager         .Update();
                    _bullet_hits_enemy      .Update();
                    _portal_manager         .Update();
                    _energy_storage_manager .Update();
                    _powerup_manager        .Update(gameTime, ref _player, ref _energy_storage_manager);
                    _enemy_managers.Values
                        .ToList()
                        .ForEach(x => x     .Update(ref Highscore, gameTime));
                    _wave_manager           .Update(gameTime, ref Highscore);
                    //game over 
                    if (_energy_storage_manager._energy_storages.Count == 0)
                    {
                        _state = CGameState.Gameover;
                        goto hack;
                    }
                    break;

                case CGameState.Gameover:
                    if (_gameoverscreen == null)
                        LoadGameOver(_wave_manager.Highscore);
                    ret = _gameoverscreen.Update(ms);
                    if (ret.HasValue && ret.Value == CGameState.StartGame)
                    {
                        _state = ret.Value;
                        goto hack;
                    }
                    else if (ret.HasValue && ret.Value == CGameState.Quit)
                    {
                        _state = ret.Value;
                        goto hack;
                    }
                    break;

                case CGameState.Quit:
                    Exit();
                    break;
            }

            if (ks.IsKeyDown(Keys.Escape)) Exit();

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(31, 31, 31));

            spriteBatch.Begin();

            switch (_state)
            {
                case CGameState.MainScreen:
                    _mainscreen             .Draw(spriteBatch);
                    break;

                case CGameState.CreditsScreen:
                    _creditsScreen          .Draw(spriteBatch);
                    break;

                case CGameState.Playing:
                _background_manager         .Draw(spriteBatch);
                    _portal_manager         .Draw(spriteBatch);
                    _bullet_manager         .Draw(spriteBatch);
                    _enemy_managers.Values
                        .ToList()
                        .ForEach(x => x     .Draw(spriteBatch));
                    _player                 .Draw(spriteBatch);
                    _energy_storage_manager .Draw(spriteBatch);
                    _bullet_hits_enemy      .Draw(spriteBatch);
                    _powerup_manager        .Draw(spriteBatch);

                    break;

                case CGameState.Gameover:
                    _gameoverscreen         .Draw(spriteBatch);
                    break;
            }

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}