using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    class NumberGrid : PositionedObject
    {
        #region Fields
        ShapeGenerater _HTLine;
        ShapeGenerater _HBLine;
        ShapeGenerater _VTLLine;
        ShapeGenerater _VTRLine;
        ShapeGenerater _VBLLine;
        ShapeGenerater _VBRLine;
        ShapeGenerater _MLine;
        ShapeGenerater _backplate;
        Vector3 _color;
        byte _number = 11;
        bool _useBackplate = true;
        #endregion
        #region Properties
        public Vector3 Color { get => _color; set => _color = value; }
        public byte Number { set => ChangeNumber(value); get => _number; }

        #endregion
        #region Constructor
        public NumberGrid(Game game, Camera camera, float scale, Vector3 color) : base(game)
        {
            VertexPositionNormalTexture[] genHLine = GeneratedText.HorizontalLine();
            VertexPositionNormalTexture[] genVLine = GeneratedText.VerticalLine();
            VertexPositionNormalTexture[] genDLine = GeneratedText.MiddleLine(4.75f);

            _HTLine = new ShapeGenerater(game, camera, genHLine, 12);
            _HBLine = new ShapeGenerater(game, camera, genHLine, 12);

            _VTLLine = new ShapeGenerater(game, camera, genVLine, 12);
            _VTRLine = new ShapeGenerater(game, camera, genVLine, 12);
            _VBLLine = new ShapeGenerater(game, camera, genVLine, 12);
            _VBRLine = new ShapeGenerater(game, camera, genVLine, 12);

            _MLine = new ShapeGenerater(game, camera, genDLine, 20);

            _backplate = new ShapeGenerater(game, camera, GeneratedShapes.DigitBackplate(), 12);

            Scale = scale;
            _color = color;

            _HTLine.PO.AddAsChildOf(this);
            _HBLine.PO.AddAsChildOf(this);

            _VBLLine.PO.AddAsChildOf(this);
            _VBRLine.PO.AddAsChildOf(this);
            _VTLLine.PO.AddAsChildOf(this);
            _VTRLine.PO.AddAsChildOf(this);

            _MLine.PO.AddAsChildOf(this);

            _backplate.PO.AddAsChildOf(this);

            float vlinesX = 5.25f;
            float vlineY = 4.75f;
            float hlineY = 8.75f;

            _HTLine.Position = new Vector3(0, hlineY * Scale, 0);
            _HBLine.Position = new Vector3(0, -hlineY * Scale, 0);

            _HBLine.PO.Rotation.Z = MathHelper.Pi;

            _VTLLine.Position = new Vector3(-vlinesX * Scale, vlineY * Scale, 0);
            _VTLLine.PO.Rotation.Y = MathHelper.Pi;
            _VBLLine.Position = new Vector3(-vlinesX * Scale, -vlineY * Scale, 0);
            _VBLLine.PO.Rotation.Y = MathHelper.Pi;
            _VTRLine.Position = new Vector3(vlinesX * Scale, vlineY * Scale, 0);
            _VBRLine.Position = new Vector3(vlinesX * Scale, -vlineY * Scale, 0);

            _MLine.Position = Vector3.Zero;

            _backplate.PO.Position.Z = -0.5f * scale;

            ChangeColor(_color);
            ChangeBackplateColor(new Vector3(0.1f, 0.1f, 0.1f));
            ChangeScale(Scale);
            Clear();
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

        public void ChangeNumber(byte number)
        {
            if (number == _number)
                return;

            _number = number;
            Clear();

            switch (number)
            {
                case 0:
                    Zero();
                    break;
                case 1:
                    One();
                    break;
                case 2:
                    Two();
                    break;
                case 3:
                    Three();
                    break;
                case 4:
                    Four();
                    break;
                case 5:
                    Five();
                    break;
                case 6:
                    Six();
                    break;
                case 7:
                    Seven();
                    break;
                case 8:
                    Eight();
                    break;
                case 9:
                    Nine();
                    break;
            }
        }
        public void ChangeScale(float scale)
        {
            Scale = scale;
            _HBLine.Scale = Scale;
            _HTLine.Scale = Scale;
            _VBLLine.Scale = Scale;
            _VBRLine.Scale = Scale;
            _VTLLine.Scale = Scale;
            _VTRLine.Scale = Scale;
            _MLine.Scale = Scale;
            _backplate.Scale = Scale;
        }

        public void ChangeColor(Vector3 color)
        {
            _color = color;

            _HTLine.DiffuseColor = _color;
            _HBLine.DiffuseColor = _color;
            _VTLLine.DiffuseColor = _color;
            _VBLLine.DiffuseColor = _color;
            _VTRLine.DiffuseColor = _color;
            _VBRLine.DiffuseColor = _color;
            _MLine.DiffuseColor = _color;
        }

        public void ChangeBackplateColor(Vector3 color)
        {
            _backplate.DiffuseColor = color;
        }

        public void Clear()
        {
            _HBLine.Enabled = false;
            _HTLine.Enabled = false;
            _VBLLine.Enabled = false;
            _VBRLine.Enabled = false;
            _VTLLine.Enabled = false;
            _VTRLine.Enabled = false;
            _MLine.Enabled = false;
            _backplate.Enabled = _useBackplate;
        }

        void One()
        {
            _VTRLine.Enabled = true;
            _VBRLine.Enabled = true;
        }

        void Two()
        {
            _HTLine.Enabled = true;
            _VTRLine.Enabled = true;
            _MLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HBLine.Enabled = true;
        }

        void Three()
        {
            _HTLine.Enabled = true;
            _VTRLine.Enabled = true;
            _MLine.Enabled = true;
            _VBRLine.Enabled = true;
            _HBLine.Enabled = true;
        }

        void Four()
        {
            _VTLLine.Enabled = true;
            _VTRLine.Enabled = true;
            _MLine.Enabled = true;
            _VBRLine.Enabled = true;
        }

        void Five()
        {
            _HTLine.Enabled = true;
            _VTLLine.Enabled = true;
            _MLine.Enabled = true;
            _VBRLine.Enabled = true;
            _HBLine.Enabled = true;
        }

        void Six()
        {
            _HTLine.Enabled = true;
            _VTLLine.Enabled = true;
            _MLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HBLine.Enabled = true;
        }

        void Seven()
        {
            _HTLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VTRLine.Enabled = true;
        }

        void Eight()
        {
            _HBLine.Enabled = true;
            _HTLine.Enabled = true;
            _VBLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VTRLine.Enabled = true;
            _MLine.Enabled = true;
        }

        void Nine()
        {
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VTRLine.Enabled = true;
            _MLine.Enabled = true;
            _VBRLine.Enabled = true;
        }

        void Zero()
        {
            _HBLine.Enabled = true;
            _HTLine.Enabled = true;
            _VBLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VTRLine.Enabled = true;
        }
    }
}
