using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;
using Asteroids2020.Entities;
using System.ComponentModel;

public enum GameState
{
    PlayerHit,
    Over,
    InPlay,
    Pause,
    HighScore,
    MainMenu
};

public struct HighScore
{
    public uint score;
    public string name;
}

namespace Asteroids2020
{
    public class GameLogic : GameComponent
    {
        Camera _camera;
        VectorModel cross;
        Player player;
        RockManager rockManager;
        UFOManager ufoManager;
        Timer highScoreListTimer;
        FileIO fileIO;
        List<VectorModel> playerShipModels = new List<VectorModel>();
        HighScore[] highScoreArray = new HighScore[10];
        SpriteFont hyper20Font;
        SpriteFont hyper16Font;
        SpriteFont hyper8Font;
        SoundEffect bonusSound;
        Vector3[] dotVerts;
        Vector2 scorePosition = new Vector2();
        Vector2 highScorePosition = new Vector2();
        Vector2 highScoreListPosition = new Vector2();
        Vector2 highScoreInstructionsPosition = new Vector2();
        Vector2 highScoreLettersPosition = new Vector2();
        Vector2 copyPosition = new Vector2();
        Vector2 gameoverPosition = new Vector2();
        GameState _gameMode = GameState.Over;
        string scoreText;
        string highScoreText;
        string newHighScoreEntryText = "";
        string highScoresText = "High Scores";
        string[] highScoreInstructions = new string[4];
        string copyRightText = "(c) 1979 Atari inc"; //©
        string gameOverText = "1 coin 1 play";
        string fileNameHighScoreList = "HighScoreList.sav";
        char[] highScoreSelectedLetters = new char[3];
        uint score = 0;
        uint highScore = 0;
        uint bonusLifeAmount = 10000;
        uint bonusLifeScore = 0;
        uint wave = 0;
        int lives = 0;
        int highScoreSelectedSpace;
        int newHighScorePosition;
        bool displayHighScoreList = true;

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

            highScoreListTimer = new Timer(game);
            fileIO = new FileIO();

            game.Components.Add(this);
        }
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            highScoreInstructions[0] = "Your score is one of the ten best";
            highScoreInstructions[1] = "Please enter your initials";
            highScoreInstructions[2] = "Push rotate to select letter";
            highScoreInstructions[3] = "Push hyperspace when letter is correct";
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

