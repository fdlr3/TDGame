using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Sprites {

    public class BulletManager {
        private Bullet _rootBullet;
        private Rectangle _window_size;
        public List<Bullet> Bullets { get; set; }

        public BulletManager(Texture2D texture, Rectangle window_size) {
            _rootBullet = new Bullet(texture);
            _window_size = window_size;
            Bullets = new List<Bullet>();
        }
        public void Add(Vector2 position, Vector2 direction) {
            Bullet x = _rootBullet.Clone() as Bullet;
            x.SetDirection(position, direction);
            Bullets.Add(x);
        }
        public void Update() {
            for (int i = 0; i < Bullets.Count; i++) {
                Bullets[i].UpdateBullet(_window_size);
                if (!Bullets[i]._isvalid) {
                    Bullets.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch) {
            foreach (var x in Bullets) {
                if (x._isvalid) {
                    //spriteBatch.Draw(x._texture, x._position, Color.White);
                    x.Draw(spriteBatch);
                }
            }
        }
    }


    public class Bullet : ICloneable {
        public Texture2D _texture;
        private int Velocity = -20;
        private Vector2 _direction;
        public Vector2 _position;
        public bool _isvalid;

        public Bullet(Texture2D texture) {
            _texture = texture;
            _isvalid = false;
        }
        public void SetDirection(Vector2 position, Vector2 direction) {
            _direction = direction;
            _position = position;
            _isvalid = true;
        }
        public void UpdateBullet(Rectangle _win_rect) {
            if (!_win_rect.Contains(_position))
                _isvalid = false;
            _position += _direction * Velocity;
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                _position,
                null, 
                Color.White,
                1f,
                default,
                new Vector2(1,1),
                SpriteEffects.None,
                0.25f
            );
        }
        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}