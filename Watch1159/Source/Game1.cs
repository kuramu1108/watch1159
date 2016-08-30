using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Android.Content.Res;


namespace Watch1159
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		Resources resources;
		BasicEffect effect;
		SpriteBatch spriteBatch;

//		SpriteBatch dials;
//		SpriteFont spriteFont;

		Camera camera;
		Watch watch;
		Quad face;

		enum AppState
		{
			BASIC,
			MENU,
			DESIGN,
		}
			
		cButton btnMenu;
		cButton btnTitle;
		cButton btnDesign;
		cButton btnSave;
		cButton btnLoad;
		cButton btnCart;
		cButton btnSetting;

		List<cButton> buttons = new List<cButton>();

		Rectangle rectangle_menu = new Rectangle(20, 10, 150, 150);
		Rectangle rectangle_title = new Rectangle (20, 161, 500, 70);
		Rectangle rectangle_design = new Rectangle (20, 231, 500, 140);
		Rectangle rectangle_save = new Rectangle(20, 371, 500, 140);
		Rectangle rectangle_load = new Rectangle(20, 511, 500, 140);
		Rectangle rectangle_cart = new Rectangle(20, 651, 500, 140);
		Rectangle rectangle_setting = new Rectangle(20, 791, 500, 140);

		// For watch selection 
		Rectangle rectangle_watch1 = new Rectangle(150, 1560, 300, 340);
		Rectangle rectangle_watch2 = new Rectangle(430, 1560, 280, 340);
		Rectangle rectangle_watch3 = new Rectangle(680, 1560, 300, 360);

		Rectangle rectangle_sampleArea = new Rectangle(0, 1560, 1080, 360);

		AppState CurrentAppState;
		DepthStencilState stencilState = new DepthStencilState();

		public Game1 (Resources resources)
		{
			graphics = new GraphicsDeviceManager (this);
			this.resources = resources;
			graphics.PreferredBackBufferWidth = 480;
			graphics.PreferredBackBufferHeight = 800;
			//graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
			graphics.IsFullScreen = true;
			graphics.ApplyChanges ();
			Content.RootDirectory = "Content";

			// This must be on to add the UI HUD -  10.5.16 Dongyeop
			stencilState.StencilEnable = true;
			stencilState.StencilFunction = CompareFunction.Always;
			stencilState.StencilPass = StencilOperation.Replace;
			stencilState.ReferenceStencil = 1;
			stencilState.DepthBufferEnable = true;
			IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			spriteBatch = new SpriteBatch(GraphicsDevice);
			effect = new BasicEffect (graphics.GraphicsDevice);
			lightSetup ();


			// component init
			camera = new Camera (graphics.GraphicsDevice);
			watch = new Watch (graphics.GraphicsDevice);

			face = new Quad (Vector3.Zero, Vector3.Backward, Vector3.Up, 1, 1, graphics.GraphicsDevice);

			TouchPanel.EnabledGestures = GestureType.Pinch
										| GestureType.HorizontalDrag
										| GestureType.PinchComplete
										| GestureType.Hold
										| GestureType.DoubleTap
										| GestureType.Tap
										| GestureType.VerticalDrag;
			graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
//			rasterizerState = new RasterizerState();
//			rasterizerState.FillMode = FillMode.WireFrame;
//			rasterizerState.CullMode = CullMode.None;
			//rasterizerState.MultiSampleAntiAlias = false;

			CurrentAppState = AppState.DESIGN;

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		/// 
		Texture2D texture;

		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
//			dials = new SpriteBatch(graphics.GraphicsDevice);
//			spriteFont = Content.Load<SpriteFont> ("Courier New");
			texture = Content.Load<Texture2D> ("face");
			effect.Texture = texture;

			// button init
			btnMenu = new cButton(graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D>("icon_menu"), rectangle_menu);
			btnTitle = new cButton (graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D> ("menu_title"), rectangle_title);
			btnDesign = new cButton (graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D> ("menu_design"), rectangle_design);
			btnSave = new cButton(graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D>("menu_save"), rectangle_save);
			btnLoad = new cButton(graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D>("menu_load"), rectangle_load);
			btnCart = new cButton(graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D>("menu_cart"), rectangle_cart);
			btnSetting = new cButton(graphics.GraphicsDevice, spriteBatch, Content.Load<Texture2D>("menu_settings"), rectangle_setting);

			buttons.Add (btnDesign);
			buttons.Add (btnSave);
			buttons.Add (btnLoad);
			buttons.Add (btnCart);
			buttons.Add (btnSetting);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			switch (CurrentAppState) {
			case AppState.BASIC:
				break;
			case AppState.DESIGN:
				HandleTouchInput(gameTime); // add menu trigger later
				break;
			case AppState.MENU:
				HandleMenuInput (gameTime);
				break;
			}
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.White);
			//TODO: Add your drawing code here
			watch.Draw (effect);
			watch.DrawIndicator (effect, camera.View);

			Vector3 textPosition = new Vector3(0, 0, 0);

			effect.View = camera.ViewMatrix;
			effect.Projection = camera.ProjectionMatrix;
			effect.VertexColorEnabled = true;
			effect.TextureEnabled = true;
			effect.Texture = texture;


//			Vector3 screenPosition = graphics.GraphicsDevice.Viewport.Project (textPosition, camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
//			Vector2 dialPosition;
//			dialPosition.X = screenPosition.X;
//			dialPosition.Y = screenPosition.Y;


			// testing !!!!!
//			DrawFace();


//			dials.Begin ();
//			dials.DrawString (spriteFont, "hello, world", dialPosition, Color.Black, 0, spriteFont.MeasureString ("hello, world") / 2, 2, 0, 0);
//			dials.End ();

