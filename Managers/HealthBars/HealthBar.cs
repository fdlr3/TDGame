using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace TDGame.Managers {
    public class HealthBar : SpriteObject {
        private float _red_length = 1.0f;
        private Color _color = Color.Red;

        public HealthBar(Texture2D texture, int _w, int _h)
            : base(texture, _w, _h) {
            _position = new Vector2(0,0);
        }

        public void SetColor(Color color) => _color = color;
        public void ReduceLenght(float red_length) => _red_length = red_length;
        public void Update(Vector2 position) => _position = position;

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                new Rectangle
                (
                    (int)_position.X,
                    (int)_position.Y,
                    (int)(_width * _red_length),
                    _heigth
                ),
                new Rectangle
                (
                    0, 0,
                    (int)(_width * _red_length),
                    _heigth
                ),
                _color,
                .0f,
                new Vector2(1, 1),
                SpriteEffects.None,
                1.0f
            );
            if(_red_length != 1.0f)
                Debug.WriteLine($"w/l {_red_length} => {_width * _red_length}");
        }

    }

}
