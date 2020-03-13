using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDGame.Managers {
    public class WaveManager {
        private static Random rnd;

        public int WaveCount { get; set; } = 1;
        public int Highscore { get; set; } = 0;
        public double WaveTimeout = 0;
        public double TimeoutCounter = 0;
        public double OffsetCounter = 0;
        public double SpawnOffset = 0;
        private Dictionary<string, EnemyManager> _EM;
        private Queue<string> _enemy_queue;

        public WaveManager(ref Dictionary<string, EnemyManager> EM, double wave_timout, double spawn_offset) {
            _EM = EM;
            WaveTimeout = wave_timout;
            SpawnOffset = spawn_offset;
            _enemy_queue = new Queue<string>();
            rnd = new Random();
            GenerateEnemyQueue();
        }

        public void Update(GameTime gameTime, ref int add_hs) {
            OffsetCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
            Highscore += add_hs;

            if (_enemy_queue.Count > 0) {
                if (OffsetCounter > SpawnOffset) {
                    _EM[_enemy_queue.Dequeue()].Add();
                    OffsetCounter = 0;
                }
            } else {
                TimeoutCounter += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TimeoutCounter > WaveTimeout) {
                    TimeoutCounter = 0;
                    GenerateEnemyQueue();
                    WaveCount++;
                    foreach(var a in _EM.Values) {
                        a.IncreaseStrength(0.025f);
                    }
                    if(SpawnOffset > 1000)
                        SpawnOffset -= 100;
                }
            }
        }

        private void GenerateEnemyQueue() {
            List<string> keys = new List<string>();
            _EM.ToList().ForEach(x => keys.Add(x.Key));
            int target_count = (10 * WaveCount) + WaveCount;
            for(int i = 0; i < target_count; i++)
                _enemy_queue.Enqueue(keys[rnd.Next(0, keys.Count)]);
        }
    }
}
