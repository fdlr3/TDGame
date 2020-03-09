
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
        public int _health;
        public int _strength;
        public int _velocity;
        public int _counter;
        public int _pause;
        public float _angle = .0f;
        


        public Enemy(Texture2D texture, int w, int h, int health, int strength, int velocity)
            : base(texture, w, h) {
            _arrived = false;
            _health = health;
            _strength = strength;
            _velocity = velocity;
        }

        public void Init(Vector2 position, Rectangle final_position, float angle) {
            _position = position;
            _isvalid = true;
            _arrived = false;
            Vector2 diff = Vector2.Subtract(position, final_position.Center.ToVector2());
            _direction = Vector2.Normalize(diff);
            _final_position = final_position;
            _counter = 0;
            _pause = 0;
            _angle = angle;
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

        }

        public void Draw(SpriteBatch spriteBatch) {
            if (_counter == 4)
                _counter = 0;
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    (int)_position.X, //952
                    (int)_position.Y, //81
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
        }
    }
}
