using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class EnergyStorage : SpriteObject {
        private Tuple<HealthBar, HealthBar> _healthbar;
        public int _max_health;
        public int _health;
        public int _counter;
        public int _pause;

        public EnergyStorage(Texture2D texture, int w, int h)
            : base(texture, w, h) {
            _isvalid = true;
        }

        public void Init(Vector2 position, Vector2 hp_pos, HealthBar hproot, int health) {
            _position = position;
            _health = health;
            _max_health = health;
            _counter = 0;
            _pause = 0;
            _isvalid = true;

            _healthbar = new Tuple<HealthBar, HealthBar>
                (
                    hproot.Clone() as HealthBar,
                    hproot.Clone() as HealthBar
                );
            _healthbar.Item1.SetColor(Color.Red);
            _healthbar.Item2.SetColor(Color.Black);
            _healthbar.Item1.Update(hp_pos);
            _healthbar.Item2.Update(hp_pos);
        }

        public void Damage(int dmg) => _health -= dmg;

        public void Update() {
            if (_pause < 3) {
                _pause++;
            } else {
                _counter++;
                _pause = 0;
            }
            _healthbar.Item1.ReduceLenght((float)_health / (float)_max_health);
            if(_health <= 0) {
                _isvalid = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (_counter == 7)
                _counter = 0;
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    (int)_position.X, //952
                    (int)_position.Y, //81
                    _width,
                    _height),
                new Rectangle(
                    _counter * _width,
                    0,
                    _width,
                    _height),
                Color.White,
                0.0f,
                new Vector2(1,1),
                SpriteEffects.None,
                0.25f
            );
            _healthbar.Item2.Draw(spriteBatch);
            _healthbar.Item1.Draw(spriteBatch);
        }
    }
}
