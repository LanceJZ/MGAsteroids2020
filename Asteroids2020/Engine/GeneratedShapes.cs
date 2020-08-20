using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    static public class GeneratedShapes
    {
        #region Fields

        #endregion
        #region Properties

        #endregion
        #region Public Methods
        static public VertexPositionNormalTexture[] Cube()
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[36];

            Vector2 Texcoords = Vector2.Zero;

            Vector3[] face = new Vector3[6];

            //TopLeft
            face[0] = new Vector3(-1f, 1f, 0.0f);
            //BottomLeft
            face[1] = new Vector3(-1f, -1f, 0.0f);
            //TopRight
            face[2] = new Vector3(1f, 1f, 0.0f);
            //BottomLeft
            face[3] = new Vector3(-1f, -1f, 0.0f);
            //BottomRight
            face[4] = new Vector3(1f, -1f, 0.0f);
            //TopRight
            face[5] = new Vector3(1f, 1f, 0.0f);

            //front face
            Matrix RotY180 = Matrix.CreateRotationY(-(float)Math.PI);
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i] = new VertexPositionNormalTexture(Vector3.Transform(face[i], RotY180) + Vector3.UnitZ, Vector3.UnitZ, Texcoords);
                vertexes[i + 3] = new VertexPositionNormalTexture(Vector3.Transform(face[i + 3], RotY180) + Vector3.UnitZ, Vector3.UnitZ, Texcoords);
            }

            //Back face
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i + 6] = new VertexPositionNormalTexture(Vector3.Transform(face[2 - i], RotY180) - Vector3.UnitZ, -Vector3.UnitZ, Texcoords);
                vertexes[i + 6 + 3] = new VertexPositionNormalTexture(Vector3.Transform(face[5 - i], RotY180) - Vector3.UnitZ, -Vector3.UnitZ, Texcoords);
            }

            //left face
            Matrix RotY90 = Matrix.CreateRotationY((float)Math.PI / 2f);
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i + 12] = new VertexPositionNormalTexture(Vector3.Transform(face[i], RotY90) - Vector3.UnitX,
                           -Vector3.UnitX, Texcoords);
                vertexes[i + 12 + 3] = new VertexPositionNormalTexture(Vector3.Transform(face[i + 3], RotY90) - Vector3.UnitX,
                           -Vector3.UnitX, Texcoords);
            }

            //Right face
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i + 18] = new VertexPositionNormalTexture(Vector3.Transform(face[2 - i], RotY90) + Vector3.UnitX,
                     Vector3.UnitX, Texcoords);
                vertexes[i + 18 + 3] = new VertexPositionNormalTexture(Vector3.Transform(face[5 - i], RotY90) + Vector3.UnitX,
                     Vector3.UnitX, Texcoords);
            }

            //Top face
            Matrix RotX90 = Matrix.CreateRotationX((float)Math.PI / 2f);
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i + 24] = new VertexPositionNormalTexture(Vector3.Transform(face[i], RotX90) + Vector3.UnitY,
                    Vector3.UnitY, Texcoords);
                vertexes[i + 24 + 3] = new VertexPositionNormalTexture(Vector3.Transform(face[i + 3], RotX90) + Vector3.UnitY,
                    Vector3.UnitY, Texcoords);
            }

            //Bottom face
            for (int i = 0; i <= 2; i++)
            {
                vertexes[i + 30] = new VertexPositionNormalTexture(Vector3.Transform(face[2 - i], RotX90) - Vector3.UnitY,
                    -Vector3.UnitY, Texcoords);
                vertexes[i + 30 + 3] = new VertexPositionNormalTexture(Vector3.Transform(face[5 - i], RotX90) - Vector3.UnitY,
                    -Vector3.UnitY, Texcoords);
            }

            return vertexes;
        }

        static public VertexPositionNormalTexture[] DigitBackplate()
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[36];

            Vector2 texcoords = Vector2.Zero;

            float width = 8.0f;
            float height = 10.75f;
            float depth = 0.1f;

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = new Vector3(-width, height, -depth);
            Vector3 topLeftBack = new Vector3(-width, height, depth);
            Vector3 topRightFront = new Vector3(width, height, -depth);
            Vector3 topRightBack = new Vector3(width, height, depth);

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = new Vector3(-width, -height, -depth);
            Vector3 btmLeftBack = new Vector3(-width, -height, depth);
            Vector3 btmRightFront = new Vector3(width, -height, -depth);
            Vector3 btmRightBack = new Vector3(width, -height, depth);

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 normalTop = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 normalBottom = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 normalLeft = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 normalRight = new Vector3(1.0f, 0.0f, 0.0f);

            // Add the vertices for the FRONT face.
            vertexes[0] = new VertexPositionNormalTexture(topLeftFront, normalBack, texcoords);
            vertexes[1] = new VertexPositionNormalTexture(btmLeftFront, normalBack, texcoords);
            vertexes[2] = new VertexPositionNormalTexture(topRightFront, normalBack, texcoords);
            vertexes[3] = new VertexPositionNormalTexture(btmLeftFront, normalBack, texcoords);
            vertexes[4] = new VertexPositionNormalTexture(btmRightFront, normalBack, texcoords);
            vertexes[5] = new VertexPositionNormalTexture(topRightFront, normalBack, texcoords);

            // Add the vertices for the BACK face.
            vertexes[6] = new VertexPositionNormalTexture(topLeftBack, normalFront, texcoords);
            vertexes[7] = new VertexPositionNormalTexture(topRightBack, normalFront, texcoords);
            vertexes[8] = new VertexPositionNormalTexture(btmLeftBack, normalFront, texcoords);
            vertexes[9] = new VertexPositionNormalTexture(btmLeftBack, normalFront, texcoords);
            vertexes[10] = new VertexPositionNormalTexture(topRightBack, normalFront, texcoords);
            vertexes[11] = new VertexPositionNormalTexture(btmRightBack, normalFront, texcoords);

            // Add the vertices for the TOP face.
            vertexes[12] = new VertexPositionNormalTexture(topLeftFront, normalBottom, texcoords);
            vertexes[13] = new VertexPositionNormalTexture(topRightBack, normalBottom, texcoords);
            vertexes[14] = new VertexPositionNormalTexture(topLeftBack, normalBottom, texcoords);
            vertexes[15] = new VertexPositionNormalTexture(topLeftFront, normalBottom, texcoords);
            vertexes[16] = new VertexPositionNormalTexture(topRightFront, normalBottom, texcoords);
            vertexes[17] = new VertexPositionNormalTexture(topRightBack, normalBottom, texcoords);

            // Add the vertices for the BOTTOM face. 
            vertexes[18] = new VertexPositionNormalTexture(btmLeftFront, normalTop, texcoords);
            vertexes[19] = new VertexPositionNormalTexture(btmLeftBack, normalTop, texcoords);
            vertexes[20] = new VertexPositionNormalTexture(btmRightBack, normalTop, texcoords);
            vertexes[21] = new VertexPositionNormalTexture(btmLeftFront, normalTop, texcoords);
            vertexes[22] = new VertexPositionNormalTexture(btmRightBack, normalTop, texcoords);
            vertexes[23] = new VertexPositionNormalTexture(btmRightFront, normalTop, texcoords);

            // Add the vertices for the LEFT face.
            vertexes[24] = new VertexPositionNormalTexture(topLeftFront, normalRight, texcoords);
            vertexes[25] = new VertexPositionNormalTexture(btmLeftBack, normalRight, texcoords);
            vertexes[26] = new VertexPositionNormalTexture(btmLeftFront, normalRight, texcoords);
            vertexes[27] = new VertexPositionNormalTexture(topLeftBack, normalRight, texcoords);
            vertexes[28] = new VertexPositionNormalTexture(btmLeftBack, normalRight, texcoords);
            vertexes[29] = new VertexPositionNormalTexture(topLeftFront, normalRight, texcoords);

            // Add the vertices for the RIGHT face. 
            vertexes[30] = new VertexPositionNormalTexture(topRightFront, normalLeft, texcoords);
            vertexes[31] = new VertexPositionNormalTexture(btmRightFront, normalLeft, texcoords);
            vertexes[32] = new VertexPositionNormalTexture(btmRightBack, normalLeft, texcoords);
            vertexes[33] = new VertexPositionNormalTexture(topRightBack, normalLeft, texcoords);
            vertexes[34] = new VertexPositionNormalTexture(topRightFront, normalLeft, texcoords);
            vertexes[35] = new VertexPositionNormalTexture(btmRightBack, normalLeft, texcoords);

            return vertexes;
        }
        #endregion
    }
}
