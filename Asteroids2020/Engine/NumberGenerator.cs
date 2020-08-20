using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    class NumberGenerator : PositionedObject
    {
        #region Fields
        List<NumberGrid> _numberDisplay = new List<NumberGrid>();
        Vector3 _color;
        Camera _camera;
        ulong _number;
        byte _digits;
        #endregion
        #region Properties
        public Vector3 Color { get => _color; set => _color = value; }
        public ulong Number { set => ChangeNumber(value); get => _number; }

        #endregion
        #region Constructor
        public NumberGenerator(Game game, Camera camera, float scale, Vector3 color) : base(game)
        {
            Scale = scale;
            _color = color;
            _camera = camera;
        }
        public NumberGenerator(Game game) : base(game)
        {
            Scale = 0.025f;
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

        public void ChangeNumber(ulong number)
        {
            if (_number == number)
                return;

            _number = number;

            if (_digits < NumberOfDigits())
            {
                for (uint i = _digits; i < NumberOfDigits(); i++)
                {
                    _numberDisplay.Add(new NumberGrid(Game, _camera, Scale, _color));
                }

                _digits = NumberOfDigits();
            }
            else if (_digits > NumberOfDigits())
            {
                foreach(NumberGrid num in _numberDisplay)
                {
                    num.Clear();
                }
            }

            byte[] numbers = NumbersIn(number);


            for (int i = 0; i < numbers.Length; i++)
            {
                _numberDisplay[i].ChangeNumber(numbers[i]);
            }

            SetPosition();
        }

        public void SetRotationVolicity()
        {
            foreach (NumberGrid num in _numberDisplay)
            {
                num.RotationVelocity = RotationVelocity;
            }
        }

        void SetPosition()
        {
            Vector3 position = Position * Scale;            

            float space = ((-Scale * 16) * (_digits - 1));

            for (int i = 0; i < _digits; i++)
            {
                _numberDisplay[i].Position.Y = position.Y;
                _numberDisplay[i].Position.X = space + position.X;
                space += (Scale * 16);
            }
        }

        byte[] NumbersIn(ulong num)
        {
            List<byte> listOfInts = new List<byte>();

            while (num > 0)
            {
                uint digit = (uint)num % 10;
                listOfInts.Add((byte) digit);
                num = num / 10;
            }

            listOfInts.Reverse();
            return listOfInts.ToArray();
        }

        byte NumberOfDigits()
        {
            if (_number < 10) return 1;
            if (_number < 100) return 2;
            if (_number < 1000) return 3;
            if (_number < 10000) return 4;
            if (_number < 100000) return 5;
            if (_number < 1000000) return 6;
            if (_number < 10000000) return 7;
            if (_number < 100000000) return 8;
            if (_number < 1000000000) return 9;
            if (_number < 10000000000) return 10;
            if (_number < 100000000000) return 11;
            if (_number < 1000000000000) return 12;
            if (_number < 10000000000000) return 13;
            if (_number < 100000000000000) return 14;
            if (_number < 1000000000000000) return 15;
            if (_number < 10000000000000000) return 16;
            return 17;
        }
    }
}
