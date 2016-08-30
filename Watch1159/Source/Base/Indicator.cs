using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Watch1159
{
	public class Indicator
	{
//		VertexPositionColor[] vertices  = new VertexPositionColor[3];
		VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[3];
		Vector3 orientation;
		Vector3 platVector;
		Vector3 top;
		GraphicsDevice device;


		public Indicator (Vector3 top, Vector3 orientation, Vector3 platVector, GraphicsDevice device)
		{
			this.orientation = Vector3.Normalize(orientation);
			this.platVector = Vector3.Normalize (platVector);
			this.top = top;
			this.device = device;
			PopulateTriangle ();
			UpdateNormal ();
		}

		public void Draw(Effect effect) {
			foreach (EffectPass effectPass in effect.CurrentTechnique.Passes) {
//				effectPass.Apply ();
				device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1, VertexPositionColorNormal.VertexDeclaration);
			}
		}

		public void UpdateNormal() {
			for (int i = 0; i < 3; i++) {
				vertices [i].Normal = Vector3.UnitY;
			}
		}

		public void Inactive() {
			for(int i = 0; i < 3; i++)
				vertices[i].Color = Color.Gray;
		}

		public void Active() {
			for(int i = 0; i < 3; i++)
				vertices[i].Color = Color.Yellow;
		}

		private void PopulateTriangle() {
//				 ^ orientation 
//				 |

//			vertices[0]
//			     /\
//			    /  \
//			   /    \
//			  /______\     ---> platVector
//			[1]      [2]

			vertices [0].Position = top;
			vertices [0].Color = Color.Gray;

			float ver = 0.87f/2;
			float hor = 0.5f/2;

			vertices [1].Position = new Vector3 (top.X - orientation.X *  ver+ platVector.X * hor, top.Y - orientation.Y * ver + platVector.Y * hor, top.Z - orientation.Z * ver + platVector.Z * hor);
			vertices [1].Color = Color.Gray;
			vertices [2].Position = new Vector3 (top.X - orientation.X * ver - platVector.X * hor, top.Y - orientation.Y * ver - platVector.Y * hor, top.Z - orientation.Z * ver - platVector.Z * hor);
			vertices [2].Color = Color.Gray;
		}
	}
}

