using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public class Factory : GameComponent
    {
        Camera _camera;
        public Factory(Game game, Camera camera) : base(game)
        {
            _camera = camera;
            game.Components.Add(this);
        }

        public override void Initialize()
        {

            base.Initialize();
            LoadContent();
            BeginRun();
        }

        public void LoadContent()
        {

        }

        public void BeginRun()
        {

        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public List<ModelEntity> Spawn(List<ModelEntity> entities, Vector3 position)
        {
            bool makeNew = true;
            int thisOne = 0;


            foreach (ModelEntity item in entities)
            {
                if (!item.Enabled)
                {
                    makeNew = false;
                    break;
                }

                thisOne++;
            }

            if (makeNew)
            {
                entities.Add(new ModelEntity(Game, _camera));
                thisOne = entities.Count - 1;
            }

            entities[thisOne].Spawn(position);
            return entities;
        }
    }
}
