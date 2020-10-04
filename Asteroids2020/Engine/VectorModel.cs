#region Using
using System;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
#endregion

namespace Panther
{
    public class VectorModel : Entity
    {
        Camera theCamera;
        FileIO fileIO;
        List<VectorModel> _children = new List<VectorModel>();
        Matrix localMatrix;
        VertexPositionColor[] pointList;
        Vector3[] vertexArray;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        RasterizerState rasterizerState;
        string name;
        short[] lineIndices;
        Color color = Color.White;
        float modelScale = 1;
        float Alpha = 1;

        public Vector3[] VertexArray { get => vertexArray; }
        public Color Color { get => color; set => color = value; }
        public float ModelScale { get => modelScale; set => modelScale = value; }
        public string Name { get => name; set => name = value; }

        public VectorModel (Game game, Camera camera): base(game, camera)
        {
            theCamera = camera;
            fileIO = new FileIO();
        }

        public override void Initialize()
        {
            base.Initialize();
            rasterizerState = new RasterizerState();
            rasterizerState.FillMode = FillMode.WireFrame;
            rasterizerState.CullMode = CullMode.None;
            Enabled = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                base.Update(gameTime);

                if (Moveable)
                {
                    UpdateMatrix();
                }
            }
        }

        public void UpdateMatrix()
        {
            if (_effect == null)
            {
                Core.DebugConsole("Effect is null in VectorModel " + name);
                return;
            }

            _effect.Projection = _camera.Projection;
            _effect.View = _camera.View;
            _effect.DiffuseColor = _diffuseColor;
            _effect.EmissiveColor = _emissiveColor;
            _effect.Alpha = Alpha;
            _world = Matrix.CreateScale(Scale) * RotateMatrix(Rotation) * Matrix.CreateTranslation(Position);

            if (_PO.Child)
            {
                foreach (PositionedObject po in _PO.ParentPOs)
                {
                    if (!_PO.DirectConnection)
                    {
                        _world *= RotateMatrix(po.Rotation) * Matrix.CreateTranslation(po.Position);
                    }
                }
            }

            _effect.World = _world;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Enabled && Visible)
            {
                base.Draw(gameTime);

                if (Effect == null)
                {
                    Core.DebugConsole("Effect is null in " + name);
                    return;
                }

                Core.GraphicsDM.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                Core.GraphicsDM.GraphicsDevice.Indices = indexBuffer;

                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    Core.GraphicsDM.GraphicsDevice.DrawIndexedPrimitives
                        (
                        PrimitiveType.LineList,
                        0,
                        0,
                        pointList.Length
                        );
                }
            }
        }

        public override void Spawn(Vector3 position)
        {
            base.Spawn(position);
            UpdateMatrix();
        }

        public override void Spawn(Vector3 position, Vector3 velocity)
        {
            base.Spawn(position, velocity);
            UpdateMatrix();
        }

        public override void Spawn(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            base.Spawn(position, rotation, velocity);
            UpdateMatrix();
        }

        public override void Spawn(Vector3 position, Vector3 rotation, Vector3 rotationVelocity, Vector3 velocity)
        {
            base.Spawn(position, rotation, rotationVelocity, velocity);
            UpdateMatrix();
        }

        public void AddAsChildOf(VectorModel parent)
        {
            _PO.AddAsChildOf(parent.PO, true, false, true, false);
        }

        public void AddAsChildOf(VectorModel parent, bool directConnection)
        {
            PO.AddAsChildOf(parent.PO, true, directConnection, true, false);
        }

        public void AddAsChildOf(VectorModel parent, bool directConnection,
            bool rotationDependent)
        {
            parent._children.Add(this);
            PO.AddAsChildOf(parent.PO, true, directConnection, rotationDependent, false);
        }

        public float LoadVectorModel(string fileName)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, modelScale, fileName);
        }
        public float LoadVectorModel(string fileName, string name)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, modelScale, name);
        }

        public float LoadVectorModel(string fileName, float scale)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, scale, fileName);
        }

        public float LoadVectorModel(string fileName, string name, float scale)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, scale, name);
        }

        public float LoadVectorModel(string fileName, Color color)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, modelScale, fileName);
        }

        public float LoadVectorModel(string fileName, string name, Color color)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, modelScale, fileName);
        }

        public float LoadVectorModel(string fileName, Color color, float scale)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, scale, fileName);
        }

        public float LoadVectorModel(string fileName, string name, Color color, float scale)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(fileName), color, scale, name);
        }

        public float InitializePoints(Vector3[] pointPositions, string name)
        {
            return InitializePoints(pointPositions, color, modelScale, name);
        }

        public float InitializePoints(Vector3[] pointPositions, Color color, string name)
        {
            return InitializePoints(pointPositions, color, modelScale, name);
        }

        public float InitializePoints(Vector3[] pointPositions, Color color, float scale, string name)
        {
            this.name = name;
            vertexArray = pointPositions;
            this.color = color;

            if (scale != 1)
            {
                Vector3[] oldScale = pointPositions;

                for (int i = 0; i < pointPositions.Count(); i++)
                {
                    pointPositions[i] = oldScale[i] * scale;
                }
            }

            if (pointPositions != null)
            {
                VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
                );

                pointList = new VertexPositionColor[pointPositions.Length];

                for (int x = 0; x < pointPositions.Length; x++)
                {
                    pointList[x] = new VertexPositionColor(pointPositions[x], color);
                }

                // Initialize the vertex buffer, allocating memory for each vertex.
                vertexBuffer = new VertexBuffer(Core.GraphicsDM.GraphicsDevice, vertexDeclaration,
                    pointPositions.Length, BufferUsage.None);

                // Set the vertex buffer data to the array of vertices.
                vertexBuffer.SetData(pointList);

                InitializeLineList();
                InitializeEffect();
                UpdateMatrix();
            }

            for (int i = 0; i < pointPositions.Length; i++)
            {
                if (Math.Abs(pointPositions[i].X) > PO.Radius)
                    PO.Radius = Math.Abs(pointPositions[i].X);

                if (Math.Abs(pointPositions[i].Y) > PO.Radius)
                    PO.Radius = Math.Abs(pointPositions[i].Y);
            }

            return PO.Radius * scale;
        }
        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game. Moved to Services.
        /// </summary>
        public void InitializeEffect()
        {
            _effect = new BasicEffect(Core.Graphics);
            _effect.VertexColorEnabled = true;
            _effect.TextureEnabled = false;
            _effect.View = theCamera.View;
            _effect.Projection = theCamera.Projection;
            _effect.World = localMatrix;
        }
        public void Destroy()
        {
            vertexBuffer.Dispose();
            rasterizerState.Dispose();
            Dispose();
        }
        #region Private Methods
        void InitializeLineList()
        {
            // Initialize an array of indices of type short.
            lineIndices = new short[(pointList.Length * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < pointList.Length - 1; i++)
            {
                lineIndices[i * 2] = (short)(i);
                lineIndices[(i * 2) + 1] = (short)(i + 1);
            }

            indexBuffer = new IndexBuffer(Core.GraphicsDM.GraphicsDevice, typeof(short),
                lineIndices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(lineIndices);
        }
        #endregion
    }
}
