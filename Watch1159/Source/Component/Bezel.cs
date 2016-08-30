using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Watch1159
{
	public class Bezel : PrimitiveC
	{
		public GraphicsDevice device;

		public float CaseHeight { get; set; }
		public float Height { get; set; }
		public float OuterRadius { get; set; }
		public float InnerRadius { get; set; }
		public int Segmentation { get; set; }
//		public Color color { get; set; }
//		public Color defColor { get; set; }

		public Bezel (GraphicsDevice device)
			: this (device, 2, 1.5f, 14, 12, 32)
		{
		}

		public Bezel (GraphicsDevice device, float caseH, float height, float outerR, float innerR, int tessellation)
		{
			this.device = device;
			CaseHeight = caseH;
			Height = height;
			OuterRadius = outerR;
			InnerRadius = innerR;
			Segmentation = tessellation;
			color = Color.LightGray;
			defColor = Color.LightGray;
			Construct ();
			SetBoundingBox ();
			InitIndicators ();
		}

		public override void SetBoundingBox() {
			Vector3 topLeft = GetCircleVector(Segmentation/8, Segmentation) * OuterRadius * 1.2f + Vector3.Up * (Height + CaseHeight/2) ;
			Vector3 botRight = GetCircleVector(Segmentation*5/8, Segmentation) * OuterRadius * 1.2f + Vector3.Up * CaseHeight/2;
			box = new BoundingBox (topLeft, botRight);
			buffers = BoundingBoxBuffers.CreateBoundingBoxBuffers (box, device);
		}

		static Vector3 GetCircleVector (int i, int tessellation)
		{
			float angle = i * MathHelper.TwoPi / tessellation;
			float dx = (float)Math.Cos (angle);
			float dz = (float)Math.Sin (angle);
			return new Vector3 (dx, 0, dz);
		}
		
		static Vector3 GetInnerCircleVector (int i, int tessellation)
		{
			float angle = i * MathHelper.TwoPi / tessellation;
			float dx = (float)Math.Cos (angle);
			float dz = (float)Math.Sin (angle);
			return new Vector3 (-dx, 0, -dz);
		}

		public override void Construct() {
			if (Segmentation < 3) {
				throw new ArgumentOutOfRangeException ("Bezel Segmentation");
			}

			for (int i = 0; i < Segmentation; i++) {
				Vector3 normal = GetCircleVector (i, Segmentation);
				Vector3 innerNormal = GetInnerCircleVector (i, Segmentation);
//				float k = (OuterRadius - InnerRadius) / 5;
//				float r = OuterRadius;
//				// create vertex on the curve side
//				for (float j = 0; j < Height; j+=(Height/5), r-=k) { 
//
//					AddVertex (position: normal * r + Vector3.Up * (halfCase + j), color: color, normal: normal);
//				}
//				// create vertex on the inner side
//				AddVertex (position: normal * InnerRadius + Vector3.Up * halfCase, color: color, normal: innerNormal);
//				// i = 0
//				/* add triangle on th left side
//				 * j
//				 * 0  1  2  3  4  5 
//				 * ____________
//				 * | /| /| /| /
//				 * |/ |/ |/ |/
//				 * 6  7  8  9  10 11
//				*/
//				for (int j = i * 6; j < 5 + i * 6; j++) {
//					AddIndex (j);
//					AddIndex (j + 1);
//					AddIndex ((j + 6) % (Segmentation * 6));
//
//					AddIndex (j + 1);
//					AddIndex ((j + 7) % (Segmentation * 6));
//					AddIndex ((j + 6) % (Segmentation * 6));
//				}
//				// i = 0
//				/* add triangle on th right side
//				 * j
//				 * 0  1  2  3  4  5 
//				 *   /| /| /| /|
//				*  / |/ |/ |/ |
//				*  """"""""""""
//				* 6  7  8  9  10 11
//				*/

				AddVertex (position: normal * OuterRadius + Vector3.Up * CaseHeight / 2, color: color, normal: innerNormal);
				AddVertex (position: normal * InnerRadius + Vector3.Up * (CaseHeight / 2 + Height), color: color, normal: innerNormal);
				AddVertex (position: normal * InnerRadius + Vector3.Up * CaseHeight / 2, color: color, normal: innerNormal);
				for (int j = i * 3; j < 2 + i * 3; j++) {
					AddIndex (j);
					AddIndex (j + 1);
					AddIndex ((j + 3) % (Segmentation * 3));

					AddIndex (j + 1);
					AddIndex ((j + 4) % (Segmentation * 3));
					AddIndex ((j + 3) % (Segmentation * 3));
				}
			}

			// bottom cap not created yet

			InitializePrimitive (device);
		}

		public void UpdateCaseHeight (float scale) {
			CaseHeight += scale;
			if (CaseHeight <= 2)
				CaseHeight = 2;
			if (CaseHeight >= 7)
				CaseHeight = 7;
			Reset ();
			Construct();
		}

		public void UpdateHeight(float scale) {
			Height += scale;
			if (Height <= 0.1f)
				Height = 0.1f;
			if (Height >= 2)
				Height = 2;
			Reset ();
			Construct ();
		}

		public void UpdateOuterRadius(float scale) {
			OuterRadius += scale;
			if (OuterRadius <= 6.5f)
				OuterRadius = 6.5f;
			if (OuterRadius >= 8)
				OuterRadius = 8;
			Reset ();
			Construct ();
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

		private void InitIndicators() {
			float offset = .2f;

			// side view
			List<IndicatorGroup> indicatorsSide = new List<IndicatorGroup>();

			// Indicators for height
			Indicator ind_1 = new Indicator(new Vector3(-OuterRadius - offset * 10, CaseHeight/2, 0), Vector3.UnitY, Vector3.UnitZ, device);
			Indicator ind_2 = new Indicator(new Vector3(-OuterRadius - offset * 10, CaseHeight/2+Height, 0), -Vector3.UnitY, Vector3.UnitZ, device);
			IndicatorGroup ig_height = new IndicatorGroup ("HEIGHT");
			ig_height.AddToGroup (ind_1);
			ig_height.AddToGroup (ind_2);
			ig_height.Active ();
			indicatorsSide.Add (ig_height);

			indicatorView.Add ("SIDE", indicatorsSide);

			// front view
			List<IndicatorGroup> indicatorsFront = new List<IndicatorGroup>();

			// Indicators for Outer Radius
			Indicator ind_3 = new Indicator(new Vector3((OuterRadius+ offset) / 1.4f, CaseHeight/2 + offset, (OuterRadius+ offset) / 1.4f), new Vector3(-1, 0, -1), new Vector3(-1, 0, 1), device);
			Indicator ind_4 = new Indicator(new Vector3((InnerRadius-offset) / 1.4f, CaseHeight/2 + offset, (InnerRadius-offset) / 1.4f), new Vector3(1, 0, 1), new Vector3(1, 0, -1), device);
			IndicatorGroup ig_outradius = new IndicatorGroup ("OUTERRADIUS");
			ig_outradius.AddToGroup (ind_3);
			ig_outradius.AddToGroup (ind_4);
			ig_outradius.Active ();
			indicatorsFront.Add (ig_outradius);

			Indicator ind_5 = new Indicator(new Vector3(0, CaseHeight/2 + offset, (OuterRadius+ offset)), -Vector3.UnitZ, Vector3.UnitX, device);
			Indicator ind_6 = new Indicator(new Vector3(0, CaseHeight/2 + offset, -(OuterRadius+offset)), Vector3.UnitZ, Vector3.UnitX, device);
			IndicatorGroup ig_vert = new IndicatorGroup ("VERT");
			ig_vert.AddToGroup (ind_5);
			ig_vert.AddToGroup (ind_6);
			indicatorsFront.Add (ig_vert);

			Indicator ind_7 = new Indicator(new Vector3((OuterRadius+ offset), CaseHeight/2 + offset, 0), -Vector3.UnitX, Vector3.UnitZ, device);
			Indicator ind_8 = new Indicator(new Vector3(-(OuterRadius+offset), CaseHeight/2 + offset, 0), Vector3.UnitX, Vector3.UnitZ, device);
			IndicatorGroup ig_hori = new IndicatorGroup ("HORI");
			ig_hori.AddToGroup (ind_7);
			ig_hori.AddToGroup (ind_8);
			indicatorsFront.Add (ig_hori);

			indicatorView.Add ("FRONT", indicatorsFront);


		}
	}
}
