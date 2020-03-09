using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class EnergyStorageManager {
        private EnergyStorage _rootenergystorage;
        public List<EnergyStorage> _energy_storages;

        public EnergyStorageManager(Texture2D texture, int w, int h) {
            _rootenergystorage = new EnergyStorage(texture, w, h);
            _energy_storages = new List<EnergyStorage>();
        }

        public void Add(Vector2 energy_locations, int health) {
            var x = _rootenergystorage.Clone() as EnergyStorage;
            x.Init(energy_locations, health);
            _energy_storages.Add(x);
        }

        public void Update() {
            foreach (var a in _energy_storages)
                a.Update();
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var x in _energy_storages)
                x.Draw(spriteBatch);
        }

    }
}
