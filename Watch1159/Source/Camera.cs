using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Watch1159
{
	public class Camera
	{
		GraphicsDevice graphicsDevice;

		Vector3 position = new Vector3 (0, 40, 0);

		float angle;
		public String View { get; set; }

		public Matrix ViewMatrix {
			get {
				var lookAtVector = Vector3.Zero;

				//var rotationMatrix = Matrix.CreateRotationZ (angle);
				//lookAtVector = Vector3.Transform (lookAtVector, rotationMatrix);
				//lookAtVector += position;

				var upVector = Vector3.UnitZ;

				position = Vector3.Transform(position - lookAtVector, Matrix.CreateFromAxisAngle(upVector, angle)) + lookAtVector;
				return Matrix.CreateLookAt (position, lookAtVector, upVector);
			}
		}

		public Matrix ProjectionMatrix {
			get {
				float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
				float nearClipPlane = 1;
				float farClipPlane = 200;
				float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

				return Matrix.CreatePerspectiveFieldOfView (fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
			}
		}

		public Camera (GraphicsDevice graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
			View = "FRONT";
		}

		public void Update(GameTime gameTime, GestureSample gesture)
		{
			//TouchCollection touchCollection = TouchPanel.GetState();

//			bool isTouchingScreen = touchCollection.Count > 0;
//			if (isTouchingScreen) {
//				
//				var xPosition = touchCollection [0].Position.X;
//
//				float xRatio = xPosition / (float)graphicsDevice.Viewport.Width;
//
//				if (xRatio < 1 / 2.0f) {
//					angle = (float)gameTime.ElapsedGameTime.TotalSeconds;
//				} else {
//					angle = -(float)gameTime.ElapsedGameTime.TotalSeconds;
//				}
//			} else {
//				angle = 0;
//			}
//				

//			if (gesture.Delta.X < 0)
//				angle = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
//			else
//				angle = -(float)gameTime.ElapsedGameTime.TotalSeconds * 4;
			angle = -(float)gameTime.ElapsedGameTime.TotalSeconds * gesture.Delta.X/4;

			//Android.Util.Log.Debug ("Posotioin", "X: " + position.X.ToString() + "Y: " + position.Y.ToString());
			 
			if (position.Y < 20 && position.Y> -20) {
				View = "SIDE";
				//Android.Util.Log.Debug ("CAMERA", View);
			} else {
				View = "FRONT";
				//Android.Util.Log.Debug ("CAMERA", View);
			}
		}

		public void Update(GameTime gameTime) {
			angle = 0;
		}
	}
}