            dotVerts = fileIO.ReadVectorModelFile("Dot");
            bonusSound = Core.LoadSoundEffect("ExtraShip");
        }

        public void BeginRun()
        {
            rockManager.DotVerts = dotVerts;
            ufoManager.DotVerts = dotVerts;
            player.DotVerts = dotVerts;
            player.BeginRun();
            rockManager.BeginRun();
            ufoManager.BeginRun();
            cross.Enabled = false;
            highScoreText = "00";
            LoadHighScore();
            HighScoreChanged();
            copyPosition = new Vector2(Core.WindowWidth / 2 - hyper8Font.MeasureString(copyRightText).X / 2,
                Core.WindowHeight - 20);
            gameoverPosition = new Vector2(Core.WindowWidth / 2 - hyper20Font.MeasureString(gameOverText).X / 2,
                Core.WindowHeight / 1.25f);
            highScoreListPosition = new Vector2(Core.WindowWidth / 2.75f, Core.WindowHeight / 4.25f);
            highScoreInstructionsPosition = new Vector2(50, Core.WindowHeight / 4);
            highScoreLettersPosition = new Vector2(Core.WindowWidth / 2.25f, Core.WindowHeight / 1.25f);
            highScorePosition.Y = 10;
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
                if (CheckPlayerClear() && ThePlayer.CheckDoneExploding())
                {
                    _gameMode = GameState.InPlay;
                    ThePlayer.Spawn(Vector3.Zero);
                    TheUFO.TheUFO.Reset();
                }
            }

            if (_gameMode == GameState.HighScore)
            {
                NewHighScoreEntry();
            }

            if (_gameMode == GameState.Over  || _gameMode == GameState.HighScore)
            {
                if (highScoreListTimer.Elapsed)
                {
                    displayHighScoreList = !displayHighScoreList;
                    highScoreListTimer.Reset(15);
                }

                if (displayHighScoreList || _gameMode == GameState.HighScore)
                {
                    foreach (Rock rock in rockManager.Rocks)
                    {
                        rock.Visible = false;
                        rock.explodeFX = false;
                    }

                    ufoManager.TheUFO.Visible = false;
                    ufoManager.TheUFO.Shot.Visible = false;
                    ufoManager.TheUFO.explodeFX = false;
                    ufoManager.TheUFO.ResetFireTimer();
                }
                else
                {
                    foreach (Rock rock in rockManager.Rocks)
                    {
                        rock.Visible = true;
                        rock.explodeFX = true;
                    }

                    ufoManager.TheUFO.Visible = true;
                    ufoManager.TheUFO.Shot.Visible = true;
                    ufoManager.TheUFO.explodeFX = true;
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
                if (highScoreListTimer.Elapsed)
                {
                    displayHighScoreList = !displayHighScoreList;
                    highScoreListTimer.Reset(15);
                }

                if (displayHighScoreList)
                {
                    Core.SpriteBatch.DrawString(hyper20Font, highScoresText,
                        new Vector2(highScoreListPosition.X, highScoreListPosition.Y - 75), Color.White);

                    for (int i = 0; i < 10; i++)
                    {
                        Core.SpriteBatch.DrawString(hyper20Font, (1 + i).ToString(),
                            new Vector2(highScoreListPosition.X, highScoreListPosition.Y + (i * 50)),
                            Color.White);
                        Core.SpriteBatch.DrawString(hyper20Font, highScoreArray[i].name,
                            new Vector2(highScoreListPosition.X + 75, highScoreListPosition.Y + (i * 50)),
                            Color.White);
                        Core.SpriteBatch.DrawString(hyper20Font, highScoreArray[i].score.ToString(),
                            new Vector2(highScoreListPosition.X + 225, highScoreListPosition.Y + (i * 50)),
                            Color.White);
                    }
                }

                Core.SpriteBatch.DrawString(hyper20Font, gameOverText, gameoverPosition, Color.White);
            }

            if (_gameMode == GameState.HighScore)
            {
                for (int i = 0; i < 4; i++)
                {
                    Core.SpriteBatch.DrawString(hyper20Font, highScoreInstructions[i],
                        new Vector2(highScoreInstructionsPosition.X ,highScoreInstructionsPosition.Y + (i * 50)),
                        Color.White);
                }

                Core.SpriteBatch.DrawString(hyper20Font, newHighScoreEntryText,
                    highScoreLettersPosition, Color.White);
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
                bonusSound.Play();
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
                    HighScoreChanged();
                    SaveHighScore();
                }

                CheckForNewHighScore();

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

            if (Core.KeyPressed(Keys.Enter) && _gameMode == GameState.Over)
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
        void NewHighScoreEntry()
        {
            if (Core.KeyPressed(Keys.Down))
            {
                highScoreSelectedSpace++;

                if (highScoreSelectedSpace > 2)
                {
                    highScoreArray[newHighScorePosition].name = newHighScoreEntryText;
                    highScoreArray[newHighScorePosition].score = score;
                    _gameMode = GameState.Over;
                    WriteHighScoreList();
                }
                else
                {
                    highScoreSelectedLetters[highScoreSelectedSpace] = (char)65;
                }
            }
            else if (Core.KeyPressed(Keys.Left))
            {
                highScoreSelectedLetters[highScoreSelectedSpace]--;

                if (highScoreSelectedLetters[highScoreSelectedSpace] < 65)
                {
                    highScoreSelectedLetters[highScoreSelectedSpace] = (char)90;
                }
            }
            else if (Core.KeyPressed(Keys.Right))
            {
                highScoreSelectedLetters[highScoreSelectedSpace]++;

                if (highScoreSelectedLetters[highScoreSelectedSpace] > 90)
                {
                    highScoreSelectedLetters[highScoreSelectedSpace] = (char)65;
                }
            }

            newHighScoreEntryText = "";

            foreach (char letter in highScoreSelectedLetters)
            {
                newHighScoreEntryText += letter;
            }
        }

        void CheckForNewHighScore()
        {
            for (int rank = 0; rank < 10; rank++)
            {
                if (score > highScoreArray[rank].score)
                {
                    if (rank < 9)
                    {
                        HighScore[] oldScores = new HighScore[10];

                        for (int oldRank = rank; oldRank < 10; oldRank++)
                        {
                            oldScores[oldRank].score = highScoreArray[oldRank].score;
                            oldScores[oldRank].name = highScoreArray[oldRank].name;
                        }

                        for (int newRank = rank; newRank < 9; newRank++)
                        {
                            highScoreArray[newRank + 1].score = oldScores[newRank].score;
                            highScoreArray[newRank + 1].name = oldScores[newRank].name;
                        }
                    }

                    highScoreArray[rank].score = score;
                    highScoreArray[rank].name = "AAA";
                    _gameMode = GameState.HighScore;
                    highScoreSelectedLetters = "___".ToCharArray();
                    highScoreSelectedSpace = 0;
                    newHighScorePosition = rank;
                    highScoreSelectedLetters[highScoreSelectedSpace] = (char)65;
                    break;
                }
            }
        }

        void HighScoreChanged()
        {
            highScoreText = highScore.ToString();
            highScorePosition.X = Core.WindowWidth / 2 - hyper16Font.MeasureString(highScoreText).X;
        }

        bool CheckPlayerClear()
        {
            PositionedObject clearCircle = new PositionedObject(Game);
            clearCircle.Radius = Core.ScreenHeight / 2.5f;

            TheUFO.TheUFO.Reset();

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

            foreach (Rock rock in rockManager.Rocks)
            {
                rock.Visible = true;
                rock.explodeFX = true;
            }

            ufoManager.TheUFO.Visible = true;
            ufoManager.TheUFO.Shot.Visible = true;
            ufoManager.TheUFO.explodeFX = true;
        }

        void ScoreZero()
        {
            scoreText = "00";
            float textlength = hyper20Font.MeasureString(scoreText).X;
            scorePosition.X = 320 - textlength;
        }

        void SaveHighScore()
        {
            fileIO.WriteStringFile("Score.sav", highScore.ToString());
        }

        void LoadHighScore()
        {
            if (fileIO.DoesFileExist("Score.sav"))
            {
                highScore = uint.Parse(fileIO.ReadStringFile("Score.sav"));
            }

            if (fileIO.DoesFileExist(fileNameHighScoreList))
            {
                // Read High Score List into array.
                LoadandDecodeHighScores(fileNameHighScoreList);

                foreach(HighScore high in highScoreArray)
                {
                    if (highScore < high.score)
                    {
                        highScore = high.score;
                    }
                }
            }
            else
            {
                MakeNewHighScoreList();
                WriteHighScoreList();
            }
        }

        void MakeNewHighScoreList()
        {
            for(int i = 0; i < 10; i++)
            {
                highScoreArray[i].name = "AAA";
                highScoreArray[i].score = 1000;
            }
        }

        void WriteHighScoreList()
        {
            fileIO.OpenForWrite(fileNameHighScoreList);

            foreach(HighScore score in highScoreArray)
            {
                fileIO.WriteByteArray(fileIO.StringToByteArray(score.name));
                fileIO.WriteByteArray(fileIO.StringToByteArray(score.score.ToString()));
                fileIO.WriteByteArray(fileIO.StringToByteArray(":"));
            }

            fileIO.Close();
        }


        void LoadandDecodeHighScores(string fileName)
        {
            string scoreData = fileIO.ReadStringFile(fileName);

            int list = 0;
            int letter = 0;
            bool isLetter = true;
            string fromNumber = "";

            foreach (char character in scoreData)
            {
                if (character.ToString() == "\0")
                {
                    break;
                }

                if (isLetter)
                {
                    letter++;
                    highScoreArray[list].name += character;

                    if (letter == 3)
                        isLetter = false;
                }
                else
                {
                    if (character.ToString() == ":")
                    {
                        highScoreArray[list].score = uint.Parse(fromNumber);

                        list++;
                        letter = 0;
                        fromNumber = "";
                        isLetter = true;
                    }
                    else
                    {
                        fromNumber += character.ToString();
                    }
                }
            }
        }

        #endregion
    }
}
