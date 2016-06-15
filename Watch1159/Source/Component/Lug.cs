﻿using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Watch1159
{
	public class Lug: PrimitiveC
	{
		public GraphicsDevice device;

		public float Height { get; set; }
		public float BottomWidth { get; set; }
		public float TopWidth { get; set; }
		public float HalfSideWidth { get; set; }
		public float SidePosition { get; set; }
		public float StrapWidth { get; set; }
		public float CaseRadius { get; set; }

//		public Color color { get; set; }
//		public Color defColor { get; set; }

		public Vector3 TopRight { get; set; }
		public Vector3 TopLeft { get; set; }
		public Vector3 BottomRight { get; set; }
		public Vector3 BottomLeft { get; set; }

		public Lug (GraphicsDevice device, float height, float bottomWidth, float topWidth, float sideWidth, float sidePosition, float strapWidth, float caseR)
		{
			this.device = device;
			color = Color.DarkGray;
			defColor = Color.DarkGray;

			CaseRadius = caseR;
			Height = height;
			BottomWidth = bottomWidth;
			TopWidth = topWidth;
			HalfSideWidth = sideWidth/2;
			SidePosition = sidePosition;
			StrapWidth = strapWidth;

			TopRight = GetCircleVector (2);
			TopLeft = GetCircleVector (4);
			BottomRight = GetCircleVector (10);
			BottomLeft = GetCircleVector (8);

			/*    Vertex position for lugs
			 * 
			 * <- case side
			 *       1     height     0
			 *       _________________
			 *      /|               /|
			 *     / |    top       / |
			 *    /  | 7         2 /  |  6
			 * 3 |""/"""""""""""""|  /
			 *   | /  side        | /
			 *   |/               |/
			 * 5  """"""""""""""""  4
			 * */

			Construct ();
			SetBoundingBox ();
		}

		public override void Construct() {
			AddTopRightLug ();
			AddTopLeftLug ();
			AddBottomRightLug ();
			AddBottomLeftLug ();


			AddLugIndex (0);
			AddLugIndex (8);
			AddLugIndex (16);
			AddLugIndex (24);

			InitializePrimitive (device);
		}

		public override void SetBoundingBox(){
			Vector3 topLeft = TopLeft * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.UnitZ * Height + Vector3.Left * TopWidth;
			Vector3 botRight = BottomRight * CaseRadius + Vector3.Down * HalfSideWidth - Vector3.UnitZ * Height + Vector3.UnitX * TopWidth;
			box = new BoundingBox (topLeft, botRight);
			buffers = BoundingBoxBuffers.CreateBoundingBoxBuffers (box, device);
		}

		public void UpdateCaseRadius(float scale) {
			CaseRadius += scale;
			if (CaseRadius <= 7)
				CaseRadius = 7;
			if (CaseRadius >= 9)
				CaseRadius = 9;
			Reset ();
			Construct ();
		}

		public void UpdateBottomWidth(float scale) {
			BottomWidth += scale;
			if (BottomWidth <= 1.5f)
				BottomWidth = 1.5f;
			if (BottomWidth >= 2.5f)
				BottomWidth = 2.5f;
			Reset ();
			Construct ();
		}

		public void UpdateTopWidth(float scale) {
			TopWidth += scale;
			if (TopWidth <= 1)
				TopWidth = 1;
			if (TopWidth >= 2.5f)
				TopWidth = 2.5f;
			Reset ();
			Construct ();
		}

		public void UpdateSideWidth(float scale) {
			HalfSideWidth += scale / 2;
			if (HalfSideWidth <= 0.5f)
				HalfSideWidth = 0.5f;
			if (HalfSideWidth >= 1) 
				HalfSideWidth = 1;
			Reset ();
			Construct ();
		}

		static Vector3 GetCircleVector (int i)
		{
			float angle = i * MathHelper.TwoPi / 12;
			float dx = (float)Math.Cos (angle);
			float dz = (float)Math.Sin (angle);
			return new Vector3 (dx, 0, dz);
		}

		void AddTopRightLug()
		{
			// 0
			AddVertex(position: TopRight * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.UnitZ * Height, color: color, normal: Vector3.Up);
			// 1
			AddVertex(position: TopRight * CaseRadius + Vector3.Up * HalfSideWidth , color: color, normal: Vector3.Up);
			// 2
			AddVertex(position: TopRight * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.UnitZ * Height + Vector3.UnitX * TopWidth, color: color, normal: Vector3.Up);
			// 3
			AddVertex(position: TopRight * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.Right * BottomWidth - Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Up);
			// 4
			AddVertex(position: TopRight * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.UnitZ * Height + Vector3.UnitX * TopWidth, color: color, normal: Vector3.Down);
			// 5
			AddVertex(position: TopRight * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.Right * BottomWidth - Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Down);
			// 6
			AddVertex(position: TopRight * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.UnitZ * Height, color: color, normal: Vector3.Down);
			// 7
			AddVertex(position: TopRight * CaseRadius + Vector3.Down * HalfSideWidth, color: color, normal: Vector3.Down);
		}

		void AddTopLeftLug()
		{
			// 0
			AddVertex(position: TopLeft * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.UnitZ * Height + Vector3.Left * TopWidth, color: color, normal: Vector3.Up);
			// 1
			AddVertex(position: TopLeft * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.Left * BottomWidth - Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Up);
			// 2
			AddVertex(position: TopLeft * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.UnitZ * Height, color: color, normal: Vector3.Up);
			// 3
			AddVertex(position: TopLeft * CaseRadius + Vector3.Up * HalfSideWidth , color: color, normal: Vector3.Up);
			// 4
			AddVertex(position: TopLeft * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.UnitZ * Height, color: color, normal: Vector3.Down);
			// 5
			AddVertex(position: TopLeft * CaseRadius + Vector3.Down * HalfSideWidth, color: color, normal: Vector3.Down);
			// 6
			AddVertex(position: TopLeft * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.UnitZ * Height + Vector3.Left * TopWidth, color: color, normal: Vector3.Down);
			// 7
			AddVertex(position: TopLeft * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.Left * BottomWidth - Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Down);
		}

		void AddBottomRightLug()
		{
			// 0
			AddVertex(position: BottomRight * CaseRadius + Vector3.Up * HalfSideWidth , color: color, normal: Vector3.Up);
			// 1
			AddVertex(position: BottomRight * CaseRadius + Vector3.Up * HalfSideWidth - Vector3.UnitZ * Height, color: color, normal: Vector3.Up);
			// 2
			AddVertex(position: BottomRight * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.Right * BottomWidth + Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Up);
			// 3
			AddVertex(position: BottomRight * CaseRadius + Vector3.Up * HalfSideWidth - Vector3.UnitZ * Height + Vector3.UnitX * TopWidth, color: color, normal: Vector3.Up);
			// 4
			AddVertex(position: BottomRight * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.Right * BottomWidth + Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Down);
			// 5
			AddVertex(position: BottomRight * CaseRadius + Vector3.Down * HalfSideWidth - Vector3.UnitZ * Height + Vector3.UnitX * TopWidth, color: color, normal: Vector3.Down);
			// 6
			AddVertex(position: BottomRight * CaseRadius + Vector3.Down * HalfSideWidth, color: color, normal: Vector3.Down);
			// 7
			AddVertex(position: BottomRight * CaseRadius + Vector3.Down * HalfSideWidth - Vector3.UnitZ * Height, color: color, normal: Vector3.Down);
		}

		void AddBottomLeftLug()
		{
			// 0
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Up * HalfSideWidth + Vector3.Left * BottomWidth + Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Up);
			// 1
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Up * HalfSideWidth - Vector3.UnitZ * Height + Vector3.Left * TopWidth, color: color, normal: Vector3.Up);
			// 2
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Up * HalfSideWidth , color: color, normal: Vector3.Up);
			// 3
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Up * HalfSideWidth - Vector3.UnitZ * Height, color: color, normal: Vector3.Up);
			// 4
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Down * HalfSideWidth, color: color, normal: Vector3.Down);
			// 5
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Down * HalfSideWidth - Vector3.UnitZ * Height, color: color, normal: Vector3.Down);
			// 6
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Down * HalfSideWidth + Vector3.Left * BottomWidth + Vector3.UnitZ * BottomWidth, color: color, normal: Vector3.Down);
			// 7
			AddVertex(position: BottomLeft * CaseRadius + Vector3.Down * HalfSideWidth - Vector3.UnitZ * Height + Vector3.Left * TopWidth, color: color, normal: Vector3.Down);
		}



		void AddLugIndex(int current)
		{
			AddIndex (current + 0);
			AddIndex (current + 1);
			AddIndex (current + 2);

			AddIndex (current + 1);
			AddIndex (current + 2);
			AddIndex (current + 3);

			AddIndex (current + 2);
			AddIndex (current + 3);
			AddIndex (current + 4);

			AddIndex (current + 3);
			AddIndex (current + 4);
			AddIndex (current + 5);

			AddIndex (current + 4);
			AddIndex (current + 6);
			AddIndex (current + 7);

			AddIndex (current + 4);
			AddIndex (current + 5);
			AddIndex (current + 7);

			AddIndex (current + 0);
			AddIndex (current + 6);
			AddIndex (current + 7);

			AddIndex (current + 0);
			AddIndex (current + 1);
			AddIndex (current + 7);

			AddIndex (current + 1);
			AddIndex (current + 7);
			AddIndex (current + 3);

			AddIndex (current + 3);
			AddIndex (current + 5);
			AddIndex (current + 7);

			AddIndex (current + 0);
			AddIndex (current + 2);
			AddIndex (current + 6);

			AddIndex (current + 4);
			AddIndex (current + 2);
			AddIndex (current + 6);
		}

		public List<Vector3> TriangleList()
		{
			// list 
			List<Vector3> triangleList = new List<Vector3>();
			for (int i = 0; i < indices.Count; i++) {
				triangleList.Add (vertices [indices [i]].Position);
			}
			return triangleList;
		}
	}
}

