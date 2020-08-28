using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;

namespace Asteroids2020.Entities
{
    public class Player : VectorModel
    {
        #region Fields
        List<Shot> shots = new List<Shot>();
        Camera CameraRef;
        VectorModel flame;
        Timer flameTimer;
        Color color = new Color(175, 175, 255);
        float thrustAmount = 2.666f;
        float deceleration = 0.2666f;
        float maxVelocity = 42.666f;
        #endregion
        #region Properties
        public List<Shot> Shots { get => shots; }
        #endregion
        #region Constructor
        public Player(Game game, Camera camera) : base(game, camera)
        {
            CameraRef = camera;
            flame = new VectorModel(game, camera);
            flameTimer = new Timer(game, 0.015f);

            for (int i = 0; i < 4; i++)
            {
                shots.Add(new Shot(game, camera));
            }
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

        }

        public void LoadAssets()
        {
            base.LoadContent();
            PO.Radius = LoadVectorModel("PlayerShip", color);
            flame.LoadVectorModel("PlayerFlame", color);
        }

        public void BeginRun()
        {
            flame.PO.AddAsChildOf(PO);
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (Enabled)
            {
                Position = Core.WrapSideToSide(Core.WrapTopBottom(Position, Core.ScreenHeight), Core.ScreenWidth);
                GetKeys();
            }
        }
        #endregion
        #region Public Methods

        #endregion
        #region Private Methods
        void GetKeys()
        {
            float rotationAmound = MathHelper.Pi;

            if (Core.KeyDown(Keys.W) || Core.KeyDown(Keys.Up))
            {
                ThrustOn();
            }
            else
            {
                ThrustOff();
            }

            if (Core.KeyDown(Keys.A) || Core.KeyDown(Keys.Left))
            {
                PO.RotationVelocity.Z = rotationAmound;
            }
            else if (Core.KeyDown(Keys.D) || Core.KeyDown(Keys.Right))
            {
                PO.RotationVelocity.Z = -rotationAmound;
            }
            else
            {
                PO.RotationVelocity.Z = 0;
            }

            if (Core.KeyPressed(Keys.Down))
            {
                HyperSpace();
            }

            if (Core.KeyPressed(Keys.LeftControl) || Core.KeyPressed(Keys.Space))
            {
                Fire();
            }
        }

        void ThrustOn()
        {
            if (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) < maxVelocity)
            {
                Acceleration = Core.VelocityFromAngleZ(Rotation.Z, thrustAmount);
            }
            else
            {
                ThrustOff();
            }

            if (flameTimer.Elapsed)
            {
                flame.Enabled = !flame.Enabled;
                flameTimer.Reset();
            }

            flame.PO.UpdateChild();
            flame.Transform();
        }

        void ThrustOff()
        {
            Acceleration = -Velocity * deceleration;
            //Acceleration = Vector3.Zero;
            flame.Enabled = false;
        }
        void HyperSpace()
        {
            X = Core.RandomMinMax(-Core.ScreenWidth, Core.ScreenWidth);
            Y = Core.RandomMinMax(-Core.ScreenHeight, Core.ScreenHeight);
            Velocity = Vector3.Zero;
        }

        void Fire()
        {
            Vector3 dir = Core.VelocityFromAngleZ(Rotation.Z, 26.66f);
            Vector3 offset = Core.VelocityFromAngleZ(Rotation.Z, PO.Radius);

            foreach (Shot shot in shots)
            {
                if (!shot.Enabled)
                {
                    shot.Spawn(Position + offset, dir + (Velocity * 0.75f), 1.25f);
                    break;
                }
            }
        }
        #endregion
    }
}
