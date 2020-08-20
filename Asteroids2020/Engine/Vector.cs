#region Using
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Linq;
#endregion

namespace Panther
{
    public class Vector : Entity
    {
        Camera theCamera;
        Matrix localMatrix;
        VertexPositionColor[] pointList;
        VertexBuffer vertexBuffer;
        RasterizerState rasterizerState;
        short[] lineListIndices;
        FileStream fileStream;
        public float Alpha = 1;
        bool fileLoaded;

        public Vector (Game game, Camera camera): base(game, camera)
        {
            theCamera = camera;
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
            if (fileLoaded)
            {
                base.Update(gameTime);
                Transform();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Enabled && fileLoaded)
            {
                foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                }

                //Effect.World = localMatrix;
                Effect.View = _camera.View;
                Effect.Projection = _camera.Projection;
                Effect.EmissiveColor = EmissiveColor;
                Effect.Alpha = Alpha;

                Core.GraphicsDM.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(                    PrimitiveType.LineList, pointList,
                    0,  // vertex buffer offset to add to each element of the index buffer
                    pointList.Length,  // number of vertices in pointList
                    lineListIndices,  // the index buffer
                    0,  // first index element to read
                    pointList.Length - 1   // number of primitives to draw
                );
            }
        }

        public void LoadVectorModel(string name)
        {

        }

        public float InitializePoints(Vector3[] pointPosition)
        {
            float radius = 0;

            if (fileLoaded)
            {
                VertexDeclaration vertexDeclaration = new VertexDeclaration(new VertexElement[]
                    {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                    }
                );

                pointList = new VertexPositionColor[pointPosition.Length];

                for (int x = 0; x < pointPosition.Length; x++)
                {
                    pointList[x] = new VertexPositionColor(pointPosition[x], new Color(190, 170, 255));
                }

                // Initialize the vertex buffer, allocating memory for each vertex.
                vertexBuffer = new VertexBuffer(Core.GraphicsDM.GraphicsDevice, vertexDeclaration,
                    pointPosition.Length, BufferUsage.None);

                // Set the vertex buffer data to the array of vertices.
                vertexBuffer.SetData<VertexPositionColor>(pointList);
                InitializeLineList();
                InitializeEffect();
                Transform();
            }

            for (int i = 0; i < pointPosition.Length; i++)
            {
                if (Math.Abs(pointPosition[i].X) > radius)
                    radius = Math.Abs(pointPosition[i].X);

                if (Math.Abs(pointPosition[i].Y) > radius)
                    radius = Math.Abs(pointPosition[i].Y);
            }

            return radius;
        }

        /// <summary>
        /// Initializes the effect (loading, parameter setting, and technique selection)
        /// used by the game. Moved to Services.
        /// </summary>
        public void InitializeEffect()
        {
            Effect = new BasicEffect(Core.Graphics);
            Effect.VertexColorEnabled = true;
            Effect.View = theCamera.View;
            Effect.Projection = theCamera.Projection;
            Effect.World = localMatrix;
        }

        public void WriteFile(Vector3[] vertices, string fileName)
        {
            fileName = "Content/Models/" + fileName + ".vec";
            fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            foreach (Vector3 vert in vertices)
            {
                byte[] verticy = new UTF8Encoding(true).GetBytes(vert.ToString());
                fileStream.Write(verticy, 0, verticy.Length);
            }

            Close();
        }
        /// <summary>
        /// Read model file and convert/decode to Vector3 Array.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Vector3[] ReadFile(string fileName)
        {
            string dataRead = "";
            List<Vector3> vertRead = new List<Vector3>();
            fileName = "Content/Models/" + fileName + ".vec";

            if (File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] dataByte = new byte[1024];
                UTF8Encoding bufferUTF8 = new UTF8Encoding(true);

                while (fileStream.Read(dataByte, 0, dataByte.Length) > 0)
                {
                    dataRead += bufferUTF8.GetString(dataByte, 0, dataByte.Length);
                }

                Close();

                fileLoaded = true;
                string onAxis = "";
                string number = "";
                float X = 0;
                float Y = 0;
                float Z;

                foreach(char dataChar in dataRead)
                {
                    if (dataChar.ToString() == "'\0'")
                    {
                        break;
                    }

                    if (dataChar.ToString() == "{" || dataChar.ToString() == ":")
                    {
                        continue;
                    }

                    switch (dataChar.ToString())
                    {
                        case "X":
                            onAxis = dataChar.ToString();
                            continue;
                        case "Y":
                            onAxis = dataChar.ToString();
                            continue;
                        case "Z":
                            onAxis = dataChar.ToString();
                            continue;
                    }

                    switch (onAxis)
                    {
                        case "X":
                            if (dataChar.ToString() == " ")
                            {
                                X = float.Parse(number);
                                number = "";
                                onAxis = "";
                            }
                            else
                            {
                                if (dataChar.ToString() == "}")
                                {
                                    continue;
                                }

                                number += dataChar.ToString();
                            }
                            break;
                        case "Y":
                            if (dataChar.ToString() == " ")
                            {
                                Y = float.Parse(number);
                                number = "";
                                onAxis = "";
                            }
                            else
                            {
                                if (dataChar.ToString() == "}")
                                {
                                    continue;
                                }

                                number += dataChar.ToString();
                            }
                            break;
                        case "Z":
                            if (dataChar.ToString() == "}")
                            {
                                Z = float.Parse(number);
                                vertRead.Add(new Vector3(X, Y, Z));
                                onAxis = "";
                                number = "";
                            }
                            else
                            {
                                number += dataChar.ToString();
                            }
                            break;
                    }
                }
            }
            else
            {
                //Debug file not found.
                System.Diagnostics.Debug.WriteLine("File " + fileName + " not found.");
                vertRead.Add(Vector3.Zero);
            }

            Vector3[] verts = new Vector3[vertRead.Count];

            for (int i = 0; i < vertRead.Count; i++)
            {
                verts[i] = vertRead[i];
            }

            return verts;
        }

        public void Destroy()
        {
            vertexBuffer.Dispose();
            rasterizerState.Dispose();
            Dispose();
        }

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

        void Transform()
        {
            // Calculate the mesh transformation by combining translation, rotation, and scaling
            localMatrix = Matrix.CreateScale(Scale) * 
                Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)
                * Matrix.CreateTranslation(Position);
            // Apply to Effect
            Effect.World = localMatrix;
        }

        void Close()
        {
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }
    }
}
