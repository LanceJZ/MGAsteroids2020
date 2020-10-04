#region Using
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
#endregion

namespace Panther
{
    public class PositionedObject : GameComponent
    {
        #region Fields
        public PositionedObject ParentPO;
        public List<PositionedObject> ChildrenPOs;
        public List<PositionedObject> ParentPOs;
        // These are fields because XYZ are fields of Vector3, a struct,
        // so they do not get data binned as a property.
        public Vector3 Position = Vector3.Zero;
        public Vector3 Acceleration = Vector3.Zero;
        public Vector3 Velocity = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 RotationVelocity = Vector3.Zero;
        public Vector3 RotationAcceleration = Vector3.Zero;
        Vector3 _childPosition;
        Vector3 _orginalPosition;
        Vector2 _heightWidth;
        float _elapsedGameTime;
        float _scalePercent = 1;
        float _gameRefScale = 1;
        float _radius = 0;
        bool _newSpawn;
        bool _hit;
        bool _explosionActive;
        bool _isPaused;
        bool _isMoveable = true;
        bool _isActiveDependent;
        bool _isDirectConnected;
        bool _isParent;
        bool _isChild;
        bool _inDebugMode;
        bool _rotationDependent;
        bool _childUpdate;
        #endregion
        #region Properties

        public Vector3 WorldPosition
        {
            get
            {
                Vector3 parentPOs = Vector3.Zero;

                foreach (PositionedObject po in ParentPOs)
                {
                    parentPOs += po.Position;
                }

                return Position + parentPOs;
            }
        }

        public Vector3 WorldVelocity
        {
            get
            {
                Vector3 parentPOs = Vector3.Zero;

                foreach (PositionedObject po in ParentPOs)
                {
                    parentPOs += po.Velocity;
                }

                return Velocity + parentPOs;
            }
        }

        public Vector3 WorldAcceleration
        {
            get
            {
                Vector3 parentPOs = Vector3.Zero;

                foreach (PositionedObject po in ParentPOs)
                {
                    parentPOs += po.Acceleration;
                }

                return Acceleration + parentPOs;
            }
        }

        public Vector3 WorldRotation
        {
            get
            {
                Vector3 parentPOs = Vector3.Zero;

                foreach (PositionedObject po in ParentPOs)
                {
                    parentPOs += po.Rotation;
                }

                return Rotation + parentPOs;
            }
        }

        public Vector3 WorldRotationVelocity
        {
            get
            {
                Vector3 parentPOs = Vector3.Zero;

                foreach (PositionedObject po in ParentPOs)
                {
                    parentPOs += po.RotationVelocity;
                }

                return RotationVelocity + parentPOs;
            }
        }

        public Vector3 WorldRotationAcceleration
        {
            get
            {
                Vector3 parentPOs = Vector3.Zero;

                foreach (PositionedObject po in ParentPOs)
                {
                    parentPOs += po.RotationAcceleration;
                }

                return RotationAcceleration + parentPOs;
            }
        }

        public float ElapsedGameTime { get => _elapsedGameTime; }
        /// <summary>
        /// Scale by percent of original. If base of sprite, used to enlarge sprite.
        /// </summary>
        public float Scale { get => _scalePercent; set => _scalePercent = value; }
        /// <summary>
        /// Used for circle collusion. Sets radius of circle.
        /// </summary>
        public float Radius { get => _radius * _scalePercent; set => _radius = value; }
        /// <summary>
        /// Enabled means this class is a parent, and has at least one child.
        /// </summary>
        public bool Parent { get => _isParent; set => _isParent = value; }
        /// <summary>
        /// Enabled means this class is a child to a parent.
        /// </summary>
        public bool Child { get => _isChild; set => _isChild = value; }
        /// <summary>
        /// Enabled tells class hit by another class.
        /// </summary>
        public bool Hit { get => _hit; set => _hit = value; }
        /// <summary>
        /// Enabled tells class an explosion is active.
        /// </summary>
        public bool ExplosionActive { get => _explosionActive; set => _explosionActive = value; }
        /// <summary>
        /// Enabled pauses class update.
        /// </summary>
        public bool Pause { get => _isPaused; set => _isPaused = value; }
        /// <summary>
        /// Enabled will move using velocity and acceleration.
        /// </summary>
        public bool Moveable { get => _isMoveable; set => _isMoveable = value; }

