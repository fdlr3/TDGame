using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TDGame.Sprites {
    public class Player {
        private Texture2D _texture;
        private Rectangle _position;
        private Rectangle _window_size;
        private Vector2 _direction;
        private float _angle = 0f;


        public Player(Texture2D texture, Rectangle win_size, Rectangle position) {
            _position = position;
            _texture = texture;
            _window_size = win_size;
            _direction = new Vector2(0, 0);
        }      
        public Vector2 GetPosition() {
            return new Vector2(_position.X, _position.Y);
        }
        public Vector2 GetDirection() {
            return _direction;
        }
        public void Update(KeyboardState ks, MouseState ms) {
            var n_v = _position;
            var cam_v = new Vector2(ms.X, ms.Y) - _position.Center.ToVector2();
            _angle = (float)Math.Atan2(cam_v.Y, cam_v.X);

            if (ks.IsKeyDown(Keys.W)) {
                n_v.Y -= 3;
            } if (ks.IsKeyDown(Keys.S)) {
                n_v.Y += 3;
            } if (ks.IsKeyDown(Keys.A)) {
                n_v.X -= 3;
            } if (ks.IsKeyDown(Keys.D)) {
                n_v.X += 3;
            }

            if (_window_size.Contains(n_v)) {
                _position = n_v;

                //set camera position
                Vector2 diff = Vector2.Subtract(new Vector2(_position.X, _position.Y), new Vector2(ms.X, ms.Y));
                _direction = Vector2.Normalize(diff);
            }
            Debug.WriteLine($"C: ({_position.X}, {_position.Y}) M: ({ms.X}, {ms.Y}) Angle: {_angle}");
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                (float)(_angle + (Math.PI * 0.5f)),
                new Vector2(_position.Width / 2, _position.Height / 2),
                SpriteEffects.FlipVertically,
                .75f
            );
        }
    }
}
