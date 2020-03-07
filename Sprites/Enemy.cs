using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TDGame.Sprites {

    public class EnemyManager {
        private Enemy _rootEnemy;
        public List<Enemy> Enemies { get; set; }
        private List<Vector2> _spawn_locations;
        private List<Rectangle> _target_location;
        private Random _rand;

        public EnemyManager(List<Vector2> spawnLocations, List<Rectangle> target_locations, Texture2D texture) {
            _spawn_locations = spawnLocations;
            _target_location = target_locations;
            _rootEnemy = new Enemy(texture);
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

    public class Enemy : ICloneable {
        public Texture2D _texture;
        public Vector2 _position;
        private Rectangle _final_position;
        private Vector2 _direction;
        private bool _arrived;
        public int _health = 100;
        public bool _isvalid;


        public Enemy(Texture2D texture2D) {
            _texture = texture2D;
            _isvalid = false;
            _arrived = false;
        }

        public void Init(Vector2 position, Rectangle final_position) {
            _position = position;
            _isvalid = true;
            _arrived = false;
            Vector2 diff = Vector2.Subtract(position, final_position.Center.ToVector2());
            _direction = Vector2.Normalize(diff);
            _final_position = final_position;
        }

        public void Damage(int dmg) {
            this._health -= dmg;
            if (_health <= 0)
                _isvalid = false;
        }

        public void Update() {
            //todo go till _destination is achieved
            if (_final_position.Contains(_position))
                _arrived = true;
            if (!_arrived)
                _position += _direction * -2;
                
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                0.0f,
                default,
                new Vector2(1, 1),
                SpriteEffects.None,
                0.25f
            );
        }

        public object Clone() {
            return this.MemberwiseClone();
        }
    }
}
