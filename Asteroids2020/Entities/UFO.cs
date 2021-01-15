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
        Explode explosion;
        Camera cameraRef;
        Color color = new Color(175, 175, 255);
        Timer fireTimer;
        Timer vectorTimer;
        SoundEffect shotSound;
        SoundEffect explodeSound;
        SoundEffectInstance engineLSound;
        SoundEffectInstance engineSSound;
        public GameLogic.UFOType type;
        float speed =  2.666f;
        float shotSpeed = 16.666f;
        public bool explodeFX = true;
        #endregion
        #region Properties
        public Shot Shot { get => shot; }
        public Vector3[] DotVerts { set => explosion.DotVerts = value; }
        #endregion
        #region Constructor
        public UFO(Game game, Camera camera) : base(game, camera)
        {
            shot = new Shot(game, camera);
            cameraRef = camera;
            fireTimer = new Timer(game);
            vectorTimer = new Timer(game);
            explosion = new Explode(game, camera);
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
        public new void LoadContent()
        {
            base.LoadContent();
            LoadVectorModel("UFO", color);
            shotSound = Core.LoadSoundEffect("UFOFire");
            explodeSound = Core.LoadSoundEffect("UFOExplosion");
            engineLSound = Core.LoadSoundEffectInstance("UFOLarge");
            engineSSound = Core.LoadSoundEffectInstance("UFOSmall");
            shot.LoadVectorModel("Dot"); //convert to DotVerts.
        }

        public void BeginRun()
        {
            explosion.AddAsChildOf(PO, false, true);
            explosion.Color = new Color(100, 100, 180);
            explosion.Speed = 4.7666f;
            explosion.Maxlife = 0.9f;
            explosion.Minlife = 0.5f;
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
                    Reset();
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

                CheckCollision();

                if (Main.instance.CurrentMode == GameState.InPlay)
                {
                    switch (type)
                    {
                        case GameLogic.UFOType.Large:
                            if (engineLSound.State != SoundState.Playing)
                            {
                                engineLSound.Play();
                            }
                            break;
                        case GameLogic.UFOType.Small:
                            if (engineSSound.State != SoundState.Playing)
                            {
                                engineSSound.Play();
                            }
                            break;
                    }
                }
            }
        }
        #endregion
        #region Public Methods
        public override void Spawn(Vector3 position)
        {
            base.Spawn(position);

            switch (type)
            {
                case GameLogic.UFOType.Large:
                    speed = 2.666f;
                    break;
                case GameLogic.UFOType.Small:
                    speed = 3.666f;
                    break;
            }

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

        public void Explode()
        {
            if (Main.instance.CurrentMode == GameState.InPlay)
            {
                explodeSound.Play();
            }

            Reset();

            if (explodeFX)
            {
                explosion.Spawn(Core.RandomMinMax(15, 35));
            }
        }

        public void Reset()
        {
            Main.instance.TheUFO.ResetTimer();
            Enabled = false;
        }
        #endregion
        #region Private Methods
        void CheckCollision()
        {
            Player player = Main.instance.ThePlayer;

            foreach (Shot shot in player.Shots)
            {
                if (PO.CirclesIntersect(shot.PO))
                {
                    Explode();
                    PlayerScored();
                }
            }

            if (PO.CirclesIntersect(player.PO))
            {
                Explode();
                PlayerScored();
                Main.instance.PlayerHit();
            }

            if (shot.PO.CirclesIntersect(player.PO))
            {
                shot.Enabled = false;
                Main.instance.PlayerHit();
            }
        }

        public void ResetFireTimer()
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
                if (Main.instance.CurrentMode == GameState.InPlay)
                {
                    shotSound.Play();
                }

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
