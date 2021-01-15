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
        List<Shot> shotList = new List<Shot>();
        List<Line> lineList = new List<Line>();
        Vector3[] dotVerts;
        Camera cameraRef;
        VectorModel flame;
        Timer flameTimer;
        SoundEffect fireSound;
        SoundEffectInstance thrustSound;
        SoundEffect explodeSound;
        Color color = new Color(175, 175, 255);
        float thrustAmount = 2.666f;
        float deceleration = 0.2666f;
        float maxVelocity = 42.666f;
        #endregion
        #region Properties
        public List<Shot> Shots { get => shotList; }
        public new Color Color { get => color; }
        public Vector3[] DotVerts { set => dotVerts = value; }
        #endregion
        #region Constructor
        public Player(Game game, Camera camera) : base(game, camera)
        {
            cameraRef = camera;
            flame = new VectorModel(game, camera);
            flameTimer = new Timer(game, 0.015f);

            for (int i = 0; i < 4; i++)
            {
                shotList.Add(new Shot(game, camera));
            }
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

        }

        public new void LoadContent()
        {
            base.LoadContent();
            LoadVectorModel("PlayerShip", color);
            flame.LoadVectorModel("PlayerFlame", color);
            fireSound = Core.LoadSoundEffect("PlayerFire");
            thrustSound = Core.LoadSoundEffectInstance("Thrust2");
            explodeSound = Core.LoadSoundEffect("PlayerExplode");
            FileIO fileIO = new FileIO();
        }

        public void BeginRun()
        {
            flame.PO.AddAsChildOf(PO);
            flame.Enabled = false;
            Enabled = false;

            foreach (Shot shot in shotList)
            {
                shot.InitializePoints(dotVerts, Color.White, "Player Shot");
            }
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Position = Core.WrapSideToSide(Core.WrapTopBottom(Position, Core.ScreenHeight),
                Core.ScreenWidth);
            GetKeys();
        }
        #endregion
        #region Public Methods
        public new void Hit()
        {
            explodeSound.Play();
            SpawnExplosion();
            Main.instance.TheUFO.TheUFO.Reset();
            flame.Enabled = false;
            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            Acceleration = Vector3.Zero;
            Enabled = false;
        }

        public bool CheckDoneExploding()
        {
            foreach (Line line in lineList)
            {
                if (line.Enabled)
                {
                    return false;
                }
            }

            return true;
        }
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
                if (thrustSound.State != SoundState.Playing)
                {
                    thrustSound.Play();
                }

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
            flame.UpdateMatrix();
        }

        void ThrustOff()
        {
            Acceleration = -Velocity * deceleration;
            thrustSound.Stop();
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

            foreach (Shot shot in shotList)
            {
                if (!shot.Enabled)
                {
                    fireSound.Play(0.25f, 0, 0);
                    shot.Spawn(Position + offset, dir + (Velocity * 0.75f), 1.25f);
                    break;
                }
            }
        }

        void SpawnExplosion()
        {
            int count = Core.RandomMinMax(6, 8);
            float speed = 0.05f;

            for (int c = 0; c < count; c++)
            {
                bool spawnDot = true;
                int line = lineList.Count;

                for (int i = 0; i > line; line++)
                {
                    if (!lineList[i].Enabled)
                    {
                        line = i;
                        spawnDot = false;
                        break;
                    }
                }

                if (spawnDot)
                {
                    lineList.Add(new Line(Game, cameraRef));
                    lineList.Last().BeginRun();
                }

                float life = Core.RandomMinMax(1.1f, 3.5f);
                Vector3 offset = new Vector3(Core.RandomMinMax(-0.8666f, 0.8666f),
                    Core.RandomMinMax(-0.8666f, 0.8666f), 0);
                Vector3 rotation = new Vector3(0, 0, Core.RandomMinMax(0.1f, MathHelper.Pi));
                Vector3 rotationVelocity = new Vector3(0, 0, Core.RandomMinMax(-1.15f, 1.15f));
                Vector3 velocity = new Vector3(Core.RandomMinMax(-speed, speed),
                    Core.RandomMinMax(-speed, speed), 0);
                lineList[line].Spawn(Position + offset, velocity, rotation, rotationVelocity, life);
            }
        }
        #endregion
    }
}
