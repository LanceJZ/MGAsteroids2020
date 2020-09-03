using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Panther
{
    public abstract class Entity : DrawableGameComponent
    {
        #region Fields
        protected PositionedObject _PO;
        protected BasicEffect _effect;
        protected Vector3 _diffuseColor = Vector3.One;
        protected Vector3 _emissiveColor;
        protected Vector3 _lightDirection;
        protected Vector3 _ambiantColor;
        protected Vector3 _directionalLightColor;
        protected Camera _camera;
        protected Matrix _world = Matrix.Identity;
        #endregion
        #region Properties
        public virtual Vector3 Position
        {
            get => _PO.Position;
            set => _PO.Position = value;
        }

        public virtual Vector3 Rotation
        {
            get => _PO.Rotation;
            set => _PO.Rotation = value;
        }
        public virtual Vector3 Velocity
        {
            get => _PO.Velocity;
            set => _PO.Velocity = value;
        }
        public virtual Vector3 Acceleration
        {
            get => _PO.Acceleration;
            set => _PO.Acceleration = value;
        }
        public virtual Vector3 RotationVelocity
        {
            get => _PO.RotationVelocity;
            set => _PO.RotationVelocity = value;
        }
        public virtual Vector3 RotationAcceleration
        {
            get => _PO.RotationAcceleration;
            set => _PO.RotationAcceleration = value;
        }
        public float Scale
        {
            get => PO.Scale;
            set => PO.Scale = value;
        }
        public bool Moveable
        {
            get => _PO.Moveable;
            set => _PO.Moveable = value;
        }
        public bool Hit
        {
            get => _PO.Hit;
            set => _PO.Hit = value;
        }

        public new bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                Visible = value;
                _PO.Enabled = value;
            }
        }
        public PositionedObject PO { get => _PO; }
        public BasicEffect Effect { get => _effect; set => _effect = value; }
        public Vector3 LightDirection { get => _lightDirection; set => _lightDirection = value; }
        public Vector3 AmbiantColor { get => _ambiantColor; set => _ambiantColor = value; }
        public Vector3 DiffuseColor { get => _diffuseColor; set => _diffuseColor = value; }
        public Vector3 DirectionalLightColor { get => _directionalLightColor; set => _directionalLightColor = value; }
        public Vector3 EmissiveColor { get => _emissiveColor; set => _emissiveColor = value; }
        public float X { get => _PO.Position.X; set => _PO.Position.X = value; }
        public float Y { get => _PO.Position.Y; set => _PO.Position.Y = value; }
        public float Z { get => _PO.Position.Z; set => _PO.Position.Z = value; }
        public Matrix WorldMatrixRef { get => _world; }
        #endregion
        #region Constructor
        public Entity(Game game, Camera camera) : base(game)
        {
            _camera = camera;
            _PO = new PositionedObject(game);
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
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion
        public Matrix RotateMatrix(Vector3 rotation)
        {
            return Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z);
        }
        #region Spawn
        /// <summary>
        /// If position, rotation, rotation velocity and velocity are used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        /// <param name="rotation">Rotation to spawn at.</param>
        /// <param name="rotationVelocity">Initial Rotation Velocity to spawn with.</param>
        /// <param name="velocity">Initial Velocity to spawn with.</param>
        public virtual void Spawn(Vector3 position, Vector3 rotation, Vector3 rotationVelocity, Vector3 velocity)
        {
            _PO.Velocity = velocity;
            _PO.RotationVelocity = rotationVelocity;
            Spawn(position, rotation);
        }
        /// <summary>
        /// If position, rotation and velocity are used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        /// <param name="rotation">Rotation to spawn at.</param>
        /// <param name="velocity">Initial Velocity to spawn with.</param>
        public virtual void Spawn(Vector3 position, Vector3 rotation, Vector3 velocity)
        {
            _PO.Velocity = velocity;
            Spawn(position, rotation);
        }
        /// <summary>
        /// If only position and rotation are used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        /// <param name="rotation">Rotation to spawn at.</param>
        public virtual void Spawn(Vector3 position, Vector3 velocity)
        {
            _PO.Velocity = velocity;
            Spawn(position);
        }
        /// <summary>
        /// If only position is used.
        /// </summary>
        /// <param name="position">Position to spawn at.</param>
        public virtual void Spawn(Vector3 position)
        {
            Enabled = true;
            Visible = true;
            _PO.Hit = false;
            _PO.NewSpawn = true;
            _PO.Position = position;
        }
        #endregion
    }
}
