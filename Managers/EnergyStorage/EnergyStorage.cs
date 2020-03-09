using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class EnergyStorage : SpriteObject {
        private int _health;
        public int _counter;
        public int _pause;

        public EnergyStorage(Texture2D texture, int w, int h)
            : base(texture, w, h) {

        }

        public void Init(Vector2 position, int health) {
            _position = position;
            _health = health;
            _counter = 0;
            _pause = 0;
        }

        public void Update() {
            if (_pause < 3) {
                _pause++;
            } else {
                _counter++;
                _pause = 0;
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
                    _heigth),
                new Rectangle(
                    _counter * _width,
                    0,
                    _width,
                    _heigth),
                Color.White,
                0.0f,
                new Vector2(1,1),
                SpriteEffects.None,
                0.25f
            );
        }
    }
}
