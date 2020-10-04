#region Using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
#endregion
namespace Panther
{
    public class ModelEntity : Entity
    {
        #region Fields
        string _modelFileName;
        List<ModelEntity> _children = new List<ModelEntity>();
        bool _modelLoaded;
        protected Model _model;
        protected Dictionary<string, Matrix> _baseTransforms = new Dictionary<string, Matrix>();
        protected Dictionary<string, Matrix> _currentTransforms = new Dictionary<string, Matrix>();
        protected Matrix[] _boneTransforms;
        protected Vector3 _minVector;
        protected Vector3 _maxVector;
        public Vector3 ModelScaleVelocity = Vector3.Zero;
        public Vector3 ModelScaleAcceleration = Vector3.Zero;
        public float Alpha = 1;
        #endregion
        #region Properties
        public Camera CameraRef { get => _camera; }

        public Vector3 WorldPosition
        {
            get => _PO.WorldPosition;
        }

        public Vector3 WorldVelocity
        {
            get => _PO.WorldVelocity;
        }


        public Vector3 WorldAcceleration
        {
            get => _PO.WorldAcceleration;
        }

        public Vector3 WorldRotation
        {
            get => _PO.WorldRotation;
        }

        public Vector3 WorldRotationVelocity
        {
            get => _PO.WorldRotationVelocity;
        }

        public Vector3 WorldRotationAcceleration
        {
            get => _PO.WorldRotationAcceleration;
        }

        public Model ModelRef { get => _model; }

        public BoundingBox Bounds
        {
            get
            {
                Matrix scaleMatrix = Matrix.CreateScale(Scale);
                Matrix rotate = RotateMatrix(Rotation);
                Matrix translate = Matrix.CreateTranslation(Position);

                Matrix transform = scaleMatrix * rotate * translate;

                Vector3 v1 = Vector3.Transform(_minVector, transform);
                Vector3 v2 = Vector3.Transform(_maxVector, transform);
                Vector3 boxMin = Vector3.Min(v1, v2);
                Vector3 boxMax = Vector3.Max(v1, v2);

                return new BoundingBox(boxMin, boxMax);
            }
        }

        public BoundingSphere Sphere
        {
            get
            {
                BoundingSphere sphere = _model.Meshes[0].BoundingSphere;
                sphere.Radius *= 0.75f;
                sphere = sphere.Transform(_world);
                return sphere;
            }
        }

        public new bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;

