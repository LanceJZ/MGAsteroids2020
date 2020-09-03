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
    public class Rock : VectorModel
    {
        #region Fields
        Explode explosion;
        Camera cameraRef;
        public GameLogic.RockSize size = new GameLogic.RockSize();
        public float baseRadius;
        #endregion
        #region Properties
        public float Radius { get => PO.Radius; set => PO.Radius = value; }
        public Vector3[] DotVerts { set => explosion.DotVerts = value; }
        #endregion
        #region Constructor
        public Rock(Game game, Camera camera) : base(game, camera)
        {
            cameraRef = camera;
            explosion = new Explode(game, camera);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

        }

        public void BeginRun()
        {
            explosion.AddAsChildOf(PO);
            explosion.Color = new Color(100, 100, 166);
            explosion.Speed = 4.666f;
            explosion.Maxlife = 0.8f;
            explosion.Minlife = 0.25f;
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Enabled)
            {
                Position = Core.WrapSideToSide(Core.WrapTopBottom(Position, Core.ScreenHeight), Core.ScreenWidth);
                CheckCollusion();
            }
        }
        #endregion
        #region Public Methods
        #endregion
        #region Private Methods
        void CheckCollusion()
        {
            Player player = Main.instance.ThePlayer;
            UFO ufo = Main.instance.TheUFO.TheUFO;

            foreach (Shot shot in player.Shots)
            {
                if (PO.CirclesIntersect(shot.PO))
                {
                    shot.Enabled = false;
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

            if (PO.CirclesIntersect(ufo.PO))
            {
                ufo.Explode();
                Explode();
            }

            if (PO.CirclesIntersect(ufo.Shot.PO))
            {
                ufo.Shot.Enabled = false;
                Explode();
            }
        }

        void PlayerScored()
        {
            uint points = 0;

            switch(size)
            {
                case GameLogic.RockSize.Large:
                    points = 20;
                    break;
                case GameLogic.RockSize.Medium:
                    points = 50;
                    break;
                case GameLogic.RockSize.Small:
                    points = 100;
                    break;
            }

            Main.instance.PlayerScore(points);
        }

        void Explode()
        {
            Enabled = false;
            Main.instance.TheRocks.RockDistroyed(this);
            explosion.Spawn(Core.RandomMinMax(20, 40));
        }
        #endregion
    }
}
