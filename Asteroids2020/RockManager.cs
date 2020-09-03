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
    struct RockModel
    {
        public Vector3[] rock;
    }

    public class RockManager : GameComponent
    {
        #region Fields
        Camera cameraRef;
        RockModel[] rocksModels = new RockModel[4];
        Vector3[] dotVerts;
        List<Rock> rocksList = new List<Rock>();
        SoundEffect explodeSound;
        Color color = new Color(150, 150, 255);
        int largeRockAmount;
        #endregion
        #region Properties
        public List<Rock> Rocks { get => rocksList; }
        public Vector3[] DotVerts { set => dotVerts = value; }
        #endregion
        #region Constructor
        public RockManager(Game game, Camera camera) : base(game)
        {
            cameraRef = camera;
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
            FileIO modelLoader = new FileIO(Game);

            rocksModels[0].rock = modelLoader.ReadVectorModelFile("RockOne");
            rocksModels[1].rock = modelLoader.ReadVectorModelFile("RockTwo");
            rocksModels[2].rock = modelLoader.ReadVectorModelFile("RockThree");
            rocksModels[3].rock = modelLoader.ReadVectorModelFile("RockFour");
            explodeSound = Core.LoadSoundEffect("RockExplosion");
        }

        public void BeginRun()
        {
            Reset();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        #endregion
        #region Public Methods
        public void RockDistroyed(Rock rockHit)
        {
            explodeSound.Play(0.5f, 0, 0);

            switch (rockHit.size)
            {
                case GameLogic.RockSize.Large:
                    SpawnRocks(rockHit.Position, GameLogic.RockSize.Medium, 2);
                    break;
                case GameLogic.RockSize.Medium:
                    SpawnRocks(rockHit.Position, GameLogic.RockSize.Small, 2);
                    break;
                case GameLogic.RockSize.Small:
                    bool newWave = true;

                    foreach (Rock rock in rocksList)
                    {
                        if (rock.Enabled)
                        {
                            newWave = false;
                        }
                    }

                    if (newWave)
                    {
                        NewWaveSpawn();
                        return;
                    }
                    break;
            }
        }

        public void Reset()
        {
            foreach(Rock rock in rocksList)
            {
                rock.Enabled = false;
            }

            largeRockAmount = 2;
            NewWaveSpawn();
        }
        #endregion
        #region Private Methods
        void SpawnRocks(Vector3 position, GameLogic.RockSize rockSize, int count)
        {
            for (int numberOfRocks = 0; numberOfRocks < count; numberOfRocks++)
            {
                bool spawnNewRock = true;
                int rock = rocksList.Count;

                for (int i = 0; i < rock; i++)
                {
                    if (!rocksList[i].Enabled)
                    {
                        spawnNewRock = false;
                        rock = i;
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    rocksList.Add(new Rock(Game, cameraRef));
                    rocksList[rock].InitializePoints(rocksModels[Core.RandomMinMax(0, 3)].rock, color);
                    rocksList.Last().DotVerts = dotVerts; // For explosion.
                    rocksList.Last().BeginRun();
                }

                float maxSpeed = 10.666f;

                switch (rockSize)
                {
                    case GameLogic.RockSize.Large:
                        position.Y = Core.RandomMinMax(-Core.ScreenHeight, Core.ScreenHeight);
                        position.X = Core.ScreenWidth;
                        SpawnRock(rock, 1, 20, position,
                            Core.RandomMinMax(maxSpeed / 10, maxSpeed / 3), GameLogic.RockSize.Large);
                        break;
                    case GameLogic.RockSize.Medium:
                        SpawnRock(rock, 0.5f, 50, position,
                            Core.RandomMinMax(maxSpeed / 10, maxSpeed / 2), GameLogic.RockSize.Medium);
                        break;
                    case GameLogic.RockSize.Small:
                        SpawnRock(rock, 0.25f, 100, position,
                            Core.RandomMinMax(maxSpeed / 10, maxSpeed), GameLogic.RockSize.Small);
                        break;
                }
            }
        }

        void SpawnRock(int rock, float scale, int points,
            Vector3 position, float speed, GameLogic.RockSize size)
        {
            rocksList[rock].Scale = scale;
            rocksList[rock].size = size;
            rocksList[rock].Spawn(position, Core.VelocityFromAngleZ(speed));
        }

        void NewWaveSpawn()
        {
            if (largeRockAmount < 12)
            {
                largeRockAmount += 2;
            }

            SpawnRocks(Vector3.Zero, GameLogic.RockSize.Large, largeRockAmount);
            Main.instance.Wave++;
        }

        #endregion
    }
}
