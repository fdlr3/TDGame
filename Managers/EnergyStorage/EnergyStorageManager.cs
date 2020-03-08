using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class EnergyStorageManager {
        private EnergyStorage _rootenergystorage;
        public List<EnergyStorage> _enemy_storages;

        public EnergyStorageManager(Texture2D texture, int w, int h) {
            _rootenergystorage = new EnergyStorage(texture, w, h);
            _enemy_storages = new List<EnergyStorage>();
        }

        public void Add(Vector2 energy_locations, int health) {
            var x = _rootenergystorage.Clone() as EnergyStorage;
            x.Init(energy_locations, health);
            _enemy_storages.Add(x);
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var x in _enemy_storages) {
                x.Draw(spriteBatch);
            }
        }

    }

    public class EnergyStorage : SpriteObject {
        private int _health;

        public EnergyStorage(Texture2D texture, int w, int h) 
            : base(texture, w, h) 
        {
            
        }

        public void Init(Vector2 position, int health) {
            _position = position;
            _health = health;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                new Rectangle((int)_position.X, (int)_position.Y, _width, _heigth),
                null, Color.White, 0.0f, new Vector2(1, 1), SpriteEffects.None, 0.25f
            ) ;
        }
    }

    
}