                foreach (ModelEntity child in _children)
                {
                    child.Visible = value;
                }
            }
        }
        #endregion
        #region Constructor
        public ModelEntity(Game game, Camera camera) : base(game, camera)
        {
        }

        public ModelEntity(Game game, Camera camera, Model model) : base(game, camera)
        {
            SetModel(model);
        }

        public ModelEntity(Game game, Camera camera, string modelFileName) : base(game, camera)
        {
            _modelFileName = modelFileName;
        }

        public ModelEntity(Game game, Camera camera, Model model, Vector3 position) : base(game, camera)
        {
            _PO.Position = position;
            SetModel(model);
        }

        public ModelEntity(Game game, Camera camera, string modelFileName, Vector3 position) : base(game, camera)
        {
            _PO.Position = position;
            _modelFileName = modelFileName;
        }

        #endregion
        #region Initialize-Load-BeginRun
        public override void Initialize()
        {
            base.Initialize();

            if (_model == null)
                LoadModel(_modelFileName);

            if (_model != null)
            {
                _boneTransforms = new Matrix[_model.Bones.Count];

                for (int i = 1; i < _model.Bones.Count; i++)
                {
                    _baseTransforms[_model.Bones[i].Name] = _model.Bones[i].Transform;
                    _currentTransforms[_model.Bones[i].Name] = _model.Bones[i].Transform;
                }

                _minVector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                _maxVector = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                foreach (ModelMesh mesh in _model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        VertexPositionNormalTexture[] vertexData = new
                        VertexPositionNormalTexture[part.VertexBuffer.VertexCount];
                        part.VertexBuffer.GetData<VertexPositionNormalTexture>(vertexData);

                        for (int i = 0; i < part.VertexBuffer.VertexCount; i++)
                        {
                            _minVector = Vector3.Min(_minVector, vertexData[i].Position);
                            _maxVector = Vector3.Max(_maxVector, vertexData[i].Position);
                        }
                    }
                }

                Game.SuppressDraw();
                //Enabled = true;
            }
            else
            {
                Core.DebugConsole("The Model was null for this Entity. " + this);
            }

        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Dispose(bool disposing)
        {
            _model = null;

            base.Dispose(disposing);
        }

        public void Remove()
        {
            Enabled = false;
            Game.Components.Remove(this);
            Dispose();
        }
        #endregion
        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (PO.Moveable || !_modelLoaded)
            {
                MatrixUpdate();
            }
        }

        public void MatrixUpdate()
        {
            if (_model != null && _boneTransforms != null)
            {
                _model.Root.Transform = Matrix.CreateScale(Scale) *
                    RotateMatrix(Rotation) * Matrix.CreateTranslation(Position);

                if (_PO.Child)
                {
                    foreach (PositionedObject po in _PO.ParentPOs)
                    {
                        _model.Root.Transform *=
                            RotateMatrix(po.Rotation) * Matrix.CreateTranslation(po.Position);
                    }
                }

                foreach (string keys in _currentTransforms.Keys)
                {
                    _model.Bones[keys].Transform = _currentTransforms[keys];
                }

                _model.CopyAbsoluteBoneTransformsTo(_boneTransforms);
                _world = _model.Root.Transform;

                _modelLoaded = true;
            }
        }
        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            if (_model == null)
                return;

            if (_camera == null)
            {
                Core.DebugConsole("The Camera is not setup (null) on the class. " + this);
                return;
            }

            if (_PO.Child)
            {
                if (!_PO.ParentPO.Enabled)
                    return;
            }

            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.World = _boneTransforms[mesh.ParentBone.Index];
                    basicEffect.View = _camera.View;
                    basicEffect.Projection = _camera.Projection;
                    basicEffect.DiffuseColor = _diffuseColor;
                    basicEffect.EmissiveColor = EmissiveColor;
                    basicEffect.Alpha = Alpha;
                    basicEffect.EnableDefaultLighting();
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Parents of any children must be added first or they will not be seen
        /// by the children added later. Active Dependent by default.
        /// </summary>
        /// <param name="parent">Parent Entity</param>
        public void AddAsChildOf(ModelEntity parent)
        {
            AddAsChildOf(parent, true);
        }
        /// <summary>
        /// Parents of any children must be added first or they will not be seen
        /// by the children added later.
        /// </summary>
        /// <param name="parent">Parent Entity</param>
        /// <param name="activeDepedent">True if child active state depends on parent.</param>
        public void AddAsChildOf(ModelEntity parent, bool activeDepedent)
        {
            parent._children.Add(this);
            _PO.AddAsChildOf(parent._PO, activeDepedent);
        }
        /// <summary>
        /// Sets the model from a loaded XNA Model.
        /// </summary>
        /// <param name="model">Loaded model.</param>
        public void SetModel(Model model)
        {
            if (model != null)
            {
                _model = model;
            }
        }
        /// <summary>
        /// Loads and sets the model from the file name.
        /// </summary>
        /// <param name="modelFileName">The file name located at content/Models</param>
        public void LoadModel(string modelFileName)
        {
            _model = Core.LoadModel(modelFileName);
            SetModel(_model);
        }
        /// <summary>
        /// Return a Model that is loaded from the filename.
        /// </summary>
        /// <param name="modelFileName">The file name located at content/Models</param>
        /// <returns></returns>
        public Model Load(string modelFileName)
        {
            return Core.LoadModel(modelFileName);
        }

        public bool IsCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }
        #endregion
    }
}
