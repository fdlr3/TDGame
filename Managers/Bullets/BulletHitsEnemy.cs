using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TDGame.Managers {
    public class BulletHitsEnemy {
        public Player P { get; set; }
        public EnemyManager EM { get; set; }
        public BulletManager BM { get; set; }

        public BulletHitsEnemy(
            ref EnemyManager enemy_manager, 
            ref BulletManager bullet_manager,
            ref Player player
            ) 
            {
            EM = enemy_manager;
            BM = bullet_manager;
            P = player;
        }

        public void Update() {
            foreach(var b in BM.Bullets) 
            {
                foreach(var e in EM.Enemies) 
                {
                    Rectangle rect = new Rectangle((int)e._position.X, (int)e._position.Y, 50, 50);
                    if(rect.Contains(b._position)) {
                        e.Damage(P._strength);
                        b._isvalid = false;
                    }
                }
            }
        }
    }
}
