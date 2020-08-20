using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public class Explode : GameComponent
    {
        List<ExplodeParticle> Particles;
        Camera TheCamera;
        Vector3 TheColor = Vector3.One;
        Vector3 TheEmissiveColor = Vector3.Zero;

        public Vector3 DefuseColor { set => TheColor = value; }
        public Vector3 EmissiveColor { set => TheEmissiveColor = value; }

        public Explode(Game game, Camera camera) : base(game)
        {
            Particles = new List<ExplodeParticle>();
            TheCamera = camera;

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            TheColor = Vector3.One;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            bool done = true;

            foreach (ExplodeParticle particle in Particles)
            {
                if (particle.Enabled)
                {
                    done = false;
                    break;
                }
            }

            if (done)
                Enabled = false;

            base.Update(gameTime);
        }

        public void Setup(Vector3 color, Vector3 lightcolor)
        {
            TheColor = color;
            TheEmissiveColor = lightcolor;
        }

        public void Spawn(Vector3 position, float radius, int minCount, float speed,
            float scale, float life)
        {
            Enabled = true;
            int count = Core.RandomMinMax(minCount, (int)(minCount + radius * 2));

            if (count > Particles.Count)
            {
                int more = count - Particles.Count;

                for (int i = 0; i < more; i++)
                {
                    Particles.Add(new ExplodeParticle(Game, TheCamera));
                }
            }

            foreach (ExplodeParticle particle in Particles)
            {
                position += new Vector3(Core.RandomMinMax(-radius, radius),
                    Core.RandomMinMax(-radius, radius), 0);

                particle.Spawn(position, speed, scale, life);
                particle.DiffuseColor = TheColor;
                particle.EmissiveColor = TheEmissiveColor;
            }
        }

        public void Kill()
        {
            foreach (ExplodeParticle particle in Particles)
            {
                particle.Enabled = false;
                Enabled = false;
            }
        }
    }
}
