﻿using PrimitiveBuddy;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Vector2Extensions;

namespace CollisionBuddy
{
	/// <summary>
	/// A line class... it has a start and end point, and all the properties that entails.
	/// </summary>
	public class Line : ILine
	{
		#region Properties

		protected Vector2 _start = Vector2.Zero;

		/// <summary>
		/// update or get the start point... 
		/// If you are going to update both the start and end points at the same time, use the Set() method!!!
		/// </summary>
		public Vector2 Start
		{
			get
			{
				return _start;
			}
			set
			{
				OldStart = _start;
				_start = value;
				Updated(); //set the length and direction
			}
		}

		protected Vector2 _end = Vector2.Zero;

		/// <summary>
		/// update or get the start point... 
		/// If you are going to update both the start and end points at the same time, use the Set() method!!!
		/// </summary>
		public Vector2 End
		{
			get
			{
				return _end;
			}
			set
			{
				OldEnd = _end;
				_end = value;
				Updated(); //set the length and direction
			}
		}

		/// <summary>
		/// The last position of the start point
		/// </summary>
		public Vector2 OldStart { get; protected set; }

		/// <summary>
		/// The last position of the end point
		/// </summary>
		public Vector2 OldEnd { get; protected set; }

		/// <summary>
		/// the length of the line, world units
		/// </summary>
		public float Length { get; protected set; }

		protected Vector2 _direction = Vector2.Zero;

		/// <summary>
		/// unit vector, the direction of the line
		/// </summary>
		public Vector2 Direction 
		{
			get
			{
				return _direction;
			}
			protected set
			{
				//this better be a unit vector!
				_direction = value;
			}
		}

		/// <summary>
		/// unit vector perpendicular to the line
		/// </summary>
		public Vector2 Normal { get; protected set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Line()
		{
			OldStart = Vector2.Zero;
			OldEnd = Vector2.Zero;
			Length = 0.0f;
			Direction = Vector2.Zero;
			Normal = Vector2.Zero;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="start">start point of the line</param>
		/// <param name="end">end point of the line</param>
		public Line(Vector2 start, Vector2 end)
		{
			Initialize(start, end);
		}

		/// <summary>
		/// Set all the values of this line
		/// </summary>
		/// <param name="start">the start point</param>
		/// <param name="end">the end point</param>
		public void Initialize(Vector2 start, Vector2 end)
		{
			OldStart = start;
			_start = start;
			OldEnd = end;
			_end = end;
			Updated();
		}

		/// <summary>
		/// When the end points get updated, the length and direction need to be updated as well
		/// </summary>
		private void Updated()
		{
			//set the length and direction
			Direction = End - Start;
			Length = Direction.Length();
			_direction.Normalize();
			CalculateNormal();
		}

		/// <summary>
		/// Calculate the unit vector perpendicular to the line.
		/// </summary>
		private void CalculateNormal()
		{
			Normal = Direction.Perp();
		}

		/// <summary>
		/// Get the center point of the line
		/// </summary>
		/// <returns></returns>
		public Vector2 Center()
		{
			return (Start + End) / 2.0f;
		}

		/// <summary>
		/// Given a rectangle, create a list of lines with the normals pointing in
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static List<ILine> InsideRect(Rectangle rect)
		{
			var lines = new List<ILine>();

			//create the points we need
			Vector2 upperLeft = new Vector2(rect.Left, rect.Top);
			Vector2 upperRight = new Vector2(rect.Right, rect.Top);
			Vector2 lowerLeft = new Vector2(rect.Left, rect.Bottom);
			Vector2 lowerRight = new Vector2(rect.Right, rect.Bottom);

			//create the lines
			lines.Add(new Line(upperLeft, upperRight));
			lines.Add(new Line(upperRight, lowerRight));
			lines.Add(new Line(lowerRight, lowerLeft));
			lines.Add(new Line(lowerLeft, upperLeft));

			return lines;
		}

		/// <summary>
		/// Given a rectangle, create a list of lines with the normals pointing in
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static List<ILine> ExtendedInsideRect(Rectangle rect, float extend)
		{
			var lines = new List<ILine>();

			//create the points we need
			Vector2 upperLeft = new Vector2(rect.Left, rect.Top);
			Vector2 upperRight = new Vector2(rect.Right, rect.Top);
			Vector2 lowerLeft = new Vector2(rect.Left, rect.Bottom);
			Vector2 lowerRight = new Vector2(rect.Right, rect.Bottom);

			//create the lines
			lines.Add(new Line(new Vector2(upperLeft.X - extend, upperLeft.Y),
				new Vector2(upperRight.X + extend, upperRight.Y)));

			lines.Add(new Line(new Vector2(upperRight.X, upperRight.Y - extend),
				new Vector2(lowerRight.X, lowerRight.Y + extend)));

			lines.Add(new Line(new Vector2(lowerRight.X + extend, lowerRight.Y),
				new Vector2(lowerLeft.X - extend, lowerLeft.Y)));

			lines.Add(new Line(new Vector2(lowerLeft.X, lowerLeft.Y + extend),
				new Vector2(upperLeft.X, upperLeft.Y - extend)));

			return lines;
		}

		/// <summary>
		/// Given a rectangle, create a list of lines with the normals pointing out
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public static List<ILine> OutsideRect(Rectangle rect)
		{
			var lines = new List<ILine>();

			//create the points we need
			Vector2 upperLeft = new Vector2(rect.Left, rect.Top);
			Vector2 upperRight = new Vector2(rect.Right, rect.Top);
			Vector2 lowerLeft = new Vector2(rect.Left, rect.Bottom);
			Vector2 lowerRight = new Vector2(rect.Right, rect.Bottom);

			//create the lines
			lines.Add(new Line(upperLeft, lowerLeft));
			lines.Add(new Line(lowerLeft, lowerRight));
			lines.Add(new Line(lowerRight, upperRight));
			lines.Add(new Line(upperRight, upperLeft));

			return lines;
		}

		/// <summary>
		/// draw the line with normal
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public void Draw(IPrimitive prim, Color color)
		{
			//draw the line
			prim.Line(Start, End, color);

			//draw the normal
			Vector2 normalStart = Center();
			Vector2 normalEnd = Center() + (Normal * 10.0f);
			prim.Line(normalStart, normalEnd, color);
		}

		#endregion //Methods
	}
}
