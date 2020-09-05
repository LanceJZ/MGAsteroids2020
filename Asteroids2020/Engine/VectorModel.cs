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
        Matrix localMatrix;
        VertexPositionColor[] pointList;
        Vector3[] vertexArray;
        VertexBuffer vertexBuffer;
        RasterizerState rasterizerState;
        short[] lineListIndices;
        public float Alpha = 1;

        public Vector3[] VertexArray { get => vertexArray; }

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
                Transform();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Enabled)
            {
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                }

                Core.GraphicsDM.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, pointList, //Type, Vertex array for vertices.
                    0,  // Vertex buffer offset to add to each element of the index buffer.
                    pointList.Length,  // Number of vertices in pointList.
                    lineListIndices,  // The index buffer.
                    0,  // First index element to read.
                    pointList.Length - 1   // Number of primitives to draw.
                );
            }
        }

        public void Transform()
        {
            // Calculate the mesh transformation by combining translation, rotation, and scaling
            localMatrix = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)
                * Matrix.CreateTranslation(Position);
            // Apply to Effect
            Effect.World = localMatrix;
            Effect.View = _camera.View;
            Effect.Projection = _camera.Projection;
            Effect.EmissiveColor = EmissiveColor;
            Effect.DiffuseColor = DiffuseColor;
            Effect.Alpha = Alpha;
        }

        public override void Spawn(Vector3 position)
        {
            base.Spawn(position);
            Transform();
        }

        public override void Spawn(Vector3 position, Vector3 velocity)
        {
            base.Spawn(position, velocity);
            Transform();
        }

        public override void Spawn(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            base.Spawn(position, rotation, velocity);
            Transform();
        }

        public override void Spawn(Vector3 position, Vector3 rotation, Vector3 rotationVelocity, Vector3 velocity)
        {
            base.Spawn(position, rotation, rotationVelocity, velocity);
            Transform();
        }

        public float LoadVectorModel(string name, Color color)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(name), color);
        }

        public float LoadVectorModel(string name)
        {
            return InitializePoints(fileIO.ReadVectorModelFile(name), Color.White);
        }

        public float InitializePoints(Vector3[] pointPositions, Color color)
        {
            vertexArray = pointPositions;

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
                vertexBuffer.SetData<VertexPositionColor>(pointList);
                InitializeLineList();
                InitializeEffect();
                Transform();
            }

            for (int i = 0; i < pointPositions.Length; i++)
            {
                if (Math.Abs(pointPositions[i].X) > PO.Radius)
                    PO.Radius = Math.Abs(pointPositions[i].X);

                if (Math.Abs(pointPositions[i].Y) > PO.Radius)
                    PO.Radius = Math.Abs(pointPositions[i].Y);
            }

            return PO.Radius;
        }
        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game. Moved to Services.
        /// </summary>
        public void InitializeEffect()
        {
            Effect = new BasicEffect(Core.Graphics);
            Effect.VertexColorEnabled = true;
            Effect.TextureEnabled = false;
            Effect.View = theCamera.View;
            Effect.Projection = theCamera.Projection;
            Effect.World = localMatrix;
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
            lineListIndices = new short[(pointList.Length * 2) - 2];

            // Populate the array with references to indices in the vertex buffer
            for (int i = 0; i < pointList.Length - 1; i++)
            {
                lineListIndices[i * 2] = (short)(i);
                lineListIndices[(i * 2) + 1] = (short)(i + 1);
            }
        }
        #endregion
    }
}
