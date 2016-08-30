/*
 * Vertex structure taking position color and normal
 * 									by Po-Hao
 *             */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Watch1159
{
	public struct VertexPositionColorNormal: IVertexType
	{
		public Vector3 Position;
		public Color Color;
		public Vector3 Normal;

		public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal)
		{
			Position = position;
			Color = color;
			Normal = normal;
		}

		public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration 
		(
			new VertexElement (0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement (sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
			new VertexElement (sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
		);

		public static readonly int SizeInBytes = sizeof(float) * (3 + 1 + 3);

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}
	}
}

