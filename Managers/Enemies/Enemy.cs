
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


        public Enemy(Texture2D texture, int w, int h, int health, int strength, int velocity)
            : base(texture, w, h) {
            _arrived = false;
            _health = health;
            _strength = strength;
            _velocity = velocity;
        }

        public void Init(Vector2 position, Rectangle final_position) {
            _position = position;
            _isvalid = true;
            _arrived = false;
            Vector2 diff = Vector2.Subtract(position, final_position.Center.ToVector2());
            _direction = Vector2.Normalize(diff);
            _final_position = final_position;
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

        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                0.0f,
                default,
                new Vector2(1, 1),
                SpriteEffects.None,
                0.25f
            );
        }

    }
}
