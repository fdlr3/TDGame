using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TDGame.Managers {

    public class EnemyManager {

        private Enemy _rootEnemy;
        public List<Enemy> Enemies { get; set; }
        private List<Vector2> _spawn_locations;
        private EnergyStorageManager _energy_manager;
        private Random _rand;

        public EnemyManager(List<Vector2> spawnLocations, ref EnergyStorageManager energy_manager, Texture2D texture) {
            _spawn_locations = spawnLocations;
            _energy_manager = energy_manager;
            _rootEnemy = new Enemy(texture, 50, 50, 200, 10, -5);
            _rand = new Random();
            Enemies = new List<Enemy>();
        }

        public void Add() {
            var x = _rootEnemy.Clone() as Enemy;
            int sel_spawn = _rand.Next(0, _spawn_locations.Count);

            int j = 0;
            float dist = float.MaxValue;

            for(int i = 0; i < _energy_manager._enemy_storages.Count; i++) {
                var c = _energy_manager._enemy_storages[i];
                Rectangle center = new Rectangle((int)c._position.X, (int)c._position.Y, c._width, c._heigth);
                float cur_dist = Vector2.Distance(_spawn_locations[sel_spawn], center.Center.ToVector2());

                if(cur_dist < dist) {
                    dist = cur_dist;
                    j = i;
                }
            }
            var final_loc = _energy_manager._enemy_storages[j];
            x.Init(_spawn_locations[sel_spawn], new Rectangle((int)final_loc._position.X, (int)final_loc._position.Y, final_loc._width, final_loc._heigth));
            Enemies.Add(x);
        }

        public void Update() {
            for (int i = 0; i < Enemies.Count; i++) {
                Enemies[i].Update();
                if (!Enemies[i]._isvalid) {
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var x in Enemies) {
                x.Draw(spriteBatch);
            }
        }

    }    
}
