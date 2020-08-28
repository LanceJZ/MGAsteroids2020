using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;
using Asteroids2020.Entities;

public enum GameState
{
    Over,
    InPlay,
    Pause,
    HighScore,
    MainMenu
};

namespace Asteroids2020
{
    public class GameLogic : GameComponent
    {
        public static GameLogic instance;
        Camera _camera;
        GameState _gameMode = GameState.InPlay;
        VectorModel cross;
        Player player;
        RockManager rockManager;
        UFOManager ufoManager;
        int score = 0;
        int highScore = 0;
        int bonusLifeAmount = 10000;
        int bonusLifeScore = 0;
        int lives = 0;
        int wave = 0;

        public GameState CurrentMode { get => _gameMode; }
        public Player ThePlayer { get => player; }
        public RockManager TheRocks { get => rockManager; }
        public UFOManager TheUFO { get => ufoManager; }
        public int Score { get => score; }
        public int Wave { get => wave; set => wave = value; }
        public int Lives { get => lives; }
        public enum RockSize
        {
            Small,
            Medium,
            Large
        };

        public enum UFOType
        {
            Small,
            Large
        }

        public GameLogic(Game game, Camera camera) : base(game)
        {
            _camera = camera;
            cross = new VectorModel(Game, camera);

            player = new Player(game, camera);
            rockManager = new RockManager(game, camera);
            ufoManager = new UFOManager(game, camera);

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (instance == null)
            {
                instance = this;
            }

            float crossSize = 0.5f;
            Vector3[] crossVertex = { new Vector3(crossSize, 0, 0), new Vector3(-crossSize, 0, 0),
                new Vector3(0, crossSize, 0), new Vector3(0, -crossSize, 0) };
            cross.InitializePoints(crossVertex, Color.White);

            // The X: 27.63705 Y: -20.711943
            Core.ScreenWidth = 27.63705f;
            Core.ScreenHeight = 20.711943f;
        }

        public void LoadContent()
        {
            player.LoadAssets();
            rockManager.LoadContent();
            ufoManager.LoadContent();
        }

        public void BeginRun()
        {
            player.BeginRun();
            rockManager.BeginRun();
            ufoManager.BeginRun();
            cross.Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetKeys();
        }

        public void GetKeys()
        {
            if (Core.KeyPressed(Keys.Enter))
            {
                if (cross.Enabled)
                {
                    System.Diagnostics.Debug.WriteLine("X: " + cross.X.ToString() +
                        " " + "Y: " + cross.Y.ToString());
                }
            }

            if (Core.KeyPressed(Keys.End))
            {
                cross.Enabled = !cross.Enabled;
                cross.Position = Vector3.Zero;
            }

            if (Core.KeyPressed(Keys.Pause))
            {
                if (CurrentMode == GameState.InPlay)
                {
                    _gameMode = GameState.Pause;
                }
                else if (CurrentMode == GameState.Pause)
                {
                    _gameMode = GameState.InPlay;
                }
            }

            if (cross.Enabled)
            {
                if (Core.KeyDown(Keys.W))
                {
                    cross.PO.Velocity.Y += 0.125f;
                }
                else if (Core.KeyDown(Keys.S))
                {
                    cross.PO.Velocity.Y -= 0.125f;
                }
                else
                {
                    cross.PO.Velocity.Y = 0;
                }

                if (Core.KeyDown(Keys.D))
                {
                    cross.PO.Velocity.X += 0.125f;
                }
                else if (Core.KeyDown(Keys.A))
                {
                    cross.PO.Velocity.X -= 0.125f;
                }
                else
                {
                    cross.PO.Velocity.X = 0;
                }
            }
        }
    }
}
