using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Panther
{
    class ExplodeParticle : Cube
    {
        Timer LifeTimer;

        public ExplodeParticle(Game game, Camera camera) : base(game, camera)
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

        public void Spawn(Vector3 position, float velocity, float scaleRange, float maxLife)
        {
            base.Spawn(position);

            Velocity = Core.RandomVelocity(velocity);
            Scale = Core.RandomMinMax(scaleRange, 1.5f * scaleRange);
            LifeTimer.Reset(Core.RandomMinMax(0.1f, maxLife));
        }
    }
}
