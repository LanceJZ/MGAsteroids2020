using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Panther
{
    class SmokeParticle : ModelEntity
    {
        Timer LifeTimer;

        public SmokeParticle(Game game, Camera camera, Model model) : base(game, camera, model)
        {
            LifeTimer = new Timer(game);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (LifeTimer.Elapsed)
                Enabled = false;

            base.Update(gameTime);
        }

        public void Spawn(Vector3 position, float size, float drift, float speed, float life)
        {
            Scale = Core.RandomMinMax(size / 2, size);
            LifeTimer.Reset(Core.RandomMinMax(life / 10, life));
            Vector3 velocity;
            velocity.Y = Core.RandomMinMax(speed / 10, speed);
            velocity.X = Core.RandomMinMax(-drift, drift);
            velocity.Z = Core.RandomMinMax(-drift, drift);
            base.Spawn(position, velocity);
        }
    }
}
