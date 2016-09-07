using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Watch1159
{
	public class iCase : PrimitiveC
	{
		public GraphicsDevice device;

		public float Height { get; set; }
		public float OutRadius { get; set; }
		public float InRadius { get; set; }
		public int Segmentation { get; set; }

		public iCase (GraphicsDevice device)
			: this (device, 2, 15, 13, 32)
		{
		}

		public iCase (GraphicsDevice device, float height, float outerR, float innerR, int segmentation)
		{
			this.device = device;
			Height = height;
			OutRadius = outerR;
			InRadius = innerR;
			Segmentation = segmentation;
			color = Color.LightGray;
			defColor = Color.LightGray;
			Construct ();
			SetBoundingBox ();
			InitIndicators ();
		}

		public override void SetBoundingBox() {
			Vector3 topLeft = GetCircleVector(Segmentation/8, Segmentation) * OutRadius * 1.2f + Vector3.Up * Height / 2;
			Vector3 botRight = GetCircleVector(Segmentation*5/8, Segmentation) * OutRadius * 1.2f + Vector3.Down * Height / 2;
			box = new BoundingBox (topLeft, botRight);
			buffers = BoundingBoxBuffers.CreateBoundingBoxBuffers (box, device);
		}

		public override void Construct() {
			float halfHeight = Height / 2;

			for (int i = 0; i < Segmentation; i++) {
				Vector3 normal = GetCircleVector (i, Segmentation);

				// add top and bottom vertex for outer ring
				AddVertex (position: normal * OutRadius + Vector3.Up * halfHeight, color: color, normal: normal);
				AddVertex (position: normal * OutRadius + Vector3.Down * halfHeight, color: color, normal: normal);

				AddIndex (i * 2);
				AddIndex (i * 2 + 1);
				AddIndex ((i * 2 + 2) % (Segmentation * 2));

				AddIndex (i * 2 + 1);
				AddIndex ((i * 2 + 3) % (Segmentation * 2));
				AddIndex ((i * 2 + 2) % (Segmentation * 2));
			}

			for (int i = 0; i < Segmentation; i++) {
				Vector3 normal = GetCircleVector (i, Segmentation);
				Vector3 n_normal = GetInnerCircleVector (i, Segmentation);
				// add top and bottom vertex for outer ring
				AddVertex (position: normal * InRadius + Vector3.Up * halfHeight, color: color, normal: normal);
				AddVertex (position: normal * InRadius + Vector3.Down * halfHeight, color: color, normal: n_normal);

				AddIndex (Segmentation * 2 + i * 2);
				AddIndex (Segmentation * 2 + i * 2 + 1);
				AddIndex (Segmentation * 2 + (i * 2 + 2) % (Segmentation * 2));

				AddIndex (Segmentation * 2 + i * 2 + 1);
				AddIndex (Segmentation * 2 + (i * 2 + 3) % (Segmentation * 2));
				AddIndex (Segmentation * 2 + (i * 2 + 2) % (Segmentation * 2));
			}

			//CreateCap (Segmentation, halfHeight, outerR, Vector3.Up);
			//CreateCap (Segmentation, halfHeight, outerR, Vector3.Down);
			CreateCap();
			InitializePrimitive (device);

		}


		public void UpdateHeight(float scale) {
			Height += scale;
			if (Height <= 2)
				Height = 2;
			if (Height >= 7)
				Height = 7;
			Reset ();
			Construct();
		}

		public void UpdateOuterRadius(float scale) {
			OutRadius += scale;
			if (OutRadius <= 7)
				OutRadius = 7;
			if (OutRadius >= 9)
				OutRadius = 9;
			Reset ();
			Construct ();
		}

		void CreateCap ()
		{
			int tes2 = Segmentation * 2;
			for (int i = 0; i < Segmentation; i++) {
				AddIndex (i * 2);
				AddIndex (tes2 + i * 2);
				AddIndex ((i * 2 + 2) % (Segmentation * 2));

				AddIndex ((i * 2 + 2)  % (Segmentation * 2));
				AddIndex (tes2 + (i * 2) % (Segmentation * 2));
				AddIndex (tes2 + (i * 2 + 2) % (Segmentation * 2));
			}

			for (int i = 0; i < Segmentation; i++) {
				AddIndex (i * 2 + 1);
				AddIndex (tes2 + i * 2 + 1);
				AddIndex ((i * 2 + 3) % (Segmentation * 2));

				AddIndex ((i * 2 + 3)  % (Segmentation * 2));
				AddIndex (tes2 + (i * 2 + 1) % (Segmentation * 2));
				AddIndex (tes2 + (i * 2 + 3) % (Segmentation * 2));
			}

		}

		static Vector3 GetCircleVector (int i, int seg)
		{
			float angle = i * MathHelper.TwoPi / seg;
			float dx = (float)Math.Cos (angle);
			float dz = (float)Math.Sin (angle);
			return new Vector3 (dx, 0, dz);
		}

		static Vector3 GetInnerCircleVector (int i, int seg)
		{
			float angle = i * MathHelper.TwoPi / seg;
			float dx = (float)Math.Cos (angle);
			float dz = (float)Math.Sin (angle);
			return new Vector3 (-dx, 0, -dz);
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

		public override void InitIndicators() {
			float offset = .2f;
			// init view dict
			indicatorView = new Dictionary<string, List<IndicatorGroup>>();

			// side view
			List<IndicatorGroup> indicatorsSide = new List<IndicatorGroup>();

			// Indicators for height
			Indicator ind_1 = new Indicator(new Vector3(-OutRadius - offset, - Height/2, 0), Vector3.UnitY, Vector3.UnitZ, device);
			Indicator ind_2 = new Indicator(new Vector3(-OutRadius - offset, Height/2, 0), -Vector3.UnitY, Vector3.UnitZ, device);
			IndicatorGroup ig_height = new IndicatorGroup ("HEIGHT");
			ig_height.AddToGroup (ind_1);
			ig_height.AddToGroup (ind_2);
			ig_height.Active ();

			indicatorsSide.Add (ig_height);

			indicatorView.Add ("SIDE", indicatorsSide);

			// front view
			List<IndicatorGroup> indicatorsFront = new List<IndicatorGroup>();

			// Indicators for Outer Radius
			Indicator ind_3 = new Indicator(new Vector3((OutRadius+ offset) / 1.4f, Height/2, (OutRadius+ offset) / 1.4f), new Vector3(-1, 0, -1), new Vector3(-1, 0, 1), device);
			Indicator ind_4 = new Indicator(new Vector3((InRadius-offset) / 1.4f, Height/2, (InRadius-offset) / 1.4f), new Vector3(1, 0, 1), new Vector3(1, 0, -1), device);
			IndicatorGroup ig_outradius = new IndicatorGroup ("OUTERRADIUS");
			ig_outradius.AddToGroup (ind_3);
			ig_outradius.AddToGroup (ind_4);
			ig_outradius.Active ();
			indicatorsFront.Add (ig_outradius);

			Indicator ind_5 = new Indicator(new Vector3(0, Height/2, (OutRadius+ offset)), -Vector3.UnitZ, Vector3.UnitX, device);
			Indicator ind_6 = new Indicator(new Vector3(0, Height/2, -(OutRadius+offset)), Vector3.UnitZ, Vector3.UnitX, device);
			IndicatorGroup ig_vert = new IndicatorGroup ("VERTICAL");
			ig_vert.AddToGroup (ind_5);
			ig_vert.AddToGroup (ind_6);
			indicatorsFront.Add (ig_vert);

			Indicator ind_7 = new Indicator(new Vector3((OutRadius+ offset), Height/2, 0), -Vector3.UnitX, Vector3.UnitZ, device);
			Indicator ind_8 = new Indicator(new Vector3(-(OutRadius+offset), Height/2, 0), Vector3.UnitX, Vector3.UnitZ, device);
			IndicatorGroup ig_hori = new IndicatorGroup ("HORIZONTAL");
			ig_hori.AddToGroup (ind_7);
			ig_hori.AddToGroup (ind_8);
			indicatorsFront.Add (ig_hori);

			indicatorView.Add ("FRONT", indicatorsFront);


		}
	}
}
