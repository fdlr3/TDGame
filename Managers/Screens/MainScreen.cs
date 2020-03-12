using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class MainScreen {
        private List<Button> _buttons;
        private List<Tuple<Texture2D, Vector2>> _images;

        public MainScreen() {
            _buttons = new List<Button>();
            _images = new List<Tuple<Texture2D, Vector2>>();
        }

        public void AddButton(Button btn) => _buttons.Add(btn);
        public void AddTexture(Texture2D texture, Vector2 position)
            => _images.Add(new Tuple<Texture2D, Vector2>(texture, position));

        public Game1.CGameState? Update(MouseState ms) { 
            foreach(var btn in _buttons) {
                btn.Update(ms);
                if (btn.Activated())
                    return btn._state;
            }
            return null;
        }
        public void Draw(SpriteBatch spriteBatch) {
            foreach (var btn in _buttons)
                btn.Draw(spriteBatch);
            foreach (var tex in _images) {
                spriteBatch.Draw(tex.Item1, tex.Item2, Color.White);
            }
        }
        
    }
}
