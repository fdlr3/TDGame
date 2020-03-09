using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class Portal : SpriteObject {
        private int _counter = 0;
        private int _pause = 0;

        public Portal(Texture2D texture, int w, int h)
            : base(texture, w, h) {
        }

        public void Init(Vector2 position) {
            _position = position;
            _counter = 0;
            _pause = 0;
        }

        public void Update() {
            if (_pause < 5) {
                _pause++;
            } else {
                _counter++;
                _pause = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (_counter == 12)
                _counter = 0;
            spriteBatch.Draw(_texture,
                 _position,
                 new Rectangle(_counter * _width,
                               0,
                               _width,
                               _heigth),
                 Color.White);
        }
    }
}
