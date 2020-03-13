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
        private static Random _rand;
        private Enemy _rootEnemy;
        private HealthBar _roothp;

        public List<Enemy> Enemies { get; set; }

        private PortalManager _PM;
        private EnergyStorageManager _EM;

        public EnemyManager
            (
                ref PortalManager portal_manager,
                ref EnergyStorageManager 
                energy_manager, 
                Texture2D texture,
                HealthBar hp,
                int w, int h,
                int strength, int health,
                int velocity,
                int animaction_count
            ) 
            {
            _EM = energy_manager;
            _PM = portal_manager;
            _roothp = hp;

            _rootEnemy = new Enemy(texture, w, h, health, strength, -Math.Abs(velocity), animaction_count);
            _rand = new Random();
            Enemies = new List<Enemy>();
        }

        public void Add() {
            if (_EM._energy_storages.Count == 0) return;
            var x = _rootEnemy.Clone() as Enemy;
            int sel_spawn = _rand.Next(0, _PM._portals.Count);

            //calculate closest energy point
            int j = 0;
            float dist = float.MaxValue;

            for (int i = 0; i < _EM._energy_storages.Count; i++) {
                var es = _EM._energy_storages[i];
                Rectangle es_center = new Rectangle((int)es._position.X, (int)es._position.Y, es._width, es._height);
                float cur_dist = Vector2.Distance(_PM._portals[sel_spawn]._position, es_center.Center.ToVector2());

                if(cur_dist < dist) {
                    dist = cur_dist;
                    j = i;
                }
            }

            //calculate start point
            Portal selected_portal = _PM._portals[sel_spawn];
            Vector2 center_spawn = new Rectangle(
                (int)selected_portal._position.X,
                (int)selected_portal._position.Y,
                selected_portal._width,
                selected_portal._height
            ).Center.ToVector2();

            var final_loc = _EM._energy_storages[j];

            //calculate final location
            Rectangle final_position = new Rectangle(
                (int)final_loc._position.X,
                (int)final_loc._position.Y,
                final_loc._width,
                final_loc._height
            );
            final_position.Inflate(
                (int)-(final_loc._width * 0.25f), 
                (int)-(final_loc._height * 0.25f)
            );

            //get random point in final location
            Vector2 rand_point = new Vector2(
                _rand.Next(final_position.X, final_position.X + final_position.Width),
                _rand.Next(final_position.Y, final_position.Y + final_position.Height)
            );

            final_position.Inflate(
                (int)(final_loc._width * 0.25f),
                (int)(final_loc._height * 0.25f)
            );

            //calculate angle
            float monster_angle = .0f;
            var _pos_center = new Rectangle((int)center_spawn.X, (int)center_spawn.Y, _rootEnemy._width, _rootEnemy._height);
            var cam_v = final_loc._position - _pos_center.Center.ToVector2();
            monster_angle = (float)Math.Atan2(cam_v.Y, cam_v.X);

            x.Init(center_spawn, final_position, rand_point, monster_angle, _roothp, ref final_loc);
            Enemies.Add(x);
        }

        //val has to be between 0.0 and 1.0f
        public void IncreaseStrength(float val) {
            if(val >= 0.0f && val <= 1.0f) {
                _rootEnemy._max_health  +=  (int)(_rootEnemy._max_health * val);
                _rootEnemy._health      +=  (int)(_rootEnemy._health * val);
                _rootEnemy._strength    +=  (int)(_rootEnemy._strength * val);
                _rootEnemy._velocity    *=  (1.0f + val);
            }
        }

        public void Update(ref int Highscore, GameTime gameTime) {
            for (int i = 0; i < Enemies.Count; i++) {
                Enemies[i].Update(gameTime);
                if (!Enemies[i]._isvalid) {
                    if (Enemies[i]._killed)
                        Highscore += Enemies[i]._max_health / 10;
                    Enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach(var x in Enemies)
                x.Draw(spriteBatch);
        }

    }    
}
