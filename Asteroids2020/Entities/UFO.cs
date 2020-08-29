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
    public class UFO : VectorModel
    {
        #region Fields
        Shot shot;
        Camera CameraRef;
        Color color = new Color(175, 175, 255);
        Timer fireTimer;
        Timer vectorTimer;
        float largeRadius;
        float speed =  2.666f;
        float shotSpeed = 16.666f;
        public GameLogic.UFOType type;
        #endregion
        #region Properties
        public float LargeRadius { get => largeRadius; }
        public Shot Shot { get => shot; }
        #endregion
        #region Constructor
        public UFO(Game game, Camera camera) : base(game, camera)
        {
            shot = new Shot(game, camera);
            CameraRef = camera;
            fireTimer = new Timer(game);
            vectorTimer = new Timer(game);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();
            Enabled = false;
        }

        // The Y
        //0.25 is 0.667 - Top new 0.6
        //0.07 is 0.187 - Upper new 0.165
        //-0.094 is -0.251 - Middle new -0.2
        //-0.258 is -0.688 - Bottom new -0.6

        // The X
        //0.07 is 0.187 - top
        //0.164 is 0.437 - upper
        //0.466 is 1.189 - mid
        //0.188 is 0.501 - bottom
        public void LoadAssets()
        {
            base.LoadContent();
            largeRadius = LoadVectorModel("UFO", color);
        }

        public void BeginRun()
        {

        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Enabled)
            {
                Position = Core.WrapTopBottom(Position, Core.ScreenHeight);

                if (PO.OffScreenSide())
                {
                    Destroyed();
                }

                if (vectorTimer.Elapsed)
                {
                    ResetVectorTimer();
                    ChangeVector();
                }

                if (fireTimer.Elapsed)
                {
                    ResetFireTimer();
                    Fire();
                }

                CheckCollusion();
            }
        }
        #endregion
        #region Public Methods
        public override void Spawn(Vector3 position)
        {
            base.Spawn(position);

            if (X < 0)
            {
                PO.Velocity.X = speed;
            }
            else
            {
                PO.Velocity.X = -speed;
            }

            fireTimer.Reset();
            vectorTimer.Reset();

        }

        public void Destroyed()
        {
            Main.instance.TheUFO.ResetTimer();
            Enabled = false;
        }
        #endregion
        #region Private Methods
        void CheckCollusion()
        {
            Player player = Main.instance.ThePlayer;

            foreach (Shot shot in player.Shots)
            {
                if (PO.CirclesIntersect(shot.PO))
                {
                    Destroyed();
                    PlayerScored();
                }
            }

            if (PO.CirclesIntersect(player.PO))
            {
                Destroyed();
                PlayerScored();
                Main.instance.PlayerHit();
            }

            if (shot.PO.CirclesIntersect(player.PO))
            {
                shot.Enabled = false;
                Main.instance.PlayerHit();
            }
        }

        void ResetFireTimer()
        {
            fireTimer.Reset(2.75f);
        }

        void ResetVectorTimer()
        {
            vectorTimer.Reset(3.15f);
        }

        void ChangeVector()
        {
            if (Core.RandomMinMax(1, 10) < 5)
            {
                if ((int)(Velocity.Y) == 0)
                {
                    if (Core.RandomMinMax(1, 10) < 5)
                    {
                        PO.Velocity.Y = speed;
                    }
                    else
                    {
                        PO.Velocity.Y = -speed;
                    }
                }
                else
                {
                    PO.Velocity.Y = 0;
                }
            }
        }

        void Fire()
        {
            float angle = 0;

            switch (type)
            {
                case GameLogic.UFOType.Large:
                    angle = Core.RandomRadian();
                    break;
                case GameLogic.UFOType.Small:
                    angle = AimedFire();
                    break;
            }

            if (!shot.Enabled)
            {
                shot.Spawn(Position + Core.VelocityFromAngleZ(angle, PO.Radius),
                    Core.VelocityFromAngleZ(angle, shotSpeed), 1.45f);
            }
        }

        float AimedFire()
        {
            float percentChance = 0.25f - (Main.instance.Score * 0.00001f);

            if (percentChance < 0)
            {
                percentChance = 0;
            }

            return PO.AngleFromVectorsZ(Main.instance.ThePlayer.Position) +
                Core.RandomMinMax(-percentChance, percentChance);
        }

        void PlayerScored()
        {
            uint points = 0;

            switch(type)
            {
                case GameLogic.UFOType.Large:
                    points = 200;
                    break;
                case GameLogic.UFOType.Small:
                    points = 1000;
                    break;
            }

            Main.instance.PlayerScore(points);
        }
        #endregion
    }
}
