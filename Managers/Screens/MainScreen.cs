using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers
{
    public class MainScreen
    {
        private Texture2D _title_texture;
        private Vector2 _title_location;
        private int _animation_counter;
        private int _counter;
        private int _pause;
        private int _width;
        private int _height;

        private List<Button> _buttons;
        private List<Tuple<Texture2D, Vector2>> _images;
        private List<Tuple<SpriteFont, Vector2, string>> _texts;

        public MainScreen()
        {
            _buttons = new List<Button>();
            _images = new List<Tuple<Texture2D, Vector2>>();
            _texts = new List<Tuple<SpriteFont, Vector2, string>>();
        }

        public void AddTitle(Texture2D texture, Vector2 position, int n, int _w, int _h) {
            _title_texture = texture;
            _title_location = position;
            _animation_counter = n;
            _width = _w;
            _height = _h;
            _counter = 0;
            _pause = 0;
        }
        public void AddButton(Button btn) => _buttons.Add(btn);
        public void AddTexture(Texture2D texture, Vector2 position)
            => _images.Add(new Tuple<Texture2D, Vector2>(texture, position));
        public void AddText(SpriteFont font, Vector2 position, string text)
            => _texts.Add(new Tuple<SpriteFont, Vector2, string>(font, position, text));


        public Game1.CGameState? Update(MouseState ms)
        {
            if (_pause < 6) {
                _pause++;
            } else {
                _counter++;
                _pause = 0;
            }

            foreach (var btn in _buttons)
            {
                btn.Update(ms);
                if (btn.Activated())
                    return btn._state;
            }
            return null;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw title animation
            if (_counter == _animation_counter)
                _counter = 0;
            spriteBatch.Draw(
                _title_texture,
                new Rectangle(
                    (int)_title_location.X, //952
                    (int)_title_location.Y, //81
                    _width,
                    _height),
                new Rectangle(
                    _counter * _width,
                    0,
                    _width,
                    _height),
                Color.White,
                0.0f,
                new Vector2(1, 1),
                SpriteEffects.None,
                0.25f
            );

            foreach (var tex in _images)
            {
                spriteBatch.Draw(tex.Item1, tex.Item2, Color.White);
            }
            foreach (var btn in _buttons)
                btn.Draw(spriteBatch);
            foreach (var tex in _texts)
                spriteBatch.DrawString(tex.Item1, tex.Item3, tex.Item2, Color.White);

        }

    }
}
