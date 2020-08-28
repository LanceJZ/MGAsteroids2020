﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;

namespace Asteroids2020.Entities
{
    public class Shot : VectorModel
    {
        #region Fields
        Camera CameraRef;
        Timer life;
        #endregion
        #region Properties

        #endregion
        #region Constructor
        public Shot(Game game, Camera camera) : base(game, camera)
        {
            CameraRef = camera;
            life = new Timer(game);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();
            Enabled = false;
            float crossSize = 0.025f;
            Vector3[] crossVertex = { new Vector3(-crossSize, crossSize, 0), new Vector3(crossSize, -crossSize, 0),
                new Vector3(crossSize, crossSize, 0), new Vector3(-crossSize, -crossSize, 0) };
            InitializePoints(crossVertex, Color.White);
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

                if (life.Elapsed)
                {
                    Enabled = false;
                }
            }
        }
        #endregion
        #region Public Methods
        public void Spawn(Vector3 position, Vector3 velocity, float timer)
        {
            Spawn(position, velocity);
            life.Reset(timer);
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