        public bool ChildUpdate { get => _childUpdate; set => _childUpdate = value; }
        /// <summary>
        /// Enabled causes the class to update. If base of Sprite, enables sprite to be drawn.
        /// </summary>
        public new bool Enabled
        {
            get => base.Enabled;

            set
            {
                base.Enabled = value;

                foreach (PositionedObject child in ChildrenPOs)
                {
                    if (child.ActiveDependent)
                        child.Enabled = value;
                }
            }
        }

        public Vector3 OriginalPosition { get => _orginalPosition; set => _orginalPosition = value; }
        /// <summary>
        /// Enabled the active bool will mirror that of the parent.
        /// </summary>
        public bool ActiveDependent { get => _isActiveDependent; set => _isActiveDependent = value; }
        /// <summary>
        /// Enabled the position and rotation will always be the same as the parent.
        /// </summary>
        public bool DirectConnection { get => _isDirectConnected; set => _isDirectConnected = value; }

        public bool RotationDependent { get => _rotationDependent; set => _rotationDependent = value; }
        /// <summary>
        /// Gets or sets the GameModel's AABB
        /// </summary>
        public bool Debug { set => _inDebugMode = value; }

        public Vector2 WidthHeight { get => _heightWidth; set => _heightWidth = value; }

        public float GameScale { get => _gameRefScale; set => _gameRefScale = value; }

        public Rectangle BoundingBox
        {
            get => new Rectangle((int)Position.X, (int)Position.Y, (int)WidthHeight.X, (int)WidthHeight.Y);
        }

        public float X { get => Position.X; set => Position.X = value; }
        public float Y { get => Position.Y; set => Position.Y = value; }
        public float Z { get => Position.Z; set => Position.Z = value; }
        public bool NewSpawn { get => _newSpawn; set => _newSpawn = value; }
        #endregion
        #region Constructor
        /// <summary>
        /// This is the constructor that gets the Positioned Object ready for use and adds it to the Drawable Components list.
        /// </summary>
        /// <param name="game">The game class</param>
        public PositionedObject(Game game) : base(game)
        {
            ChildrenPOs = new List<PositionedObject>();
            ParentPOs = new List<PositionedObject>();
            Game.Components.Add(this);
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();
        }
        /// <summary>
        /// Allows the game component to be updated.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (_isMoveable)
            {
                base.Update(gameTime);

                _elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Velocity += Acceleration * _elapsedGameTime;
                Position += Velocity * _elapsedGameTime;
                RotationVelocity += RotationAcceleration * _elapsedGameTime;
                Rotation += RotationVelocity * _elapsedGameTime;

                Rotation = WrapAngle(Rotation);
            }

            if (_isChild)
            {
                UpdateChild();
            }

            base.Update(gameTime);
        }

        public void UpdateChild()
        {
            if (DirectConnection)
            {
                Position = ParentPO.Position;

                if (_rotationDependent)
                {
                    Rotation = ParentPO.Rotation;
                }
            }
            else
            {
                if (_childUpdate)
                {
                    Position = _orginalPosition + ParentPO.Position;

                    if (_rotationDependent)
                    {
                        Rotation = ParentPO.Rotation;
                    }
                }
            }
        }

