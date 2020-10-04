using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public class Factory
    {
        Camera cameraRef;
        Game gameRef;
        string name = "";
        public Factory(Game game, Camera camera)
        {
            cameraRef = camera;
            gameRef = game;
        }

        public List<ModelEntity> Spawn(List<ModelEntity> entities, Vector3 position)
        {
            return Spawn(entities, position, Vector3.Zero);
        }

        public List<ModelEntity> Spawn(List<ModelEntity> entities, Vector3 position, Vector3 velocity)
        {
            bool makeNew = true;
            int entity = entities.Count;


            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].Enabled)
                {
                    makeNew = false;
                    entity = i;
                    break;
                }
            }

            if (makeNew)
            {
                entities.Add(new ModelEntity(gameRef, cameraRef));
            }

            entities[entity].Spawn(position, velocity);
            return entities;
        }

        public List<VectorModel> SpawnVectorModel(List<VectorModel> entities, Vector3 position)
        {
            return SpawnVectorModel(entities, position, Vector3.Zero);
        }

        public List<VectorModel> SpawnVectorModel(List<VectorModel> entities, Vector3 position, Vector3 volocity)
        {
            bool makeNew = true;
            int entity = entities.Count;


            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].Enabled)
                {
                    makeNew = false;
                    entity = i;
                    break;
                }
            }

            if (makeNew)
            {
                entities.Add(new VectorModel(gameRef, cameraRef));
            }

            entities[entity].Spawn(position, volocity);
            return entities;
        }

        public List<VectorModel> Spawn(List<VectorModel> entities, Vector3 position, Vector3 volocity,
            Vector3[] verts, Color color)
        {
            bool makeNew = true;
            int entity = entities.Count;


            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].Enabled)
                {
                    makeNew = false;
                    entity = i;
                    break;
                }
            }

            if (makeNew)
            {
                entities.Add(new VectorModel(gameRef, cameraRef));
                entities.Last().PO.Radius = entities.Last().InitializePoints(verts, color, 1, name);
            }

            entities[entity].Spawn(position, volocity);
            return entities;
        }
    }
}

