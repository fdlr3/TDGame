
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Managers {
    public class Enemy : SpriteObject {
        private Rectangle _final_position;
        private Vector2 _direction;
        private bool _arrived;
        private Tuple<HealthBar, HealthBar> _healthbar;
        public int _max_health;
        public int _health;
        public int _strength;
        public int _velocity;
        public int _counter;
        public int _pause;
        public int _animation_count;
        public float _angle = .0f;
        
        


        public Enemy(Texture2D texture, int w, int h, int health, int strength, int velocity, int animation_count)
            : base(texture, w, h) {
            _arrived = false;
            _health = health;
            _max_health = health;
            _strength = strength;
            _velocity = velocity;
            _animation_count = animation_count;
        }

        public void Init
            (
                Vector2 position, 
                Rectangle final_position, 
                Vector2 rand_position, 
                float angle,
                HealthBar roothp
            ) {
            _position = position;
            _final_position = final_position;

            _isvalid = true;
            _arrived = false;
            _counter = 0;
            _pause = 0;
            _angle = angle;

            _healthbar = new Tuple<HealthBar, HealthBar>
                (
                    roothp.Clone() as HealthBar,
                    roothp.Clone() as HealthBar
                );
            _healthbar.Item1.SetColor(Color.Red);
            _healthbar.Item2.SetColor(Color.Black);

            //calculate direction
            Vector2 diff = Vector2.Subtract(position, rand_position);
            _direction = Vector2.Normalize(diff);
        }

        public void Damage(int dmg) {
            this._health -= dmg;
            if (_health <= 0)
                _isvalid = false;
        }

        public void Update() {
            //todo go till _destination is achieved
            if (_final_position.Contains(_position))
                _arrived = true;
            if (!_arrived)
                _position += _direction * -2;

            if (_pause < 5) {
                _pause++;
            } else {
                _counter++;
                _pause = 0;
            }
            Vector2 hp_pos = new Vector2(_position.X - (_width / 2), (_position.Y - _heigth / 2));
            _healthbar.Item1.Update(hp_pos);
            _healthbar.Item2.Update(hp_pos);
            _healthbar.Item1.ReduceLenght((float)_health / (float)_max_health);

        }

        public void Draw(SpriteBatch spriteBatch) {
            if (_counter == _animation_count)
                _counter = 0;
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _width,
                    _heigth),
                new Rectangle(
                    _counter * _width,
                    0,
                    _width,
                    _heigth),
                Color.White,
                _angle - (float) (Math.PI * 0.5f), 
                new Vector2(_width / 2, _heigth /2), 
                SpriteEffects.None, 
                0.25f
            );

            _healthbar.Item2.Draw(spriteBatch);
            _healthbar.Item1.Draw(spriteBatch);
        }
    }
}
