using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Panther
{
    class ShapeGenerater : Entity
    {
        VertexPositionNormalTexture[] _shape;
        List<ShapeGenerater> _children = new List<ShapeGenerater>();
        int _primitiveCount;
        public new bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;

                foreach (ShapeGenerater child in _children)
                {
                    child.Visible = value;
                }
            }
        }


        public ShapeGenerater(Game game, Camera camera, VertexPositionNormalTexture[] shape,
            int primitiveCount) : base(game, camera)
        {
            _shape = shape;
            _primitiveCount = primitiveCount;
            Defaults();
        }

        public ShapeGenerater(Game game, Camera camera, VertexPositionNormalTexture[] shape,
            int primitiveCount, Vector3 position) : base(game, camera)
        {
            _shape = shape;
            _primitiveCount = primitiveCount;
            Position = position;
            Defaults();
        }

        void Defaults()
        {
            _diffuseColor = Vector3.One;
            _ambiantColor = new Vector3(0.75f, 0.75f, 0.75f);
            _lightDirection = new Vector3(-0.5f, 0.5f, -0.5f);
            _directionalLightColor = new Vector3(0.15f, 0.15f, 0.15f);
            _emissiveColor = Vector3.Zero;
        }
        public override void Initialize()
        {
            _effect = new BasicEffect(Core.Graphics);
            _effect.AmbientLightColor.Round();
            _effect.LightingEnabled = true;
            _effect.EnableDefaultLighting();
        }

        public override void Update(GameTime gameTime)
        {
            if (Moveable)
            {
                UpdateMatrix();
            }
        }

        public void UpdateMatrix()
        {
            _effect.Projection = _camera.Projection;
            _effect.View = _camera.View;
            _effect.AmbientLightColor = _ambiantColor;
            _effect.DirectionalLight0.DiffuseColor = _directionalLightColor;
            _effect.DirectionalLight0.Direction = _lightDirection;
            _effect.DiffuseColor = _diffuseColor;
            _effect.EmissiveColor = _emissiveColor;
            _world = Matrix.CreateScale(Scale) * RotateMatrix(Rotation) * Matrix.CreateTranslation(Position);

            if (_PO.Child)
            {
                foreach (PositionedObject po in _PO.ParentPOs)
                {
                    _world *= RotateMatrix(po.Rotation) * Matrix.CreateTranslation(po.Position);
                }
            }

            _effect.World = _world;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Core.Graphics.DrawUserPrimitives(PrimitiveType.TriangleList, _shape, 0, _primitiveCount);
                }
            }
        }
        /// <summary>
        /// Parents of any children must be added first or they will not be seen
        /// by the children added later. Active Dependent by default.
        /// </summary>
        /// <param name="parent">Parent Entity</param>
        public void AddAsChildOf(ShapeGenerater parent)
        {
            AddAsChildOf(parent, true);
        }
        /// <summary>
        /// Parents of any children must be added first or they will not be seen
        /// by the children added later.
        /// </summary>
        /// <param name="parent">Parent Entity</param>
        /// <param name="activeDepedent">True if child active state depends on parent.</param>
        public void AddAsChildOf(ShapeGenerater parent, bool activeDepedent)
        {
            parent._children.Add(this);
            _PO.AddAsChildOf(parent._PO, activeDepedent);
        }
    }
}
