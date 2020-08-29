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
    PlayerHit,
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
        Camera _camera;
        GameState _gameMode = GameState.Over;
        VectorModel cross;
        Player player;
        RockManager rockManager;
        UFOManager ufoManager;
        List<VectorModel> playerShipModels = new List<VectorModel>();
        SpriteFont hyper20Font;
        SpriteFont hyper16Font;
        SpriteFont hyper8Font;
        string scoreText;
        string highScoreText;
        string copyRightText = "(c) 1979 Atari inc"; //©
        string gameOverText = "Game Over";
        Vector2 scorePosition = new Vector2();
        Vector2 highScorePosition = new Vector2();
        Vector2 copyPosition = new Vector2();
        Vector2 gameoverPosition = new Vector2();
        uint score = 0;
        uint highScore = 0;
        uint bonusLifeAmount = 10000;
        uint bonusLifeScore = 0;
        int lives = 0;
        uint wave = 0;

        public GameState CurrentMode { get => _gameMode; set => _gameMode = value; }
        public Player ThePlayer { get => player; }
        public RockManager TheRocks { get => rockManager; }
        public UFOManager TheUFO { get => ufoManager; }
        public uint Score { get => score; }
        public uint Wave { get => wave; set => wave = value; }
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
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

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

            hyper20Font = Game.Content.Load<SpriteFont>("Hyperspace20");
            hyper16Font = Game.Content.Load<SpriteFont>("Hyperspace16");
            hyper8Font = Game.Content.Load<SpriteFont>("Hyperspace8");
        }

        public void BeginRun()
        {
            player.BeginRun();
            rockManager.BeginRun();
            ufoManager.BeginRun();
            cross.Enabled = false;
            highScoreText = "00";
            highScorePosition.X = Core.WindowWidth / 2 - hyper16Font.MeasureString(highScoreText).X;
            copyPosition = new Vector2(Core.WindowWidth / 2 - hyper8Font.MeasureString(copyRightText).X / 2,
                Core.WindowHeight - 20);
            gameoverPosition = new Vector2(Core.WindowWidth / 2 - hyper20Font.MeasureString(gameOverText).X / 2,
                Core.WindowHeight / 2 - hyper20Font.MeasureString(gameOverText).Y);
            ScoreZero();
        }

        public void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetKeys();

            if (_gameMode == GameState.PlayerHit)
            {
                if (CheckPlayerClear())
                {
                    _gameMode = GameState.InPlay;
                    ThePlayer.Spawn(Vector3.Zero);
                }
            }
        }

        public void Draw()
        {
            Core.SpriteBatch.Begin();
            Core.SpriteBatch.DrawString(hyper20Font, scoreText, scorePosition, Color.White);
            Core.SpriteBatch.DrawString(hyper16Font, highScoreText, highScorePosition, Color.White);
            Core.SpriteBatch.DrawString(hyper8Font, copyRightText, copyPosition, Color.White);

            if (_gameMode == GameState.Over)
            {
                Core.SpriteBatch.DrawString(hyper20Font, gameOverText, gameoverPosition, Color.White);
            }

            Core.SpriteBatch.End();
        }

        public void PlayerScore(uint points)
        {
            score += points;
            scoreText = score.ToString();
            float textlength = hyper20Font.MeasureString(scoreText).X;
            scorePosition.X = 320 - textlength;

            if (score > bonusLifeScore)
            {
                lives++;
                bonusLifeScore += bonusLifeAmount;
                PlayerShipDesplay();
            }
        }

        public void PlayerHit()
        {
            ThePlayer.Hit();
            lives--;

            if (lives < 0)
            {
                _gameMode = GameState.Over;

                if (score > highScore)
                {
                    highScore = score;
                    highScoreText = highScore.ToString();
                    highScorePosition.X = Core.WindowWidth / 2 - hyper16Font.MeasureString(highScoreText).X;
                }

                return;
            }

            PlayerShipDesplay();
            _gameMode = GameState.PlayerHit;

        }

        public void GetKeys()
        {
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

            if (Core.KeyPressed(Keys.Enter) && !ThePlayer.Enabled)
            {
                ResetGame();
            }

            if (cross.Enabled)
            {
                if (Core.KeyPressed(Keys.Enter))
                {
                    System.Diagnostics.Debug.WriteLine("X: " + cross.X.ToString() +
                        " " + "Y: " + cross.Y.ToString());
                }

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
        #endregion
        #region Private Methods
        bool CheckPlayerClear()
        {
            PositionedObject clearCircle = new PositionedObject(Game);
            clearCircle.Radius = Core.ScreenHeight / 2.5f;

            TheUFO.TheUFO.Destroyed();

            if (TheUFO.TheUFO.Shot.PO.CirclesIntersect(clearCircle))
            {
                return false;
            }

            foreach (Rock rock in TheRocks.Rocks)
            {
                if (rock.PO.CirclesIntersect(clearCircle))
                {
                    return false;
                }
            }

            return true;
        }

        void PlayerShipDesplay()
        {
            foreach (VectorModel ship in playerShipModels)
            {
                ship.Enabled = false;
            }

            for (int i = 0; i < lives; i++)
            {
                bool newShip = true;
                int thisShip = 0;

                for (int j = 0; j < playerShipModels.Count; j++)
                {
                    if (!playerShipModels[j].Enabled)
                    {
                        thisShip = j;
                        newShip = false;
                        playerShipModels[j].Enabled = true;
                        break;
                    }
                }

                if (newShip)
                {
                    playerShipModels.Add(new VectorModel(Game, _camera));
                    playerShipModels.Last().InitializePoints(ThePlayer.VertexArray, ThePlayer.Color);
                    playerShipModels.Last().PO.Rotation.Z = MathHelper.PiOver2;
                }
            }

            float line = Core.ScreenHeight - (player.PO.Radius * 5);
            float column = -15;

            for(int i = 0; i < lives; i++)
            {
                if (playerShipModels[i].Enabled)
                {
                    playerShipModels[i].Position = new Vector3(column - (i * (ThePlayer.PO.Radius + 0.25f)),
                        line, 0);
                }
            }
        }

        void ResetGame()
        {
            _gameMode = GameState.InPlay;
            lives = 4;
            score = 0;
            ScoreZero();
            bonusLifeScore = bonusLifeAmount;
            ThePlayer.Spawn(Vector3.Zero);
            ufoManager.Reset();
            rockManager.Reset();
            PlayerShipDesplay();
        }

        void ScoreZero()
        {
            scoreText = "00";
            float textlength = hyper20Font.MeasureString(scoreText).X;
            scorePosition.X = 320 - textlength;
        }
        #endregion
    }
}
