using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;

public enum GameState
{
    Over,
    InPlay,
    HighScore,
    MainMenu
};

namespace Asteroids2020
{
    public class GameLogic : GameComponent
    {
        Camera _camera;

        GameState _gameMode = GameState.MainMenu;
        KeyboardState _oldKeyState;

        public GameState CurrentMode { get => _gameMode; }

        public GameLogic(Game game, Camera camera) : base(game)
        {
            _camera = camera;

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

        }

        public void BeginRun()
        {

        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState KBS = Keyboard.GetState();

            if (KBS != _oldKeyState)
            {
                if (KBS.IsKeyDown(Keys.Space))
                {

                }
            }

            _oldKeyState = Keyboard.GetState();

            if (KBS.IsKeyDown(Keys.Up))
            {

            }
            else if (KBS.IsKeyDown(Keys.Down))
            {

            }
            else
            {

            }

            if (KBS.IsKeyDown(Keys.Left))
            {

            }
            else if (KBS.IsKeyDown(Keys.Right))
            {

            }
            else
            {

            }

            base.Update(gameTime);
        }
    }
}
