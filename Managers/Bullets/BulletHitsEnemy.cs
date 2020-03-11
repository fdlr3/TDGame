using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace TDGame.Managers {
    public class BulletHitsEnemy {
        //Sealed class for the health display
        private sealed class DamageDisplayer {
            public Vector2 Position { get; set; }
            public int Damage { get; set; }
            public int Counter { get; set; }
            public DamageDisplayer(Vector2 vec, int dmg, int counter) {
                Position = vec;
                Damage = dmg;
                Counter = counter;
            }
        }

        //Properties
        public Player P { get; set; }
        public Dictionary<string, EnemyManager> EM { get; set; }
        public BulletManager BM { get; set; }
        private List<DamageDisplayer> _damage_positioins;

        //Constructor
        public BulletHitsEnemy(
            ref Dictionary<string, EnemyManager> enemies, 
            ref BulletManager bullet_manager,
            ref Player player
            ) 
            {
            EM = enemies;
            BM = bullet_manager;
            P = player;
            _damage_positioins = new List<DamageDisplayer>();
        }

        public void Update() {
            foreach(var b in BM.Bullets) 
            {
                foreach(var el in EM) 
                {
                    foreach(var e in el.Value.Enemies) 
                    {
                        Rectangle rect = new Rectangle((int)e._position.X - 30, (int)e._position.Y - 25, e._width, e._width);
                        if (rect.Contains(b._position)) {
                            e.Damage(P._strength);
                            b._isvalid = false;
                            Vector2 loc_vec = new Vector2(b._position.X, b._position.Y);

                            //reposition text if they overlap
                            if (_damage_positioins.FirstOrDefault(x => x.Position == loc_vec) != null) {
                                loc_vec.X -= 5.0f;
                                loc_vec.Y -= 5.0f;
                            }
                            _damage_positioins.Add(new DamageDisplayer(loc_vec, P._strength, 6));
                            break;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font) {
            DamageDisplayer item = null;
            for(int i = 0; i < _damage_positioins.Count; i++) {
                item = _damage_positioins[i];
                if(item.Counter == 0) {
                    _damage_positioins.RemoveAt(i);
                    i--;
                } else {
                    spriteBatch.DrawString(font, $"- {item.Damage}", item.Position, Color.Red);
                    item.Counter--;
                }
            }
        }
    }
}
