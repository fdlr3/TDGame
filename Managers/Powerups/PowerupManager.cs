using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class PowerupManager {
        public static Random _rnd;
        public enum PowerUpType { PUT_HP, PUT_PWR }

        public double _time_counter;
        public int _diff;
        private List<Powerup> _powersup;
        private List<Rectangle> _spawn_locations;
    
        public PowerupManager(int time_between, List<Rectangle> spawn_locations) {
            _diff = time_between;
            _time_counter = 0;
            _rnd = new Random();
            _powersup = new List<Powerup>();
            _spawn_locations = spawn_locations;
        }

        private Vector2 GetRandomLocation() {
            Rectangle r_loc = _spawn_locations[_rnd.Next(0, _spawn_locations.Count)];
            return new Vector2
                (
                    _rnd.Next(r_loc.X, r_loc.X + r_loc.Width),
                    _rnd.Next(r_loc.Y, r_loc.Y + r_loc.Height)
                );

        }

        public void AddPowerup(Texture2D texture, int w, int h, int n, PowerUpType type) 
            => _powersup.Add(new Powerup(texture, w, h, n, type));

        public void Update(GameTime gameTime, ref Player player, ref EnergyStorageManager esm) {
            _time_counter += gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_time_counter > _diff) {
                var powerup = _powersup[_rnd.Next(0, _powersup.Count)];
                powerup.SetPosition(GetRandomLocation());
                powerup._isvalid = true;
                _time_counter = 0;
            }
            foreach (var pu in _powersup) {
                pu.Update();

                if (pu._isvalid) {
                    var r = new Rectangle(
                        (int)pu._position.X,
                        (int)pu._position.Y,
                        pu._width,
                        pu._heigth
                        );
                    if (r.Contains(player._position)) {
                        pu._isvalid = false;
                        if (pu._type == PowerUpType.PUT_HP) {
                            foreach(var EP in esm._energy_storages) {
                                int add_hp = 200;
                                if (EP._health + 200 > EP._max_health)
                                    add_hp = EP._max_health - EP._health;
                                EP._health += add_hp;
                            }
                        } else if(pu._type == PowerUpType.PUT_PWR) {
                            player.IncStrength(10);
                        }
                    }
                }
            }
                
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var a in _powersup)
                if (a._isvalid)
                    a.Draw(spriteBatch);
        }
        


    }
}
