using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Panther
{
    public static class Core
    {
        #region Fields
        static GraphicsDeviceManager _graphicsDM;
        static GraphicsDevice _graphics;
        static SpriteBatch _spriteBatch;
        readonly static Random _randomNumberGenerator = new Random(DateTime.Now.Millisecond);
        static Game _game;
        static KeyboardState _keyStateOld;
        public static float ScreenWidth;
        public static float ScreenHeight;
        #endregion
        #region Properties
        public static Random Rand { get => _randomNumberGenerator; }
        public static GraphicsDeviceManager GraphicsDM { get => _graphicsDM; }
        public static GraphicsDevice Graphics { get => _graphics; }
        public static SpriteBatch SpriteBatch { get => _spriteBatch; }
        public static Game TheGame { get => _game; }
        /// <summary>
        /// Returns the window size in pixels, of the height.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowHeight { get => _graphicsDM.PreferredBackBufferHeight; }
        /// <summary>
        /// Returns the window size in pixels, of the width.
        /// </summary>
        /// <returns>int</returns>
        public static int WindowWidth { get => _graphicsDM.PreferredBackBufferWidth; }
        /// <summary>
        /// Returns The Windows size in pixels as a Vector2.
        /// </summary>
        public static Vector2 WindowSize
        {
            get => new Vector2(_graphicsDM.PreferredBackBufferWidth,
                _graphicsDM.PreferredBackBufferHeight);
        }
        #endregion
        #region Initialize
        public static void Initialize(Game game, GraphicsDeviceManager graphicsDeviceManager,
            SpriteBatch spriteBatch)
        {
            _game = game;
            _graphicsDM = graphicsDeviceManager;
            _graphics = graphicsDeviceManager.GraphicsDevice;
            _spriteBatch = spriteBatch;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Get a random float between min and max
        /// </summary>
        /// <param name="min">the minimum random value</param>
        /// <param name="max">the maximum random value</param>
        /// <returns>float</returns>
        public static float RandomMinMax(float min, float max)
        {
            return min + (float)_randomNumberGenerator.NextDouble() * (max - min);
        }
        /// <summary>
        /// Get a random int between min and max
        /// </summary>
        /// <param name="min">the minimum random value</param>
        /// <param name="max">the maximum random value</param>
        /// <returns>int</returns>
        public static int RandomMinMax(int min, int max)
        {
            return min + (int)(_randomNumberGenerator.NextDouble() * ((max + 1) - min));
        }
        /// <summary>
        /// Loads XNA Model from file using the filename. Stored in Content/Models/
        /// </summary>
        /// <param name="modelFileName">File name of model to load.</param>
        /// <returns>XNA Model</returns>
        public static Model LoadModel(string modelFileName)
        {
            if (modelFileName != "")
            {
                if (File.Exists("Content/Models/" + modelFileName + ".xnb"))
                    return _game.Content.Load<Model>("Models/" + modelFileName);

                DebugConsole("The Model File " + modelFileName + " was not found.");
            }
            else
                DebugConsole("The Model File Name was empty");

            return null;
        }
        /// <summary>
        /// Loads Texture2D from file using the filename. Stored in Content/Textures
        /// </summary>
        /// <param name="textureFileName">File Name of the texture.</param>
        /// <returns></returns>
        public static Texture2D LoadTexture(string textureFileName)
        {
            if (textureFileName != "")
            {
                if (File.Exists("Content/Textures/" + textureFileName + ".xnb"))
                    return _game.Content.Load<Texture2D>("Textures/" + textureFileName);
            }

            DebugConsole("The Texture File " + textureFileName + " was not found.");
            return null;
        }

        public static SoundEffect LoadSoundEffect(string soundFileName)
        {
            if (soundFileName != "")
            {
                if (File.Exists("Content/Sounds/" + soundFileName + ".xnb"))
                {
                    return _game.Content.Load<SoundEffect>("Sounds/" + soundFileName);
                }
                else
                {
                    DebugConsole("The Sound File " + soundFileName + " was not found.");
                }
            }

            return null;
        }

        public static SoundEffectInstance LoadSoundEffectInstance(string soundFileName)
        {
            if (soundFileName != "")
            {
                if (File.Exists("Content/Sounds/" + soundFileName + ".xnb"))
                {
                    return _game.Content.Load<SoundEffect>("Sounds/" + soundFileName).CreateInstance();
                }
                else
                {
                    DebugConsole("The Texture File " + soundFileName + " was not found.");
                }
            }

            return null;
        }

        public static bool KeyDown(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                return true;
            }

            return false;
        }

        public static bool KeyPressed(Keys key)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState != _keyStateOld)
            {
                if (keyState.IsKeyDown(key))
                {
                    return true;
                }
            }

            return false;
        }

        public static void UpdateKeys()
        {
            _keyStateOld = Keyboard.GetState();
        }


        public static Vector3 RandomVelocity(float speed, float radianDirection)
        {
            float amt = Core.RandomMinMax(speed * 0.15f, speed);
            return VelocityFromAngleZ(radianDirection, amt);
        }
        /// <summary>
        /// Returns a velocity with Z as the ground plane. Y as up.
        /// X as the Y, Y as the Z.
        /// </summary>
        /// <param name="angle">Angel as Vector2 Y and Z of object.</param>
        /// <param name="magnitude">How fast</param>
        /// <returns>Vector3 velocity</returns>
        public static Vector3 VelocityFromAngle(Vector2 angle, float magnitude)
        {
            return new Vector3(MathF.Cos(angle.X) * magnitude,
                MathF.Sin(angle.Y) * magnitude,
                -(MathF.Sin(angle.X) * magnitude));
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from angle and magnitude.
        /// Only X and Z are calculated, Z = 0.
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public static Vector3 VelocityFromAngleY(float rotation, float magnitude)
        {
            return new Vector3(MathF.Cos(rotation) * magnitude,
                0, -(MathF.Sin(rotation) * magnitude));
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from angle and magnitude.
        /// Angle 0 is X positive.
        /// Only X and Y are calculated, Z = 0.
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public static Vector3 VelocityFromAngleZ(float rotation, float magnitude)
        {
            return new Vector3(MathF.Cos(rotation) * magnitude,
                MathF.Sin(rotation) * magnitude, 0);
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from random angle and set magnitude. Y is ignored.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public static Vector3 VelocityFromAngleY(float magnitude)
        {
            float angle = RandomRadian();
            return new Vector3(MathF.Cos(angle) * magnitude, 0,
                -(MathF.Sin(angle) * magnitude));
        }
        /// <summary>
        /// Returns a Vector3 direction of travel from random angle and set magnitude. Z is ignored.
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns>Vector3</returns>
        public static Vector3 VelocityFromAngleZ(float magnitude)
        {
            float angle = RandomRadian();
            return new Vector3(MathF.Cos(angle) * magnitude, MathF.Sin(angle) * magnitude, 0);
        }

        public static Vector2 RandomEdge()
        {
            return new Vector2(Core.WindowWidth * 0.5f,
                Core.RandomMinMax(-Core.WindowHeight * 0.45f, Core.WindowHeight * 0.45f));
        }
        /// <summary>
        /// Aims at target using the Y ground Plane.
        /// Only X and Z are used in the calculation.
        /// </summary>
        /// <param name="target">Vector3</param>
        /// <param name="facingAngle">float</param>
        /// <param name="magnitude">float</param>
        /// <returns></returns>
        public static float AimAtTargetY(Vector3 origin, Vector3 target,
            float facingAngle, float magnitude)
        {
            float turnVelocity = 0;
            float targetAngle = AngleFromVectorsY(origin, target);
            float targetLessFacing = targetAngle - facingAngle;
            float facingLessTarget = facingAngle - targetAngle;

            if (MathF.Abs(targetLessFacing) > MathHelper.Pi)
            {
                if (facingAngle > targetAngle)
                {
                    facingLessTarget = ((MathHelper.TwoPi - facingAngle) + targetAngle) * -1;
                }
                else
                {
                    facingLessTarget = (MathHelper.TwoPi - targetAngle) + facingAngle;
                }
            }

            if (facingLessTarget > 0)
            {
                turnVelocity = -magnitude;
            }
            else
            {
                turnVelocity = magnitude;
            }

            return turnVelocity;
        }

        public static bool AimedAtTargetY(Vector3 origin, Vector3 target,
            float facingAngle, float accuricy)
        {
            float targetAngle = AngleFromVectorsY(origin, target);
            float targetLessFacing = targetAngle - facingAngle;
            float facingLessTarget = facingAngle - targetAngle;

            if (MathF.Abs(targetLessFacing) > MathHelper.Pi)
            {
                if (facingAngle > targetAngle)
                {
                    facingLessTarget = ((MathHelper.TwoPi - facingAngle) + targetAngle) * -1;
                }
                else
                {
                    facingLessTarget = (MathHelper.TwoPi - targetAngle) + facingAngle;
                }
            }

            if (facingLessTarget < accuricy && facingLessTarget > -accuricy)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Aims at target using the Z ground Plane.
        /// Only X and Y are used in the calculation.
        /// </summary>
        /// <param name="target">Vector3</param>
        /// <param name="facingAngle">float</param>
        /// <param name="magnitude">float</param>
        /// <returns></returns>
        public static float AimAtTargetZ(Vector3 origin, Vector3 target,
            float facingAngle, float magnitude)
        {
            float turnVelocity = 0;
            float targetAngle = AngleFromVectorsZ(origin, target);
            float targetLessFacing = targetAngle - facingAngle;
            float facingLessTarget = facingAngle - targetAngle;

            if (MathF.Abs(targetLessFacing) > MathHelper.Pi)
            {
                if (facingAngle > targetAngle)
                {
                    facingLessTarget = ((MathHelper.TwoPi - facingAngle) + targetAngle) * -1;
                }
                else
                {
                    facingLessTarget = (MathHelper.TwoPi - targetAngle) + facingAngle;
                }
            }

            if (facingLessTarget > 0)
            {
                turnVelocity = -magnitude;
            }
            else
            {
                turnVelocity = magnitude;
            }

            return turnVelocity;
        }
        /// <summary>
        /// Return true if PO goes beyond Borders + compSize
        /// </summary>
        /// <param name="Borders">X and Y of play area. Zero as center.</param>
        /// <param name="compSize">Size of PO to compensate for when checking against borders.</param>
        /// <returns></returns>
        public static bool CheckPlayBorders(Vector3 origin, Vector2 Borders, Vector2 compSize)
        {
            if (origin.X + compSize.X > Borders.X)
                return true;

            if (origin.X - compSize.X < -Borders.X)
                return true;

            if (origin.Y + compSize.Y > Borders.Y)
                return true;

            if (origin.Y - compSize.Y < -Borders.Y)
                return true;

            return false;
        }
        /// <summary>
        /// Wrap PO from top to bottom, bottom to top.
        /// </summary>
        /// <param name="height">Play area height.</param>
        public static Vector3 WrapTopBottom(Vector3 position, float height)
        {
            if (position.Y > height)
                position.Y = -height;
            else if (position.Y < -height)
                position.Y = height;

            return position;
        }
        /// <summary>
        /// Wrap PO from side to side.
        /// </summary>
        /// <param name="width">Play area width.</param>
        public static Vector3 WrapSideToSide(Vector3 position, float width)
        {
            if (position.X > width)
                position.X = -width;
            else if (position.X < -width)
                position.X = width;

            return position;
        }

        public static Vector3 CheckWindowBorders(Vector3 position, float width, float height)
        {
            if (position.X + width > Core.WindowWidth * 0.5f)
                position.X = width + Core.WindowWidth * 0.5f;

            if (position.X + width < -Core.WindowWidth * 0.5f)
                position.X = width + -Core.WindowWidth * 0.5f;

            if (position.Y + height > Core.WindowHeight * 0.5f)
                position.Y = width - Core.WindowHeight * 0.5f;

            if (position.Y + height < -Core.WindowHeight * 0.5f)
                position.Y = width - -Core.WindowHeight * 0.5f;

            return position;
        }

        public static Vector3 CheckWindowSideBorders(Vector3 position, float width)
        {
            if (position.X + width > Core.WindowWidth * 0.5f)
                position.X = width + Core.WindowWidth * 0.5f;

            if (position.X - width < Core.WindowWidth * 0.5f)
                position.X = width - Core.WindowWidth * 0.5f;

            return position;
        }

        public static Vector3 CheckWindowTopBottomBorders(Vector3 position, float height)
        {
            if (position.Y + height > Core.WindowHeight * 0.5f)
                position.Y = height + Core.WindowHeight * 0.5f;

            if (position.Y - height < -Core.WindowHeight * 0.5f)
                position.Y = height - Core.WindowHeight * 0.5f;

            return position;
        }
        /// <summary>
        /// Circle collusion detection. Target circle will be compared to this class's.
        /// Will return true of they intersect. Only for use with 2D Z plane.
        /// </summary>
        /// <param name="target">Position of target.</param>
        /// <param name="targetRadius">Radius of target.</param>
        /// <returns></returns>
        public static bool CirclesIntersect(Vector3 origin, float radius, Vector3 target,
            float targetRadius)
        {
            float distanceX = target.X - origin.X;
            float distanceY = target.Y - origin.Y;
            float totalRadius = radius + targetRadius;

            if ((distanceX * distanceX) + (distanceY * distanceY) < radius * totalRadius)
                return true;

            return false;
        }
        /// <summary>
        /// Circle collusion detection. Target circle will be compared to origin circle.
        /// Will return true of they intersect. Only for use with 2D Z plane.
        /// </summary>
        /// <param name="target">Target Positioned Object.</param>
        /// <param name="origin">Origin Positioned Object.</param>
        /// <returns>bool</returns>
        public static bool CirclesIntersect(PositionedObject origin, PositionedObject target)
        {
            if (!origin.Enabled || !target.Enabled)
                return false;

            float distanceX = target.Position.X - origin.Position.X;
            float distanceY = target.Position.Y - origin.Position.Y;
            float radius = origin.Radius + target.Radius;

            if ((distanceX * distanceX) + (distanceY * distanceY) < radius * radius)
                return true;

            return false;
        }
        /// <summary>
        /// Returns Vector3 direction of travel from origin to target. Y is ignored.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <param name="magnitude">float of speed of travel</param>
        /// <returns>Vector3</returns>
        public static Vector3 VelocityFromVectorsY(Vector3 origin, Vector3 target, float magnitude)
        {
            return VelocityFromAngleY(AngleFromVectorsY(origin, target), magnitude);
        }
        /// <summary>
        /// Returns a float of the angle in radians derived from two Vector3 passed into it,
        /// using only the X and Z.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        /// <summary>
        /// Returns Vector3 direction of travel from origin to target. Z is ignored.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <param name="magnitude">float of speed of travel</param>
        /// <returns>Vector3</returns>
        public static Vector3 VelocityFromVectorsZ(Vector3 origin, Vector3 target, float magnitude)
        {
            return VelocityFromAngleZ(AngleFromVectorsZ(origin, target), magnitude);
        }
        /// <summary>
        /// Returns a float of the angle in radians derived from two Vector3 passed into it,
        /// using only the X and Z.
        /// </summary>
        /// <param name="origin">Vector3 of origin</param>
        /// <param name="target">Vector3 of target</param>
        /// <returns>Float</returns>
        public static float AngleFromVectorsY(Vector3 origin, Vector3 target)
        {
            return (MathF.Atan2(-target.Z - -origin.Z, target.X - origin.X));
        }
        /// <summary>
        /// Returns a float of the angle in radians to target, using only the X and Y.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float AngleFromVectorsZ(Vector3 origin, Vector3 target)
        {
            return (MathF.Atan2(target.Y - origin.Y, target.X - origin.X));
        }

        public static float RandomRadian()
        {
            return RandomMinMax(0, MathHelper.TwoPi);
        }

        public static Vector3 RandomVelocity(float speed)
        {
            float ang = RandomRadian();
            float amt = RandomMinMax(speed * 0.15f, speed);
            return VelocityFromAngleZ(ang, amt);
        }

        public static void DebugConsole(string text)
        {
            Debug.WriteLine(text);
        }
#endregion
    }
}
