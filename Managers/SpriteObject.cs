using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class SpriteObject : ICloneable {
        protected Texture2D _texture;
        public Vector2 _position;
        public int _width;
        public int _height;
        public bool _isvalid;

        public SpriteObject(Texture2D texture, int w, int h){
            _texture = texture;
            _width = w;
            _height = h;
            _isvalid = false;
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
