using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;
using Asteroids2020.Entities;

namespace Asteroids2020
{
    public class UFOManager : GameComponent
    {
        #region Fields
        Camera CameraRef;
        UFO theUFO;
        Timer spawnTimer;
        uint spawnCount;
        #endregion
        #region Properties
        public UFO TheUFO { get => theUFO; }
        #endregion
        #region Constructor
        public UFOManager(Game game, Camera camera) : base(game)
        {
            CameraRef = camera;
            theUFO = new UFO(game, camera);
            spawnTimer = new Timer(game);

            game.Components.Add(this);
        }
        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

        }

        public void LoadContent()
        {
            theUFO.LoadAssets();
        }

        public void BeginRun()
        {
            ResetTimer();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (spawnTimer.Elapsed)
            {
                ResetTimer();

                if (!theUFO.Enabled)
                {
                    Spawn();
                }
            }
        }
        #endregion
        #region Public Methods
        public void ResetTimer()
        {
            spawnTimer.Reset(Core.RandomMinMax(10 - Main.instance.Wave * 0.1f,
                10.15f + Main.instance.Wave * 0.1f));
        }
        public void Reset()
        {
            theUFO.Enabled = false;
            ResetTimer();
        }
        #endregion
        #region Private Methods
        void Spawn()
        {
            float spawnPercent = (float)(Math.Pow(0.915, spawnCount / (Main.instance.Wave + 1)) * 100);
            Vector3 position = new Vector3();

            if (Core.RandomMinMax(0, 99) < spawnPercent - Main.instance.Score / 400)
            {
                theUFO.type = GameLogic.UFOType.Large;
                theUFO.Scale = 1;
                theUFO.PO.Radius = theUFO.LargeRadius;
            }
            else
            {
                theUFO.type = GameLogic.UFOType.Small;
                theUFO.Scale = 0.5f;
                theUFO.PO.Radius = theUFO.LargeRadius / 2;
            }

            position.Y = Core.RandomMinMax(-Core.ScreenHeight * 0.25f, Core.ScreenHeight * 0.25f);

            if (Core.RandomMinMax(1, 10) > 5)
            {
                position.X = -Core.ScreenWidth;
            }
            else
            {
                position.X = Core.ScreenWidth;
            }

            theUFO.Spawn(position);
        }
        #endregion
    }
}
