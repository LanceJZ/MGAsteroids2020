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
        public int points;
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
            Player player = GameLogic.instance.ThePlayer;
            UFO ufo = GameLogic.instance.TheUFO.TheUFO;

            foreach (Shot shot in player.Shots)
            {
                if (PO.CirclesIntersect(shot.PO) && shot.Enabled)
                {
                    shot.Enabled = false;
                    Destroyed();
                } 
            }

            if (PO.CirclesIntersect(player.PO) && player.Enabled)
            {
                Destroyed();
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

        void Destroyed()
        {
            Enabled = false;
            GameLogic.instance.TheRocks.RockDistroyed(this);
        }
        #endregion
    }
}
