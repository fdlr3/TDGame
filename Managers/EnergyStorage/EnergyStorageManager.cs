using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDGame.Managers {
    public class EnergyStorageManager {
        private HealthBar _hproot;
        private EnergyStorage _rootenergystorage;
        public List<EnergyStorage> _energy_storages;

        public EnergyStorageManager(Texture2D texture, int w, int h, HealthBar healtroot) {
            _rootenergystorage = new EnergyStorage(texture, w, h);
            _energy_storages = new List<EnergyStorage>();
            _hproot = healtroot;
        }

        public void Add(Vector2 energy_locations, Vector2 health_pos, int health) {
            var x = _rootenergystorage.Clone() as EnergyStorage;
            x.Init(energy_locations, health_pos, _hproot, health);
            _energy_storages.Add(x);
        }

        public void Update() {
            for(int i = 0; i < _energy_storages.Count; i++) {
                if (!_energy_storages[i]._isvalid) {
                    _energy_storages.RemoveAt(i);
                    i--;
                }else {
                    _energy_storages[i].Update();
                }       
            }
                
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var x in _energy_storages)
                x.Draw(spriteBatch);
        }

    }
}
