/*
 * The original Primitive class I found online
 * Class Primitive has been modified according to our requirement
 * using VertexPositionColorNormal created
 * 									by Po-Hao
 * 
 *             */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Watch1159
{
	public abstract class PrimitiveC : IDisposable
	{
		protected List<VertexPositionColorNormal> vertices = new List<VertexPositionColorNormal>();
		protected List<ushort> indices = new List<ushort>();

		public VertexBuffer vertexBuffer { get; set;}
		public IndexBuffer indexBuffer { get; set;}
		// added
		public Color color { get; set; }
		public Color defColor { get; set; }
		public BoundingBox box { get; set; }
		public BoundingBoxBuffers buffers {get; set;}
		public IndicatorGroup currentIndicatorGroup { get; set; }

		protected Dictionary<string, List<IndicatorGroup> > indicatorView;

		protected void AddVertex (Vector3 position, Color color, Vector3 normal)
		{
			vertices.Add (new VertexPositionColorNormal (position, color, normal));
		}

		protected void AddIndex (int index)
		{
			if (index > ushort.MaxValue) {
				throw new ArgumentOutOfRangeException ("index");
			}

			indices.Add ((ushort)index);
		}

		protected int CurrentVertex { get { return vertices.Count; } }

		protected void InitializePrimitive (GraphicsDevice device)
		{
			vertexBuffer = new VertexBuffer (device, typeof (VertexPositionColorNormal), vertices.Count, BufferUsage.None);
			vertexBuffer.SetData (vertices.ToArray ());
			indexBuffer = new IndexBuffer (device, typeof (ushort), indices.Count, BufferUsage.None);
			indexBuffer.SetData (indices.ToArray ());
		}

		~PrimitiveC ()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposing) {
				if (vertexBuffer != null) {
					vertexBuffer.Dispose ();
				}
				if (indexBuffer != null) {
					indexBuffer.Dispose ();
				}
			}
		}

		public void Draw (Effect effect)
		{
			
			GraphicsDevice device = effect.GraphicsDevice;
			device.SetVertexBuffer (vertexBuffer);
			device.Indices = indexBuffer;

			foreach (EffectPass effectPass in effect.CurrentTechnique.Passes) {
				effectPass.Apply ();
				int primitiveCount = indices.Count / 3;
				device.DrawIndexedPrimitives (PrimitiveType.TriangleList, 0, 0, primitiveCount);
			}
		}
		// added
		public void Drawloop (Effect effect, GraphicsDevice device) {

			int primitiveCount = indices.Count / 3;
			device.DrawIndexedPrimitives (PrimitiveType.TriangleList, 0, 0, primitiveCount);
		}

		// added by Po
		public void Reset()
		{
			vertices = new List<VertexPositionColorNormal> ();
			indices = new List<ushort> ();
			vertexBuffer.Dispose ();
			indexBuffer.Dispose ();
		}

		public abstract void Construct ();

		// bounding box testing =============================================
		public abstract void SetBoundingBox();

		public void DrawBoundingBox (Effect effect)
		{
			GraphicsDevice device = effect.GraphicsDevice;

			device.SetVertexBuffer (buffers.Vertices);
			device.Indices = buffers.Indices;

			foreach (EffectPass effectPass in effect.CurrentTechnique.Passes) {
				effectPass.Apply ();
				device.DrawIndexedPrimitives (PrimitiveType.LineList, 0, 0, buffers.PrimitiveCount);
			}
		}
		// bounding box testing =============================================

		// indicator drawing
		public void DrawIndicator(Effect effect, string view) {
			foreach (IndicatorGroup inds in indicatorView[view]) {
				inds.Draw (effect);
			}
		}

		public virtual void InitIndicators() {
		}

		public void IndicatorIntersect(Ray ray, String view) {
			IndicatorGroup result = null;
			float? closestIntersection = float.MaxValue;
			foreach (IndicatorGroup group in indicatorView[view]) {
				var intersectionResult = group.Intersects (ray);
				if (intersectionResult != null && intersectionResult < closestIntersection) {
					closestIntersection = intersectionResult;
					result = group;
				}
			}
			if (result != null) {
				foreach (var group in indicatorView[view]) {
					if (group.active) {
						group.Inactive ();
					}
				}
				result.Active ();
			}
		}

		public String GetActiveIndicatorTarget(String view) {
			foreach (IndicatorGroup group in indicatorView[view]) {
				if (group.active) {
					return group.Target;
				}
			}
			return null;
		}
	}
}
