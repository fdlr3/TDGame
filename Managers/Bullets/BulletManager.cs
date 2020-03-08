using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Managers {

	public class BulletManager {
		private Bullet _rootBullet;
		private Rectangle _window_size;
		public List<Bullet> Bullets { get; set; }

		public BulletManager(Texture2D texture, Rectangle window_size) {
			_rootBullet = new Bullet(texture, -20, 10, 10);
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



}