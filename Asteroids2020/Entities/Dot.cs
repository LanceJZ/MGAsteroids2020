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
    public class Dot : VectorModel
    {
        #region Fields
        Camera cameraRef;
        Timer lifeTimer;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Dot(Game game, Camera camera) : base(game, camera)
        {
            cameraRef = camera;
            lifeTimer = new Timer(game);
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

            if (lifeTimer.Elapsed)
            {
                Enabled = false;
            }
        }
        #endregion
        #region Public Methods
        public void Spawn(Vector3 position, Vector3 velocity, float life)
        {
            base.Spawn(position, velocity);
            lifeTimer.Reset(life);
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
