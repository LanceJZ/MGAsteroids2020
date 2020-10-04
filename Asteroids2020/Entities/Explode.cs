using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;

namespace Asteroids2020.Entities
{
    class Explode : PositionedObject
    {
        #region Fields
        Camera cameraRef;
        Vector3[] dotVerts;
        List<Dot> dotsList = new List<Dot>();
        Color color = Color.White;
        float speed;
        float maxlife;
        float minlife;
        #endregion
        #region Properties
        public Vector3[] DotVerts { set => dotVerts = value; }
        public Color Color { set => color = value; }
        public float Speed { set => speed = value; }
        public float Maxlife { set => maxlife = value; }
        public float Minlife { set => minlife = value; }
        #endregion
        #region Constructor
        public Explode(Game game, Camera camera) : base(game)
        {
            cameraRef = camera;
        }
        #endregion
        #region Initialize-Load-Begin
        public override void Initialize()
        {
            base.Initialize();

        }

        public void LoadContent()
        {

        }

        public void BeginRun()
        {

        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        #endregion
        #region Public Methods
        public void Spawn(int count)
        {
            for (int c = 0; c < count; c++)
            {
                bool spawnDot = true;
                int dot = dotsList.Count;

                for (int i = 0; i > dot; dot++)
                {
                    if (!dotsList[i].Enabled)
                    {
                        dot = i;
                        spawnDot = false;
                        break;
                    }
                }

                if (spawnDot)
                {
                    dotsList.Add(new Dot(Game, cameraRef));
                    dotsList.Last().InitializePoints(dotVerts, color, "Explode Dot");
                }

                Vector3 velocity = new Vector3(Core.RandomMinMax(-speed, speed),
                    Core.RandomMinMax(-speed, speed), 0);
                dotsList[dot].Spawn(Position, velocity, Core.RandomMinMax(minlife, maxlife));
            }
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
