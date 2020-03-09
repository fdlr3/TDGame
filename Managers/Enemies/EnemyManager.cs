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
            _rootEnemy = new Enemy(texture, 48, 50, 200, 10, -5);
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
            Vector2 start_position = _spawn_locations[sel_spawn];
            Rectangle final_position = new Rectangle(
                (int)final_loc._position.X,
                (int)final_loc._position.Y,
                final_loc._width,
                final_loc._heigth
            );

            //calculate angle
            float monster_angle = .0f;
            var _pos_center = new Rectangle((int)start_position.X + 10, (int)start_position.Y + 10, _rootEnemy._width, _rootEnemy._heigth);
            var cam_v = final_loc._position - _pos_center.Center.ToVector2();
            monster_angle = (float)Math.Atan2(cam_v.Y, cam_v.X);

            x.Init(start_position, final_position, monster_angle);
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
