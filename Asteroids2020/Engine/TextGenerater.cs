using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public class TextGenerater : GameComponent
    {
        #region Fields
        Camera _camera;
        #endregion
        #region Properties
        #endregion
        #region Constructor
        public TextGenerater(Game game, Camera camera) : base(game)
        {
            _camera = camera;

            game.Components.Add(this);
        }
        #endregion
        #region Initialize
        public override void Initialize()
        {

            base.Initialize();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
        #endregion
        #region Public Methods
        #endregion
        #region Private/Protected Methods
        #endregion
    }
}
