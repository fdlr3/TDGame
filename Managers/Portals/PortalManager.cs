using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class PortalManager {
        private Portal _rootportal;
        public List<Portal> _portals;

        public PortalManager(Texture2D texture, int w, int h) {
            _portals = new List<Portal>();
            _rootportal = new Portal(texture, w, h);
        }

        public void AddPortal(Vector2 position) {
            var x = _rootportal.Clone() as Portal;
            x.Init(position);
            _portals.Add(x);
        }

        public void Update() {
            foreach(var a in _portals)
                a.Update();
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (var a in _portals)
                a.Draw(spriteBatch);
        }

    }
}