        Vector3 WrapAngle(Vector3 angle)
        {
            if (angle.X < 0)
                angle.X += MathHelper.TwoPi;

            if (angle.Y < 0)
                angle.Y += MathHelper.TwoPi;

            if (angle.Z < 0)
                angle.Z += MathHelper.TwoPi;

            if (angle.X > MathHelper.TwoPi)
                angle.X -= MathHelper.TwoPi;

            if (angle.Y > MathHelper.TwoPi)
                angle.Y -= MathHelper.TwoPi;

            if (angle.Z > MathHelper.TwoPi)
                angle.Z -= MathHelper.TwoPi;

            return angle;
        }
        /// <summary>
        /// Adds child that is not active dependent and directly connected.
        /// </summary>
        /// <param name="parrent">The Parent to this class.</param>
        public virtual void AddAsChildOf(PositionedObject parrent)
        {
            AddAsChildOf(parrent, false, false, false, false);
        }
        /// <summary>
        /// Adds child that is directly connect.
        /// </summary>
        /// <param name="parrent">The parent to this class.</param>
        /// <param name="activeDependent">If this class is active when the parent is.</param>
        public virtual void AddAsChildOf(PositionedObject parrent, bool activeDependent)
        {
            AddAsChildOf(parrent, activeDependent, false, false, false);
        }
        /// <summary>
        /// Adds a child that is not child updated or rotation dependent.
        /// </summary>
        /// <param name="parrent">The parent to this class.</param>
        /// <param name="activeDependent">If this class is active when the parent is.</param>
        /// <param name="directConnection">If it is staying at child 0, 0, 0</param>
        public virtual void AddAsChildOf(PositionedObject parrent, bool activeDependent,
            bool directConnection)
        {
            AddAsChildOf(parrent, activeDependent, directConnection, false, false);
        }

        /// <summary>
        /// Add PO class or base PO class from AModel or Sprite as child of this class.
        /// Make sure all the parents of the parent are added before the children.
        /// </summary>
        /// <param name="parent">The parent to this class.</param>
        /// <param name="activeDependent">If this class is active when the parent is.</param>
        /// <param name="directConnection">Bind Position and Rotation to child.</param>
        public virtual void AddAsChildOf(PositionedObject parent, bool activeDependent,
            bool directConnection, bool rotationDependent, bool childUpdate)
        {
            if (ParentPO != null)
                return;

            ActiveDependent = activeDependent;
            DirectConnection = directConnection;
            RotationDependent = rotationDependent;
            Child = true;
            ChildUpdate = childUpdate;

            ParentPO = parent;
            ParentPO.Parent = true;
            ParentPO.ChildrenPOs.Add(this);
            ParentPOs.Add(parent);

            for (int i = 0; i < ParentPOs.Count; i++)
            {
                if (ParentPOs[i].ParentPO != null && ParentPOs[i].ParentPO != parent)
                {
                    ParentPOs.Add(ParentPOs[i].ParentPO);
                }
            }
        }

        public void ChildLink(bool active)
        {
            if (!active)
            {
                _childPosition = Position;
                Position = WorldPosition;
                ParentPOs.Remove(ParentPO);
                ParentPO.ChildrenPOs.Remove(this);
            }
            else
            {
                Position = _childPosition;
                ParentPOs.Add(ParentPO);
                ParentPO.ChildrenPOs.Add(this);

                for (int i = 0; i < ParentPOs.Count; i++)
                {
                    if (ParentPOs[i].ParentPO != null && ParentPOs[i].ParentPO != ParentPO)
                    {
                        ParentPOs.Add(ParentPOs[i].ParentPO);
                    }
                }
            }

            Child = active;
            ParentPO.Parent = active;
        }

        public void Remove()
        {
            Game.Components.Remove(this);
        }
        /// <summary>
        /// Circle collusion detection. Target circle will be compared to this class's.
        /// Will return true of they intersect. Only for use with 2D Z plane.
        /// </summary>
        /// <param name="target">Target Positioned Object.</param>
        /// <returns></returns>
        public bool CirclesIntersect(PositionedObject target)
        {
            if (!Enabled || !target.Enabled)
                return false;

            float distanceX = target.X - X;
            float distanceY = target.Y - Y;
            float radius = Radius + target.Radius;

            if ((distanceX * distanceX) + (distanceY * distanceY) < radius * radius)
                return true;

            return false;
        }
        /// <summary>
        /// Returns a float of the angle in radians to target, using only the X and Y.
        /// </summary>
        /// <param name="target"></param>
        /// <returns>float</returns>
        public float AngleFromVectorsZ(Vector3 target)
        {
            return MathF.Atan2(target.Y - Y, target.X - X);
        }

        public bool OffScreenSide()
        {
            if (X > Core.ScreenWidth || X < -Core.ScreenWidth)
            {
                return true;
            }

            return false;
        }

        public bool OffScreenTopBottom()
        {
            if (Y > Core.ScreenHeight || Y < -Core.ScreenHeight)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
