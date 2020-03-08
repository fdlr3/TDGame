using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
	public class Bullet : SpriteObject {
		private int _velocity;
		private Vector2 _direction;

		public Bullet(Texture2D texture, int velocity, int w, int h)
			: base(texture, w, h) {
			_velocity = velocity;
		}
		public void SetDirection(Vector2 position, Vector2 direction) {
			_direction = direction;
			_position = position;
			_isvalid = true;
		}
		public void UpdateBullet(Rectangle _win_rect) {
			if (!_win_rect.Contains(_position))
				_isvalid = false;
			_position += _direction * _velocity;

		}
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(
				_texture,
				_position,
				null,
				Color.White,
				1f,
				default,
				new Vector2(1, 1),
				SpriteEffects.None,
				0.25f
			);
		}

	}
}
