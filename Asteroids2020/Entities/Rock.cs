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
        Camera CameraRef;
        public GameLogic.RockSize size = new GameLogic.RockSize();
        public float baseRadius;
        #endregion
        #region Properties
        public float Radius { get => PO.Radius; set => PO.Radius = value; }
        #endregion
        #region Constructor
        public Rock(Game game, Camera camera) : base(game, camera)
        {
            CameraRef = camera;

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

            if (PO.CirclesIntersect(ufo.PO))
            {
                ufo.Destroyed();
                Destroyed();
            }

            if (PO.CirclesIntersect(ufo.Shot.PO))
            {
                ufo.Shot.Enabled = false;
                Destroyed();
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

        void Destroyed()
        {
            Enabled = false;
            Main.instance.TheRocks.RockDistroyed(this);
        }
        #endregion
    }
}
