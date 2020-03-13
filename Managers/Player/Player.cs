using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TDGame.Managers {
    public class Player : SpriteObject {
        private Rectangle _window_size;
        private Vector2 _direction;
        private float _angle = 0f;
        public int _strength;


        public Player(Texture2D texture, Rectangle win_size, Vector2 position, int w, int h, int strength) 
            : base(texture, w, h) {
            _position = position;
            _window_size = win_size;
            _direction = new Vector2(0, 0);
            _strength = strength;
        }

        public void IncStrength(int dmg) => _strength += dmg;

        public Vector2 GetPosition() {
            return _position;
        }
        public Vector2 GetDirection() {
            return _direction;
        }
        public void Update(KeyboardState ks, MouseState ms) {
            var n_v = _position;
            var _pos_center = new Rectangle((int)_position.X + 10, (int)_position.Y + 10, _width, _heigth);
            var cam_v = new Vector2(ms.X, ms.Y) - _pos_center.Center.ToVector2();
            _angle = (float)Math.Atan2(cam_v.Y, cam_v.X);

            if (ks.IsKeyDown(Keys.W)) {
                n_v.Y -= 8;
            } if (ks.IsKeyDown(Keys.S)) {
                n_v.Y += 8;
            } if (ks.IsKeyDown(Keys.A)) {
                n_v.X -= 8;
            } if (ks.IsKeyDown(Keys.D)) {
                n_v.X += 8;
            }

            if (_window_size.Contains(n_v)) {
                _position = n_v;

                //set camera position
                Vector2 diff = Vector2.Subtract(_position, new Vector2(ms.X, ms.Y));
                _direction = Vector2.Normalize(diff);
            }
            Debug.WriteLine($"C: ({_position.X}, {_position.Y}) M: ({ms.X}, {ms.Y}) Angle: {_angle}");
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                new Rectangle((int)_position.X, (int)_position.Y, _width, _heigth),
                null,
                Color.White,
                (float)(_angle),
                new Vector2(_width / 2, _heigth / 2),
                SpriteEffects.None,
                .75f
            );
        }
    }
}
