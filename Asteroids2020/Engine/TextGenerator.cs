using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public class TextGenerator : PositionedObject
    {
        #region Fields
        List<TextGrid> _wordDisplay = new List<TextGrid>();
        Vector3 _color;
        Camera _camera;
        string _word;
        char _letter;
        #endregion
        #region Properties
        public Vector3 Color { get => _color; set => _color = value; }
        public string Words { get => _word; set => ChangeWord(value); }
        #endregion
        #region Constructor
        public TextGenerator(Game game, Camera camera) : base(game)
        {
            _camera = camera;
            Scale = 0.05f;
            _color = new Vector3(0.24f, 0.6f, 1.0f);
            _camera = new Camera(game, new Vector3(0, 0, 20), new Vector3(0, MathHelper.Pi, 0),
                game.GraphicsDevice.Viewport.AspectRatio, 1f, 100f);
        }
        #endregion
        #region Initialize
        public override void Initialize()
        {

            base.Initialize();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        #endregion
        #region Public Methods
        public void ChangeWord(string words)
        {
            if (words == _word)
                return;

            _word = words;

            if (_word.Length < _wordDisplay.Count)
            {
                for (int i = _word.Length; i < _wordDisplay.Count; i++)
                {
                    _wordDisplay.Add(new TextGrid(Game, _camera, Scale, _color));
                }
            }
            else if (_wordDisplay.Count > _word.Length)
            {
                foreach(TextGrid letter in _wordDisplay)
                {
                    letter.Clear();
                }
            }

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] == 32)
                    continue;

                _wordDisplay[i].ChangeLetter(_word[i]);
            }


        }

        void SetPosition()
        {
            Vector3 position = Position * Scale;

            float space = ((-Scale * 16) * (_word.Length - 1));

            for (int i = 0; i < _word.Length; i++)
            {
                _wordDisplay[i].Position.Y = position.Y;
                _wordDisplay[i].Position.X = space + position.X;
                space += (Scale * 16);
            }
        }

        #endregion
        #region Private/Protected Methods
        #endregion
    }
}
