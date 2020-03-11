using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class BackgroundManager {
        private SpriteFont _spritefont;
        private List<Tuple<Texture2D, Vector2>> _textures;
        private int Wave = 0;
        private int Highscore = 0;
        private Vector2 _wave_location;
        private Vector2 _highscore_location;

        public BackgroundManager(SpriteFont font, Vector2 wave_location, Vector2 hs_location) { 
            _textures = new List<Tuple<Texture2D, Vector2>>();
            _spritefont = font;
            _wave_location = wave_location;
            _highscore_location = hs_location;
        }

        public void AddTexture(Texture2D texture, Vector2 location) {
            _textures.Add(new Tuple<Texture2D, Vector2>(texture, location));
        }

        public void Update(int wave, int highscore) {
            Wave = wave;
            Highscore = highscore;
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (var a in _textures)
                spriteBatch.Draw(
                    a.Item1,
                    a.Item2,
                    Color.White
                    );
            spriteBatch.DrawString(_spritefont, Highscore.ToString(), _highscore_location, Color.Black);
            spriteBatch.DrawString(_spritefont, Wave.ToString(), _wave_location, Color.Black);

        }


    }
}
