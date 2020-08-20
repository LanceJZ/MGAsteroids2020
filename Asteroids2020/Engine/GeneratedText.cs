using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public static class GeneratedText
    {
        #region Fields
        #endregion
        #region Properties
        #endregion
        #region Public Methods

        static public VertexPositionNormalTexture[] HorizontalLine()
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[36];

            Vector2 texcoords = new Vector2(0f, 0f);

            float width = 5.5f;
            float height = 0.75f;
            float depth = 0.5f;
            float tip = 1.5f;

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = new Vector3(-width, height, -depth);
            Vector3 topLeftBack = new Vector3(-width, height, depth);
            Vector3 topRightFront = new Vector3(width, height, -depth);
            Vector3 topRightBack = new Vector3(width, height, depth);

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = new Vector3(-width + tip, -height, -depth);
            Vector3 btmLeftBack = new Vector3(-width + tip, -height, depth);
            Vector3 btmRightFront = new Vector3(width - tip, -height, -depth);
            Vector3 btmRightBack = new Vector3(width - tip, -height, depth);

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

        static public VertexPositionNormalTexture[] MiddleLine(float width)
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[60];

            Vector2 texcoords = new Vector2(0f, 0f);

            //float width = 4.0f;
            float height = 1.0f;
            float depth = 0.5f;

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

            // Center vertices on ends.
            Vector3 centerLeftBack = new Vector3(-width - 1, 0, depth);
            Vector3 centerLeftFront = new Vector3(-width - 1, 0, -depth);
            Vector3 centerRightBack = new Vector3(width + 1, 0, depth);
            Vector3 centerRightFront = new Vector3(width + 1, 0, -depth);


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

            vertexes[8] = new VertexPositionNormalTexture(centerRightFront, normalBack, texcoords);
            vertexes[7] = new VertexPositionNormalTexture(btmRightFront, normalBack, texcoords);
            vertexes[6] = new VertexPositionNormalTexture(topRightFront, normalBack, texcoords);

            vertexes[11] = new VertexPositionNormalTexture(centerLeftFront, normalBack, texcoords);
            vertexes[10] = new VertexPositionNormalTexture(topLeftFront, normalBack, texcoords);
            vertexes[9] = new VertexPositionNormalTexture(btmLeftFront, normalBack, texcoords);

            // Add the vertices for the BACK face.
            vertexes[12] = new VertexPositionNormalTexture(topLeftBack, normalFront, texcoords);
            vertexes[13] = new VertexPositionNormalTexture(topRightBack, normalFront, texcoords);
            vertexes[14] = new VertexPositionNormalTexture(btmLeftBack, normalFront, texcoords);
            vertexes[15] = new VertexPositionNormalTexture(btmLeftBack, normalFront, texcoords);
            vertexes[16] = new VertexPositionNormalTexture(topRightBack, normalFront, texcoords);
            vertexes[17] = new VertexPositionNormalTexture(btmRightBack, normalFront, texcoords);

            vertexes[20] = new VertexPositionNormalTexture(topRightBack, normalFront, texcoords);
            vertexes[19] = new VertexPositionNormalTexture(btmRightBack, normalFront, texcoords);
            vertexes[18] = new VertexPositionNormalTexture(centerRightBack, normalFront, texcoords);

            vertexes[23] = new VertexPositionNormalTexture(btmLeftBack, normalFront, texcoords);
            vertexes[22] = new VertexPositionNormalTexture(topLeftBack, normalFront, texcoords);
            vertexes[21] = new VertexPositionNormalTexture(centerLeftBack, normalFront, texcoords);

            // Add the vertices for the TOP face.
            vertexes[24] = new VertexPositionNormalTexture(topLeftFront, normalBottom, texcoords);
            vertexes[25] = new VertexPositionNormalTexture(topRightBack, normalBottom, texcoords);
            vertexes[26] = new VertexPositionNormalTexture(topLeftBack, normalBottom, texcoords);
            vertexes[27] = new VertexPositionNormalTexture(topLeftFront, normalBottom, texcoords);
            vertexes[28] = new VertexPositionNormalTexture(topRightFront, normalBottom, texcoords);
            vertexes[29] = new VertexPositionNormalTexture(topRightBack, normalBottom, texcoords);

            // Add the vertices for the BOTTOM face. 
            vertexes[30] = new VertexPositionNormalTexture(btmLeftFront, normalTop, texcoords);
            vertexes[31] = new VertexPositionNormalTexture(btmLeftBack, normalTop, texcoords);
            vertexes[32] = new VertexPositionNormalTexture(btmRightBack, normalTop, texcoords);
            vertexes[33] = new VertexPositionNormalTexture(btmLeftFront, normalTop, texcoords);
            vertexes[34] = new VertexPositionNormalTexture(btmRightBack, normalTop, texcoords);
            vertexes[35] = new VertexPositionNormalTexture(btmRightFront, normalTop, texcoords);

            // Add the vertices for the LEFT face.
            vertexes[36] = new VertexPositionNormalTexture(topLeftFront, normalRight, texcoords);
            vertexes[37] = new VertexPositionNormalTexture(centerLeftBack, normalRight, texcoords);
            vertexes[38] = new VertexPositionNormalTexture(centerLeftFront, normalRight, texcoords);
            vertexes[39] = new VertexPositionNormalTexture(topLeftBack, normalRight, texcoords);
            vertexes[40] = new VertexPositionNormalTexture(centerLeftBack, normalRight, texcoords);
            vertexes[41] = new VertexPositionNormalTexture(topLeftFront, normalRight, texcoords);
            vertexes[42] = new VertexPositionNormalTexture(centerLeftFront, normalRight, texcoords);
            vertexes[43] = new VertexPositionNormalTexture(btmLeftBack, normalRight, texcoords);
            vertexes[44] = new VertexPositionNormalTexture(btmLeftFront, normalRight, texcoords);
            vertexes[45] = new VertexPositionNormalTexture(centerLeftBack, normalRight, texcoords);
            vertexes[46] = new VertexPositionNormalTexture(btmLeftBack, normalRight, texcoords);
            vertexes[47] = new VertexPositionNormalTexture(centerLeftFront, normalRight, texcoords);

            // Add the vertices for the RIGHT face. 
            vertexes[48] = new VertexPositionNormalTexture(topRightFront, normalLeft, texcoords);
            vertexes[49] = new VertexPositionNormalTexture(centerRightFront, normalLeft, texcoords);
            vertexes[50] = new VertexPositionNormalTexture(centerRightBack, normalLeft, texcoords);
            vertexes[51] = new VertexPositionNormalTexture(topRightBack, normalLeft, texcoords);
            vertexes[52] = new VertexPositionNormalTexture(topRightFront, normalLeft, texcoords);
            vertexes[53] = new VertexPositionNormalTexture(centerRightBack, normalLeft, texcoords);
            vertexes[54] = new VertexPositionNormalTexture(centerRightFront, normalLeft, texcoords);
            vertexes[55] = new VertexPositionNormalTexture(btmRightFront, normalLeft, texcoords);
            vertexes[56] = new VertexPositionNormalTexture(btmRightBack, normalLeft, texcoords);
            vertexes[57] = new VertexPositionNormalTexture(centerRightBack, normalLeft, texcoords);
            vertexes[58] = new VertexPositionNormalTexture(centerRightFront, normalLeft, texcoords);
            vertexes[59] = new VertexPositionNormalTexture(btmRightBack, normalLeft, texcoords);

            return vertexes;
        }

        static public VertexPositionNormalTexture[] VerticalLine()
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[36];

            Vector2 texcoords = new Vector2(0f, 0f);

            float width = 0.75f;
            float height = 4.25f;
            float depth = 0.5f;
            float tip = 1.5f;

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = new Vector3(-width, height - tip, -depth);
            Vector3 topLeftBack = new Vector3(-width, height - tip, depth);
            Vector3 topRightFront = new Vector3(width, height, -depth);
            Vector3 topRightBack = new Vector3(width, height, depth);

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = new Vector3(-width, -height + tip, -depth);
            Vector3 btmLeftBack = new Vector3(-width, -height + tip, depth);
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

        static public VertexPositionNormalTexture[] DiagnialLine()
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[36];

            Vector2 texcoords = new Vector2(0f, 0f);

            float width = 1.0f;
            float height = 0.75f;
            float depth = 0.1f;
            float tip = 2.0f;

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = new Vector3(-width, height, -depth);
            Vector3 topLeftBack = new Vector3(-width, height, depth);
            Vector3 topRightFront = new Vector3(width + tip + 0.05f, height, -depth);
            Vector3 topRightBack = new Vector3(width + tip + 0.05f, height, depth);

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = new Vector3(-width - tip, -height, -depth);
            Vector3 btmLeftBack = new Vector3(-width - tip, -height, depth);
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

        static public VertexPositionNormalTexture[] L()
        {
            VertexPositionNormalTexture[] vertexes = new VertexPositionNormalTexture[60];
            // Primitive Count = 20
            Vector2 texcoords = new Vector2(0f, 0f);

            //Bottom vertices
            Vector3 bottomLeftBack = new Vector3(-2.0f, -4, -0.5f);
            Vector3 bottomLeftFront = new Vector3(-2.0f, -4, 0.5f);
            Vector3 bottomRightFrontHorizontal = new Vector3(4.0f, -4, 0.5f);
            Vector3 bottomRightBackHorizontal = new Vector3(4.0f, -4, -0.5f);

            //Top Vertical
            Vector3 topLeftFrontVertical = new Vector3(-2.0f, 7, 0.5f);
            Vector3 topLeftBackVertical = new Vector3(-2.0f, 7, -0.5f);
            Vector3 topRightFrontVertical = new Vector3(-1.0f, 7, 0.5f);
            Vector3 topRightBackVertical = new Vector3(-1.0f, 7, -0.5f);
            Vector3 topRightFrontHorizontal = new Vector3(4.0f, -3, 0.5f);
            Vector3 topRightBackHorizontal = new Vector3(4.0f, -3, -0.5f);

            //Inside vertices
            Vector3 insideFront = new Vector3(-1.0f, -3, 0.5f);
            Vector3 insideBack = new Vector3(-1.0f, -3, -0.5f);

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f);
            Vector3 normalTop = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 normalBottom = new Vector3(0.0f, -1.0f, 0.0f);
            Vector3 normalLeft = new Vector3(-1.0f, 0.0f, 0.0f);
            Vector3 normalRight = new Vector3(1.0f, 0.0f, 0.0f);

            // Add the vertices for the FRONT Vertical face. Done
            vertexes[0] = new VertexPositionNormalTexture(topLeftFrontVertical, normalFront, texcoords);
            vertexes[1] = new VertexPositionNormalTexture(topRightFrontVertical, normalFront, texcoords);
            vertexes[2] = new VertexPositionNormalTexture(insideFront, normalFront, texcoords);
            vertexes[3] = new VertexPositionNormalTexture(insideFront, normalFront, texcoords);
            vertexes[4] = new VertexPositionNormalTexture(bottomLeftFront, normalFront, texcoords);
            vertexes[5] = new VertexPositionNormalTexture(topLeftFrontVertical, normalFront, texcoords);

            // Add the vertices for the Front horizontal face. Done
            vertexes[6] = new VertexPositionNormalTexture(topRightFrontHorizontal, normalFront, texcoords);
            vertexes[7] = new VertexPositionNormalTexture(bottomRightFrontHorizontal, normalFront, texcoords);
            vertexes[8] = new VertexPositionNormalTexture(bottomLeftFront, normalFront, texcoords);
            vertexes[9] = new VertexPositionNormalTexture(topRightFrontHorizontal, normalFront, texcoords);
            vertexes[10] = new VertexPositionNormalTexture(bottomLeftFront, normalFront, texcoords);
            vertexes[11] = new VertexPositionNormalTexture(insideFront, normalFront, texcoords);

            // Add the vertices for the Back vertical face. Done
            vertexes[12] = new VertexPositionNormalTexture(insideBack, normalBack, texcoords);
            vertexes[13] = new VertexPositionNormalTexture(topRightBackVertical, normalBack, texcoords);
            vertexes[14] = new VertexPositionNormalTexture(topLeftBackVertical, normalBack, texcoords);
            vertexes[15] = new VertexPositionNormalTexture(topLeftBackVertical, normalBack, texcoords);
            vertexes[16] = new VertexPositionNormalTexture(bottomLeftBack, normalBack, texcoords);
            vertexes[17] = new VertexPositionNormalTexture(insideBack, normalBack, texcoords);

            // Add the vertices for the Back horizontal face. Done
            vertexes[18] = new VertexPositionNormalTexture(insideBack, normalBack, texcoords);
            vertexes[19] = new VertexPositionNormalTexture(bottomLeftBack, normalBack, texcoords);
            vertexes[20] = new VertexPositionNormalTexture(topRightBackHorizontal, normalBack, texcoords);
            vertexes[21] = new VertexPositionNormalTexture(bottomLeftBack, normalBack, texcoords);
            vertexes[22] = new VertexPositionNormalTexture(bottomRightBackHorizontal, normalBack, texcoords);
            vertexes[23] = new VertexPositionNormalTexture(topRightBackHorizontal, normalBack, texcoords);

            // Add the vertices for the TOP vertical face. Done
            vertexes[24] = new VertexPositionNormalTexture(topRightBackVertical, normalTop, texcoords);
            vertexes[25] = new VertexPositionNormalTexture(topRightFrontVertical, normalTop, texcoords);
            vertexes[26] = new VertexPositionNormalTexture(topLeftFrontVertical, normalTop, texcoords);
            vertexes[27] = new VertexPositionNormalTexture(topLeftBackVertical, normalTop, texcoords);
            vertexes[28] = new VertexPositionNormalTexture(topRightBackVertical, normalTop, texcoords);
            vertexes[29] = new VertexPositionNormalTexture(topLeftFrontVertical, normalTop, texcoords);

            // Add the vertices for the Top horizontal face. Done
            vertexes[30] = new VertexPositionNormalTexture(topRightBackHorizontal, normalTop, texcoords);
            vertexes[31] = new VertexPositionNormalTexture(topRightFrontHorizontal, normalTop, texcoords);
            vertexes[32] = new VertexPositionNormalTexture(insideFront, normalTop, texcoords);
            vertexes[33] = new VertexPositionNormalTexture(topRightBackHorizontal, normalTop, texcoords);
            vertexes[34] = new VertexPositionNormalTexture(insideFront, normalTop, texcoords);
            vertexes[35] = new VertexPositionNormalTexture(insideBack, normalTop, texcoords);

            // vertices for bottom face. Done
            vertexes[36] = new VertexPositionNormalTexture(bottomLeftBack, normalBottom, texcoords);
            vertexes[37] = new VertexPositionNormalTexture(bottomLeftFront, normalBottom, texcoords);
            vertexes[38] = new VertexPositionNormalTexture(bottomRightBackHorizontal, normalBottom, texcoords);
            vertexes[39] = new VertexPositionNormalTexture(bottomLeftFront, normalBottom, texcoords);
            vertexes[40] = new VertexPositionNormalTexture(bottomRightFrontHorizontal, normalBottom, texcoords);
            vertexes[41] = new VertexPositionNormalTexture(bottomRightBackHorizontal, normalBottom, texcoords);

            // vertices for left face. Done
            vertexes[42] = new VertexPositionNormalTexture(topLeftFrontVertical, normalLeft, texcoords);
            vertexes[43] = new VertexPositionNormalTexture(bottomLeftFront, normalLeft, texcoords);
            vertexes[44] = new VertexPositionNormalTexture(bottomLeftBack, normalLeft, texcoords);
            vertexes[45] = new VertexPositionNormalTexture(topLeftFrontVertical, normalLeft, texcoords);
            vertexes[46] = new VertexPositionNormalTexture(bottomLeftBack, normalLeft, texcoords);
            vertexes[47] = new VertexPositionNormalTexture(topLeftBackVertical, normalLeft, texcoords);

            // vertices for right vertical face. Done
            vertexes[48] = new VertexPositionNormalTexture(topRightBackVertical, normalRight, texcoords);
            vertexes[49] = new VertexPositionNormalTexture(insideBack, normalRight, texcoords);
            vertexes[50] = new VertexPositionNormalTexture(insideFront, normalRight, texcoords);
            vertexes[51] = new VertexPositionNormalTexture(topRightBackVertical, normalRight, texcoords);
            vertexes[52] = new VertexPositionNormalTexture(insideFront, normalRight, texcoords);
            vertexes[53] = new VertexPositionNormalTexture(topRightFrontVertical, normalRight, texcoords);

            // vertices for right horzontal face. Done
            vertexes[54] = new VertexPositionNormalTexture(topRightBackHorizontal, normalRight, texcoords);
            vertexes[55] = new VertexPositionNormalTexture(bottomRightBackHorizontal, normalRight, texcoords);
            vertexes[56] = new VertexPositionNormalTexture(bottomRightFrontHorizontal, normalRight, texcoords);
            vertexes[57] = new VertexPositionNormalTexture(topRightBackHorizontal, normalRight, texcoords);
            vertexes[58] = new VertexPositionNormalTexture(bottomRightFrontHorizontal, normalRight, texcoords);
            vertexes[59] = new VertexPositionNormalTexture(topRightFrontHorizontal, normalRight, texcoords);

            return vertexes;
        }
        #endregion
        #region Private/Protected Methods
        #endregion
    }
}