//			graphics.GraphicsDevice.BlendState = BlendState.Opaque;
//			graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
//			graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
//			graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

			// This is to give Blend Effect and Sort mode. 10.05.16 Dongyeop
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default);

			/* Draw the menu. */
			// Main button.
			btnMenu.Draw();
			switch (CurrentAppState) {
			case AppState.MENU:
				btnTitle.Draw ();
				btnDesign.Draw ();
				btnSave.Draw ();
				btnLoad.Draw ();
				btnCart.Draw ();
				btnSetting.Draw ();

					//btnHelp.Draw(spriteBatch, rectangle_Setting);
					//btnExit.Draw(spriteBatch, rectangle_Exit);
					// display Main screen here.            
					//spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(500, 0, screenWidth, screenHeight), Color.White);  
				/// Another wayto draw a button   
				//menucomponent.Draw();
				break;

			}
			spriteBatch.End();

			// To prevent 3D model rendering corrupted
			graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
			base.Draw (gameTime);
		}

		void DrawFace()
		{
			graphics.GraphicsDevice.SetVertexBuffer (face.vertexbuffer);
			graphics.GraphicsDevice.Indices = face.indexbuffer;
			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply ();

				graphics.GraphicsDevice.DrawIndexedPrimitives (PrimitiveType.TriangleList, 0, 0, 2);
			}
		}

		private void HandleTouchInput(GameTime gameTime) {
			if (TouchPanel.IsGestureAvailable) {
				while (TouchPanel.IsGestureAvailable) {
					var gesture = TouchPanel.ReadGesture ();

					switch (gesture.GestureType) {
					case GestureType.Tap:
						Rectangle touchPoint = new Rectangle((int)gesture.Position.X, (int)gesture.Position.Y, 1, 1);
						if (touchPoint.Intersects(rectangle_menu)) {
							CurrentAppState = AppState.MENU;
						}
						break;
					case GestureType.Hold:
						//watch.SwitchComponent ();
						watch.SwitchComponent (TouchRay (gesture.Position.X, gesture.Position.Y));
						break;
					case GestureType.DoubleTap:
						/* 
							testing
						*/
//						watch.Xml ();
//						WebServiceHandler requestHandler = new WebServiceHandler ();
//						requestHandler.postWithData ("", "");
						//requestHandler.postWithDataFtp();
						Android.Util.Log.Debug ("2TAP", "output xml");
						break;
					case GestureType.Pinch:
						watch.UpdateVertex (gameTime, gesture, camera.View);
						break;
					case GestureType.PinchComplete:
						watch.UpdateBB ();
						break;
					case GestureType.HorizontalDrag:
						camera.Update (gameTime, gesture);
						break;
					}
				}
			} else {
				camera.Update (gameTime);
			}
		}

		private void HandleMenuInput(GameTime gameTime) {
			if (TouchPanel.IsGestureAvailable) {
				while (TouchPanel.IsGestureAvailable) {
					var gesture = TouchPanel.ReadGesture ();

					switch (gesture.GestureType) {
					case GestureType.Tap:
						Rectangle touchPoint = new Rectangle ((int)gesture.Position.X, (int)gesture.Position.Y, 1, 1);
						cButton selected = null;
						if (btnMenu.isPressed (touchPoint)) {
							CurrentAppState = AppState.DESIGN;
						} else if (btnDesign.isPressed (touchPoint)) {
							selected = btnDesign;
						} else if (btnSave.isPressed (touchPoint)) {
							watch.Xml ();
							selected = btnSave;
						} else if (btnLoad.isPressed (touchPoint)) {
							selected = btnLoad;
						} else if (btnCart.isPressed (touchPoint)) {
							selected = btnCart;
						} else if (btnSetting.isPressed (touchPoint)) {
							selected = btnSetting;
						} else
							CurrentAppState = AppState.DESIGN;
						foreach (cButton button in buttons)
							button.TurnOff ();
						if (selected != null)
							selected.TurnOn ();
						break;
					}
				}
			}
		}

		private void lightSetup()
		{
			effect.LightingEnabled = true;

			effect.DirectionalLight0.DiffuseColor = Color.LightGray.ToVector3();
			effect.DirectionalLight0.Direction = Vector3.Normalize( new Vector3 (1, -1, 1));
			effect.DirectionalLight0.SpecularColor = Color.LightGray.ToVector3();
			effect.DirectionalLight0.Enabled = true;

			effect.DirectionalLight1.DiffuseColor = Color.DarkGray.ToVector3();
			effect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3 (-2, -1, 0.5f));
			effect.DirectionalLight1.SpecularColor = Color.DarkGray.ToVector3();
			effect.DirectionalLight1.Enabled = true;

			effect.DirectionalLight2.DiffuseColor = Color.DarkGray.ToVector3();
			effect.DirectionalLight2.Direction = Vector3.Normalize(new Vector3 (1, 2, -0.5f));
			effect.DirectionalLight2.SpecularColor = Color.DarkGray.ToVector3();
			effect.DirectionalLight2.Enabled = true;
		}

		private Ray TouchRay(float x, float y) {
			Vector3 nearSource = new Vector3 (x, y, 0f);
			Vector3 farSource = new Vector3 (x, y, 1f);
			Matrix world = Matrix.CreateTranslation (0, 0, 0);
			Vector3 nearPoint = graphics.GraphicsDevice.Viewport.Unproject (nearSource, camera.ProjectionMatrix, camera.ViewMatrix, world);
			Vector3 farPoint = graphics.GraphicsDevice.Viewport.Unproject (farSource, camera.ProjectionMatrix, camera.ViewMatrix, world);

			Vector3 direction = farPoint - nearPoint;
			direction.Normalize ();
			return new Ray (nearPoint, direction);
		}
	}
}

