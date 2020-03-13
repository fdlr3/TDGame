using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Managers {
    public class ExplosionAnimation : SpriteObject {
        private int _animation_count;
        private int _pause;
        private int _counter;

        public ExplosionAnimation(Texture2D texure, int w, int h, Vector2 position, int n)
            : base(texure, w, h){
            _position = position;
            _animation_count = n;
            _pause = 0;
            _counter = 0;
            _isvalid = true;
        }

        public void Update() {
            if (_isvalid) {
                if (_pause < 5) {
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
                _isvalid = false;
            }
                
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
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
        }


    }
}
