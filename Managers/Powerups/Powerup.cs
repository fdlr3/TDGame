using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class Powerup : SpriteObject {
        private int _animation_count;
        private int _pause;
        private int _counter;
        public PowerupManager.PowerUpType _type;

        public Powerup(Texture2D texture, int w, int h, int n, PowerupManager.PowerUpType type)
            : base(texture, w, h) {
            _position = new Vector2(10,10);
            _animation_count = n;
            _pause = 0;
            _counter = 0;
            _isvalid = false;
            _type = type;
        }

        public void SetPosition(Vector2 position) {
            _position = position;
            _counter = 0;
        }

        public void Update() {
            if (_isvalid) {
                if (_pause < 3) {
                    _pause++;
                } else {
                    _counter++;
                    _pause = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (_counter == _animation_count) {
                _counter = 0;
            }

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
                0.0f,
                new Vector2(1, 1),
                SpriteEffects.None,
                0.25f
            );
        }
    }
}
