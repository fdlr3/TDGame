using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class Button : SpriteObject {
        public Game1.CGameState _state;
        private bool _ishovering;
        private bool _clicked;
        private Color _color;
        private ButtonState? _previous_state = null;

        public Button(Texture2D texture, Vector2 position, int w, int h, Game1.CGameState gameState)
            : base(texture, w, h) { 
            _state = gameState;
            _position = position;
            _ishovering = false;
            _color = Color.White;
            _clicked = false;
        }

        public bool Activated() => _clicked;

        public void Update(MouseState ms) {
            _clicked = false;
            if (
                    new Rectangle
                    (
                        (int)_position.X,
                        (int)_position.Y,
                        _width,
                        _heigth
                    ).Contains(new Vector2(ms.X, ms.Y))
                ) {
                _ishovering = true;
                _color = Color.Gray;

                if (ms.LeftButton == ButtonState.Pressed) {
                    _previous_state = ButtonState.Pressed;
                } else if
                    (
                    ms.LeftButton == ButtonState.Released &&
                    _previous_state.HasValue && 
                    _previous_state.Value == ButtonState.Pressed
                    ) 
                {
                    _clicked = true;
                    _previous_state = null;
                }
                    

            } else {
                _ishovering = false;
                _color = Color.White;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, _position, _color);
        }



    }
}
