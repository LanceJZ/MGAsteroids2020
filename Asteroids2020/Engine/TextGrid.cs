using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    class TextGrid : PositionedObject
    {
        #region Fields
        ShapeGenerater _HTLine;
        ShapeGenerater _HBLine;
        ShapeGenerater _VTLLine;
        ShapeGenerater _VTRLine;
        ShapeGenerater _VBLLine;
        ShapeGenerater _VBRLine;
        ShapeGenerater _MRLine;
        ShapeGenerater _MLLine;
        ShapeGenerater _CTLine;
        ShapeGenerater _CBLine;
        ShapeGenerater _DTRLine;
        ShapeGenerater _DTLLine;
        ShapeGenerater _DBRLine;
        ShapeGenerater _DBLLine;
        ShapeGenerater _backplate;
        Vector3 _color;
        ushort _charCode;
        bool _useBackplate = true;
        #endregion
        #region Properties
        public Vector3 Color { get => _color; set => _color = value; }
        public char Letter { set => ChangeLetter(value); get => (char)_charCode; }

        #endregion
        #region Constructor
        public TextGrid(Game game, Camera camera, float scale, Vector3 color) : base(game)
        {
            VertexPositionNormalTexture[] genHLine = GeneratedText.HorizontalLine();
            VertexPositionNormalTexture[] genVLine = GeneratedText.VerticalLine();
            VertexPositionNormalTexture[] genMLine = GeneratedText.MiddleLine(1.25f);
            VertexPositionNormalTexture[] genCLine = GeneratedText.MiddleLine(2.5f);
            VertexPositionNormalTexture[] genDLine = GeneratedText.DiagnialLine();

            _HTLine = new ShapeGenerater(game, camera, genHLine, 12);
            _HBLine = new ShapeGenerater(game, camera, genHLine, 12);

            _VTLLine = new ShapeGenerater(game, camera, genVLine, 12);
            _VTRLine = new ShapeGenerater(game, camera, genVLine, 12);
            _VBLLine = new ShapeGenerater(game, camera, genVLine, 12);
            _VBRLine = new ShapeGenerater(game, camera, genVLine, 12);

            _MRLine = new ShapeGenerater(game, camera, genMLine, 20);
            _MLLine = new ShapeGenerater(game, camera, genMLine, 20);

            _CTLine = new ShapeGenerater(game, camera, genCLine, 20);
            _CBLine = new ShapeGenerater(game, camera, genCLine, 20);

            _DTRLine = new ShapeGenerater(game, camera, genDLine, 12);
            _DTLLine = new ShapeGenerater(game, camera, genDLine, 12);
            _DBRLine = new ShapeGenerater(game, camera, genDLine, 12);
            _DBLLine = new ShapeGenerater(game, camera, genDLine, 12);


            _backplate = new ShapeGenerater(game, camera, GeneratedShapes.DigitBackplate(), 12);

            Scale = scale;
            _color = color;

            _HTLine.PO.AddAsChildOf(this);
            _HBLine.PO.AddAsChildOf(this);

            _VBLLine.PO.AddAsChildOf(this);
            _VBRLine.PO.AddAsChildOf(this);
            _VTLLine.PO.AddAsChildOf(this);
            _VTRLine.PO.AddAsChildOf(this);

            _MRLine.PO.AddAsChildOf(this);
            _MLLine.PO.AddAsChildOf(this);

            _CTLine.PO.AddAsChildOf(this);
            _CBLine.PO.AddAsChildOf(this);

            _DTRLine.PO.AddAsChildOf(this);
            _DTLLine.PO.AddAsChildOf(this);
            _DBRLine.PO.AddAsChildOf(this);
            _DBLLine.PO.AddAsChildOf(this);

            _backplate.PO.AddAsChildOf(this);

            float dlinesX = 2.75f;
            float dlinesY = 4.25f;
            float vlinesX = 5.25f;
            float vlineY = 4.5f;
            float mlinesX = 3.0f;
            float clineY = 3.75f;
            float hlineY = 8.5f;

            Vector3 dleft = new Vector3(0, MathHelper.Pi, MathHelper.PiOver4 + 0.15f);
            Vector3 dright = new Vector3(0, 0, MathHelper.PiOver4 + 0.15f);
            Vector3 center = new Vector3(0, 0, MathHelper.PiOver2);

            _HTLine.Position = new Vector3(0, hlineY * Scale, 0);
            _HBLine.Position = new Vector3(0, -hlineY * Scale, 0);

            _HBLine.PO.Rotation.Z = MathHelper.Pi;

            _VTLLine.Position = new Vector3(-vlinesX * Scale, vlineY * Scale, 0);
            _VTLLine.PO.Rotation.Y = MathHelper.Pi;
            _VBLLine.Position = new Vector3(-vlinesX * Scale, -vlineY * Scale, 0);
            _VBLLine.PO.Rotation.Y = MathHelper.Pi;
            _VTRLine.Position = new Vector3(vlinesX * Scale, vlineY * Scale, 0);
            _VBRLine.Position = new Vector3(vlinesX * Scale, -vlineY * Scale, 0);

            _MRLine.Position = new Vector3(mlinesX * Scale, 0, 0);
            _MLLine.Position = new Vector3(-mlinesX * Scale, 0, 0);

            _CTLine.Position = new Vector3(0, clineY * Scale, 0);
            _CBLine.Position = new Vector3(0, -clineY * Scale, 0);
            _CTLine.Rotation = center;
            _CBLine.Rotation = center;

            _DTRLine.Position = new Vector3(dlinesX * Scale, dlinesY * Scale, 0);
            _DTLLine.Position = new Vector3(-dlinesX * Scale, dlinesY * Scale, 0);
            _DBRLine.Position = new Vector3(dlinesX * Scale, -dlinesY * Scale, 0);
            _DBLLine.Position = new Vector3(-dlinesX * Scale, -dlinesY * Scale, 0);

            _DTRLine.Rotation = dright;
            _DTLLine.Rotation = dleft;
            _DBRLine.Rotation = dleft;
            _DBLLine.Rotation = dright;

            _backplate.PO.Position.Z = -0.5f * scale;

            ChangeColor(_color);
            ChangeBackplateColor(new Vector3(0.1f, 0.1f, 0.1f));
            ChangeScale(Scale);
            //Clear();
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
        #region Public methods
        public void ChangeScale(float scale)
        {
            Scale = scale;
            _HBLine.Scale = Scale;
            _HTLine.Scale = Scale;
            _VBLLine.Scale = Scale;
            _VBRLine.Scale = Scale;
            _VTLLine.Scale = Scale;
            _VTRLine.Scale = Scale;
            _MRLine.Scale = Scale;
            _MLLine.Scale = Scale;
            _CTLine.Scale = Scale;
            _CBLine.Scale = Scale;
            _DTRLine.Scale = Scale;
            _DTLLine.Scale = Scale;
            _DBRLine.Scale = Scale;
            _DBLLine.Scale = Scale;
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
            _MRLine.DiffuseColor = _color;
            _MLLine.DiffuseColor = _color;
            _CTLine.DiffuseColor = _color;
            _CBLine.DiffuseColor = _color;
            _DTRLine.DiffuseColor = _color;
            _DTLLine.DiffuseColor = _color;
            _DBRLine.DiffuseColor = _color;
            _DBLLine.DiffuseColor = _color;
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
            _MRLine.Enabled = false;
            _MLLine.Enabled = false;
            _CTLine.Enabled = false;
            _CBLine.Enabled = false;
            _DTRLine.Enabled = false;
            _DTLLine.Enabled = false;
            _DBRLine.Enabled = false;
            _DBLLine.Enabled = false;
            _backplate.Enabled = _useBackplate;
        }

        public void ChangeLetter(ushort charCode)
        {
            _charCode = charCode;

            switch (charCode)
            {
                case 65:
                    A();
                    break;
                case 66:
                    B();
                    break;
                case 67:
                    C();
                    break;
                case 68:
                    D();
                    break;
                case 69:
                    E();
                    break;
                case 70:
                    F();
                    break;
                case 71:
                    G();
                    break;
                case 72:
                    H();
                    break;
                case 73:
                    I();
                    break;
                case 74:
                    J();
                    break;
                case 75:
                    K();
                    break;
                case 76:
                    L();
                    break;
                case 77:
                    M();
                    break;
                case 78:
                    N();
                    break;
                case 79:
                    O();
                    break;
                case 80:
                    P();
                    break;
                case 81:
                    Q();
                    break;
                case 82:
                    R();
                    break;
                case 83:
                    S();
                    break;
                case 84:
                    T();
                    break;
                case 85:
                    U();
                    break;
                case 86:
                    V();
                    break;
                case 87:
                    W();
                    break;
                case 88:
                    X();
                    break;
                case 89:
                    Y();
                    break;
                case 90:
                    Z();
                    break;
            }

        }
        #endregion
        #region Private/Protected methods
        void A()
        {
            _VTRLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _MLLine.Enabled = true;
            _MRLine.Enabled = true;
        }

        void B()
        {
            _VTRLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
            _MRLine.Enabled = true;
        }

        void C()
        {
            _VTLLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
        }

        void D()
        {
            _VTRLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
        }

        void E()
        {
            _VTLLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
            _MLLine.Enabled = true;
        }

        void F()
        {
            _VTLLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _MLLine.Enabled = true;
        }

        void G()
        {
            _VTLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VBLLine.Enabled = true;
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
            _MLLine.Enabled = true;
        }

        void H()
        {
            _VTRLine.Enabled = true;
            _VTLLine.Enabled = true;
            _VBRLine.Enabled = true;
            _VBLLine.Enabled = true;
            _MRLine.Enabled = true;
            _MLLine.Enabled = true;
        }

        void I()
        {
            _HTLine.Enabled = true;
            _HBLine.Enabled = true;
            _CBLine.Enabled = true;
            _CTLine.Enabled = true;
        }

        void J()
        {

        }

        void K()
        {

        }

        void L()
        {

        }

        void M()
        {

        }

        void N()
        {

        }

        void O()
        {

        }

        void P()
        {

        }

        void Q()
        {

        }

        void R()
        {

        }

        void S()
        {

        }

        void T()
        {

        }

        void U()
        {

        }

        void V()
        {

        }

        void W()
        {

        }

        void X()
        {

        }

        void Y()
        {

        }

        void Z()
        {

        }
        #endregion
    }
}
