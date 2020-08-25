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
    public class Player : Vector
    {
        #region Fields
        GameLogic LogicRef;
        Camera CameraRef;
        Vector flame;
        Timer flameTimer;
        Color color = new Color(175, 175, 255);
        float thrustAmount = 0.0533f;
        float deceleration = 0.002666f;
        float maxPS = 32.666f;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Player(Game game, Camera camera, GameLogic gameLogic) : base(game, camera)
        {
            LogicRef = gameLogic;
            CameraRef = camera;
            flame = new Vector(game, camera);
            flameTimer = new Timer(game, 0.015f);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

            LoadContent();
            BeginRun();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            PO.Radius = InitializePoints(ReadFile("PlayerShip"), color);
            flame.InitializePoints(flame.ReadFile("PlayerFlame"), color);
        }

        public void BeginRun()
        {
            flame.PO.AddAsChildOf(PO, true, true);
            
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (Enabled)
            {
                KeyInput();
                Position = Core.WrapSideToSide(Core.WrapTopBottom(Position, Core.ScreenHeight), Core.ScreenWidth);
            }
        }
        #endregion
        #region Private Methods
        void KeyInput()
        {
            float rotationAmound = MathHelper.Pi;

            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                ThrustOn();
            }
            else
            {
                ThrustOff();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                PO.RotationVelocity.Z = rotationAmound;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                PO.RotationVelocity.Z = -rotationAmound;
            }
            else
                PO.RotationVelocity.Z = 0;

        }

        void ThrustOn()
        {
            if (Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) < maxPS)
            {
                Velocity += Core.VelocityFromAngleZ(Rotation.Z, thrustAmount);
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
            Velocity -= Velocity * deceleration;
            flame.Enabled = false;
        }
        #endregion
    }
}
