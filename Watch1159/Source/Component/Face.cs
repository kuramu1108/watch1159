using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Watch1159
{
	public class Face : PrimitiveC
	{
		public GraphicsDevice device;

		public float Height { get; set; }
		public float CaseHeight { get; set; }
		public float OuterRadius { get; set; }
		public int Segmentation { get; set; }
//		public Color color { get; set;}
//		public Color defColor { get; set; }

		public Face (GraphicsDevice device)
			: this (device, 0.5f, 2, 15, 32)
		{
		}

		public Face (GraphicsDevice device, float height, float caseheight, float outerR, int tessellation)
		{
			this.device = device;
			Height = height;
			CaseHeight = caseheight;
			OuterRadius = outerR;
			Segmentation = tessellation;
			color = Color.LightGray;
			defColor = Color.LightGray;
			Construct ();
			SetBoundingBox();
		}

		public override void SetBoundingBox() {
			Vector3 topLeft = GetCircleVector(Segmentation/8, Segmentation) * OuterRadius * 1.2f + Vector3.Down * CaseHeight / 2;
			Vector3 botRight = GetCircleVector(Segmentation*5/8, Segmentation) * OuterRadius * 1.2f + Vector3.Down * (Height + CaseHeight /2 );
			box = new BoundingBox (topLeft, botRight);
			buffers = BoundingBoxBuffers.CreateBoundingBoxBuffers (box, device);
		}

		void CreateCap (int tessellation, float radius, Color color, Vector3 yPosition, Vector3 normal)
		{
			// create cap indices.
			for (int i = 0; i < tessellation - 2; i++) {
				if (normal.Y > 0) {
					AddIndex (CurrentVertex);
					AddIndex (CurrentVertex + (i + 1) % tessellation);
					AddIndex (CurrentVertex + (i + 2) % tessellation);
				}
				else {
					AddIndex (CurrentVertex);
					AddIndex (CurrentVertex + (i + 2) % tessellation);
					AddIndex (CurrentVertex + (i + 1) % tessellation);
				}
			}

			// create cap vertices.
			for (int i = 0; i < tessellation; i++) {
				Vector3 position = GetCircleVector (i, tessellation) * radius +
					yPosition;

				AddVertex (position, color, normal);
			}
		}

		static Vector3 GetCircleVector (int i, int tessellation)
		{
			float angle = i * MathHelper.TwoPi / tessellation;
			float dx = (float)Math.Cos (angle);
			float dz = (float)Math.Sin (angle);
			return new Vector3 (dx, 0, dz);
		}

		public override void Construct() {

			float halfCaseHeight = CaseHeight / 2;

			Vector3 topYVector = Vector3.Down * halfCaseHeight;
			Vector3 bottomYVector = Vector3.Down * Height + Vector3.Down * halfCaseHeight;

			for (int i = 0; i < Segmentation; i++) {
				Vector3 normal = GetCircleVector (i, Segmentation);

				// add top and bottom vertex for outer ring
				AddVertex (position: normal * OuterRadius + topYVector, color: color, normal: normal);
				AddVertex (position: normal * OuterRadius + bottomYVector, color: color, normal: normal);

				AddIndex (i * 2);
				AddIndex (i * 2 + 1);
				AddIndex ((i * 2 + 2) % (Segmentation * 2));

				AddIndex (i * 2 + 1);
				AddIndex ((i * 2 + 3) % (Segmentation * 2));
				AddIndex ((i * 2 + 2) % (Segmentation * 2));
			}

			CreateCap (Segmentation, OuterRadius, color, topYVector, Vector3.Up);
			CreateCap (Segmentation, OuterRadius, color, bottomYVector, Vector3.Down);

			InitializePrimitive (device);
		}

		public void UpdateCaseHeight(float scale) {
			CaseHeight += scale;
			if (CaseHeight <= 2)
				CaseHeight = 2;
			if (CaseHeight >= 7)
				CaseHeight = 7;
			Reset ();
			Construct();
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
