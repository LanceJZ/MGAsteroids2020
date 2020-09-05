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
    public class Line : VectorModel
    {
        #region Fields
        Camera cameraRef;
        Timer lifeTimer;
        Color color = new Color(125, 125, 150);
        #endregion
        #region Properties
        #endregion
        #region Constructor
        public Line(Game game, Camera camera) : base(game, camera)
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
            float size = Core.RandomMinMax(0.1666f, 0.1666f);
            Vector3[] lineVerts = new Vector3[2];
            lineVerts[0] = new Vector3(-size, 0, 0);
            lineVerts[1] = new Vector3(size, 0, 0);
            InitializePoints(lineVerts, color);
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
        public void Spawn(Vector3 position, Vector3 rotation, Vector3 rotationVelocity,
            Vector3 velocity, float life)
        {
            base.Spawn(position, rotation, rotationVelocity, velocity);
            lifeTimer.Reset(life);
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
