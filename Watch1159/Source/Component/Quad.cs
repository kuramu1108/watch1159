using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Watch1159
{
	public class Quad
	{
		public VertexBuffer vertexbuffer;
		public IndexBuffer indexbuffer;
		public VertexPositionTexture[] Vertices;
		public Vector3 Origin;
		public Vector3 Up;
		public Vector3 Normal;
		public Vector3 Left;
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
		public int[] Indexes;
		public GraphicsDevice device;

		public Quad(Vector3 origin, Vector3 normal, Vector3 up,
			float width, float height, GraphicsDevice device)
		{
			this.Vertices = new VertexPositionTexture[4];
			this.Indexes = new int[6];
			this.Origin = origin;
			this.Normal = normal;
			this.Up = up;
			this.device = device;

			// Calculate the quad corners
			this.Left = Vector3.Cross(normal, this.Up);
			Vector3 uppercenter = (this.Up * height / 2) + origin;
			this.UpperLeft = uppercenter + (this.Left * width / 2);
			this.UpperRight = uppercenter - (this.Left * width / 2);
			this.LowerLeft = this.UpperLeft - (this.Up * height);
			this.LowerRight = this.UpperRight - (this.Up * height);

			this.FillVertices();
		}

		private void FillVertices()
		{
			Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
			Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
			Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
			Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

//			for (int i = 0; i < this.Vertices.Length; i++)
//			{
//				this.Vertices[i].Normal = this.Normal;
//			}

			this.Vertices[0].Position = this.LowerLeft;
			this.Vertices[0].TextureCoordinate = textureLowerLeft;
			this.Vertices[1].Position = this.UpperLeft;
			this.Vertices[1].TextureCoordinate = textureUpperLeft;
			this.Vertices[2].Position = this.LowerRight;
			this.Vertices[2].TextureCoordinate = textureLowerRight;
			this.Vertices[3].Position = this.UpperRight;
			this.Vertices[3].TextureCoordinate = textureUpperRight;

			this.Indexes[0] = 0;
			this.Indexes[1] = 1;
			this.Indexes[2] = 2;
			this.Indexes[3] = 2;
			this.Indexes[4] = 1;
			this.Indexes[5] = 3;
			vertexbuffer = new VertexBuffer (device, typeof (VertexPositionTexture), Vertices.Length, BufferUsage.None);
			indexbuffer = new IndexBuffer (device, typeof (int), Indexes.Length, BufferUsage.None);
			vertexbuffer.SetData (Vertices);
			indexbuffer.SetData (Indexes);
		}
	}

}

