﻿using System;
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
        private List<Rectangle> _target_location;
        private Random _rand;

        public EnemyManager(List<Vector2> spawnLocations, List<Rectangle> target_locations, Texture2D texture) {
            _spawn_locations = spawnLocations;
            _target_location = target_locations;
            _rootEnemy = new Enemy(texture, 50, 50, 200, 10, -5);
            _rand = new Random();
            Enemies = new List<Enemy>();
        }

        public void Add() {
            var x = _rootEnemy.Clone() as Enemy;
            int sel_spawn = _rand.Next(0, _spawn_locations.Count);

            int j = 0;
            float dist = float.MaxValue;

            for(int i = 0; i < _target_location.Count; i++) {
                Vector2 center = _target_location[i].Center.ToVector2();
                float cur_dist = Vector2.Distance(_spawn_locations[sel_spawn], center);

                if(cur_dist < dist) {
                    dist = cur_dist;
                    j = i;
                }
            }
            x.Init(_spawn_locations[sel_spawn], _target_location[j]);
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