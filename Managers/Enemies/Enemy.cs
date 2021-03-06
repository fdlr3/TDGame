﻿
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDGame.Managers {
    public class Enemy : SpriteObject {
        private Rectangle _final_position;
        private Vector2 _direction;
        public EnergyStorage _ES;
        private Tuple<HealthBar, HealthBar> _healthbar;
        public bool _arrived;
        public bool _killed;
        public int _max_health;
        public int _health;
        public int _strength;
        public double _damage_counter;
        public float _velocity;
        public int _counter;
        public int _pause;
        public int _animation_count;
        public float _angle = .0f;
  

        public Enemy(Texture2D texture, int w, int h, int health, int strength, float velocity, int animation_count)
            : base(texture, w, h) {
            _arrived = false;
            _health = health;
            _max_health = health;
            _strength = strength;
            _velocity = velocity;
            _animation_count = animation_count;
            _killed = false;
        }

        public void Init
            (
                Vector2 position, 
                Rectangle final_position, 
                Vector2 rand_position, 
                float angle,
                HealthBar roothp,
                ref EnergyStorage ES
            ) {
            _position = position;
            _final_position = final_position;
            _ES = ES;
            _isvalid = true;
            _arrived = false;
            _counter = 0;
            _pause = 0;
            _damage_counter = 0;
            _angle = angle;
            _killed = false;
            _healthbar = new Tuple<HealthBar, HealthBar>
                (
                    roothp.Clone() as HealthBar,
                    roothp.Clone() as HealthBar
                );
            _healthbar.Item1.SetColor(Color.Red);
            _healthbar.Item2.SetColor(Color.Black);

            //calculate direction
            Vector2 diff = Vector2.Subtract(position, rand_position);
            _direction = Vector2.Normalize(diff);
        }

        public void Damage(int dmg) {
            this._health -= dmg;
            if (_health <= 0) {
                _killed = true;
                _isvalid = false;
            }
                
        }

        public void Update(GameTime gameTime) {
            //todo go till _destination is achieved
            if (_final_position.Contains(_position)) {
                _arrived = true;
                _damage_counter += gameTime.ElapsedGameTime.TotalMilliseconds;
                if(_damage_counter > 1000) {
                    _ES.Damage(_strength);
                    _damage_counter = 0;
                }
            }
            if (!_ES._isvalid)
                _isvalid = false;
                
            if (!_arrived)
                _position += _direction * -2;

            if (_pause < 5) {
                _pause++;
            } else {
                _counter++;
                _pause = 0;
            }
            Vector2 hp_pos = new Vector2(_position.X - (_width / 2), (_position.Y - _height / 2));
            _healthbar.Item1.Update(hp_pos);
            _healthbar.Item2.Update(hp_pos);
            _healthbar.Item1.ReduceLenght((float)_health / (float)_max_health);

        }

        public void Draw(SpriteBatch spriteBatch) {
            if (_counter == _animation_count)
                _counter = 0;
            spriteBatch.Draw(
                _texture,
                new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
                    _width,
                    _height),
                new Rectangle(
                    _counter * _width,
                    0,
                    _width,
                    _height),
                Color.White,
                _angle - (float) (Math.PI * 0.5f), 
                new Vector2(_width / 2, _height /2), 
                SpriteEffects.None, 
                0.25f
            );

            _healthbar.Item2.Draw(spriteBatch);
            _healthbar.Item1.Draw(spriteBatch);
        }
    }
}
