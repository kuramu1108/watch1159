using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content.Res;

namespace Watch1159
{
	public class Watch
	{

		public iCase case_ { get; set; }
		public Lug lug { get; set; }
		public Bezel bezel { get; set; }
		public Face bottom { get; set; }

		public PrimitiveC[] components = new PrimitiveC[4];
		// current selected component
		public PrimitiveC selected { get; set; }
		private int selectedIndex = 0;
		private Color selColor = Color.Aqua;

		public GraphicsDevice device;
		Resources resources;


		// case dimensions
		public float caseHeight { get; set; }
		public float caseOutRadius { get; set; }
		public float caseInRadius { get; set; }
		public int caseSegmentation { get; set; }

		// bezel dimensions
		public float bezelHeight {get; set;}
		public float bezelOutRadius { get; set; }
		public float bezelInRadius { get; set; }
		public int bezelSegmentation { get; set; }

		// face dimensions
		public float bottomHeight { get; set; }
		public float bottomRadius { get; set; }
		public int bottomSegmentation { get; set; }

		// lug dimensions
		public float lugHeight { get; set; }
		public float lugBotWid { get; set; }
		public float lugTopWid { get; set; }
		public float lugSidWid { get; set; }
		public float lugSidPos { get; set; }

		// strap dimensions
		public float strapWid { get; set; }

		public Watch (GraphicsDevice device)
		{
			this.device = device;
			caseHeight = 2;
			caseOutRadius = 7.5f;
			caseInRadius = 6.5f;
			caseSegmentation = 32;

			bezelHeight = 1.5f;
			bezelOutRadius = 7;
			bezelInRadius = 6.5f;
			bezelSegmentation = 32;

			bottomHeight = 0.5f;
			bottomRadius = 7.5f;
			bottomSegmentation = 32;

			lugHeight = 2.5f;
			lugBotWid = 1.5f;
			lugTopWid = 1;
			lugSidWid = 1;
			lugSidPos = 0;

			strapWid = 5;
			CreateComponents ();
		}

		public Watch (GraphicsDevice device, Resources resources,String template)
		{
			this.device = device;
			this.resources = resources;
			switch (template) {
			case "swalovski":
//				caseHeight = float.Parse (resources.GetString (Resource.String.caseHeight));
//				caseOutRadius = float.Parse (resources.GetString (Resource.String.caseOutRadius));
//				caseInRadius = float.Parse (resources.GetString (Resource.String.caseInRadius));
//				caseSegmentation = int.Parse (resources.GetString (Resource.String.caseSegmentation));
//
//				bezelHeight = float.Parse (resources.GetString (Resource.String.bezelHeight));
//				bezelOutRadius = float.Parse (resources.GetString (Resource.String.bezelOutRadius));
//				bezelInRadius = float.Parse (resources.GetString (Resource.String.bezelInRadius));
//				bezelSegmentation = int.Parse (resources.GetString (Resource.String.bezelSegmentation));
//
//				bottomHeight = float.Parse (resources.GetString (Resource.String.bottomHeight));
//				bottomRadius = float.Parse (resources.GetString (Resource.String.bottomRadius));
//				bottomSegmentation = int.Parse (resources.GetString (Resource.String.bottomSegmentation));
//
//				lugHeight = float.Parse (resources.GetString (Resource.String.lugHeight));
//				lugBotWid = float.Parse (resources.GetString (Resource.String.lugBotWid));
//				lugTopWid = float.Parse (resources.GetString (Resource.String.lugTopWid));
//				lugSidWid = float.Parse (resources.GetString (Resource.String.lugSidWid));
//				lugSidPos = float.Parse (resources.GetString (Resource.String.lugSidPos));

				strapWid = 5;

				break;
			}
			CreateComponents ();
		}

		public void CreateComponents() {
			case_ = new iCase (device, caseHeight, caseOutRadius, caseInRadius, caseSegmentation);
			bezel = new Bezel (device, caseHeight, bezelHeight, bezelOutRadius, bezelInRadius, bezelSegmentation);
			bottom = new Face (device, bottomHeight, caseHeight, bottomRadius, bottomSegmentation);
			lug = new Lug (device, lugHeight, lugBotWid, lugTopWid, lugSidWid, lugSidPos, strapWid, caseOutRadius);
			selected = case_;

			selected.color = selColor;
			selected.Reset ();
			selected.Construct ();

			components [0] = case_;
			components [1] = bezel;
			components [2] = bottom;
			components [3] = lug;
		}

		public void Draw(Effect effect) {
			case_.Draw (effect);
			lug.Draw (effect);
			bezel.Draw (effect);
			bottom.Draw (effect);

			// bounding box debug methods
//			case_.DrawBoundingBox (effect);
//			lug.DrawBoundingBox (effect);
//			bezel.DrawBoundingBox (effect);
//			bottom.DrawBoundingBox (effect);
		}

		public void DrawIndicator(Effect effect, string view) {
			selected.DrawIndicator (effect, view);
		}

		// vertex update method
		public void UpdateVertex(GameTime gameTime, GestureSample gesture, String view) {
			float scale = Scale (gesture);
			if (scale != 0) { 
				if (selected.Equals (case_)) { // update case
					if (view.Equals ("FRONT")) {
						String target = case_.GetActiveIndicatorTarget (view);
						switch (target) {
						case "OUTERRADIUS":
							case_.UpdateOuterRadius (scale);
							lug.UpdateCaseRadius (scale);
							break;
						case "VERTICAL":
							break;
						case "HORIZONTAL":
							break;
						default:
							break;
						}
					} else if (view.Equals ("SIDE")) {
						case_.UpdateHeight (scale);
						bezel.UpdateCaseHeight (scale);
						bottom.UpdateCaseHeight (scale);
					}
				} else if (selected.Equals (bezel)) { // update bezel
					if (view.Equals ("FRONT")) {
						String target = case_.GetActiveIndicatorTarget (view);
						switch (target) {
						case "OUTERRADIUS":
							bezel.UpdateOuterRadius (scale);
							break;
						case "VERTICAL":
							break;
						case "HORIZONTAL":
							break;
						default:
							break;
						}
					} else if (view.Equals ("SIDE")) {
						bezel.UpdateHeight (scale);
					}
				} else if (selected.Equals (lug)) { // update lug
					if (view.Equals ("FRONT")) {
//						float HalfScreenHeight = device.Viewport.Height / 2;
//						if (gesture.Position.Y <= HalfScreenHeight && gesture.Position2.Y <= HalfScreenHeight) {
//							lug.UpdateTopWidth (scale);
//						} else {
//							lug.UpdateBottomWidth (scale);
//						}
						String target = lug.GetActiveIndicatorTarget (view);
						switch (target) {
						case "TOPWIDTH":
							lug.UpdateTopWidth (scale);
							break;
						case "BOTTOMWIDTH":
							lug.UpdateBottomWidth (scale);
							break;
						default:
							break;
						}
					} else if (view.Equals ("SIDE")) {
						String target = lug.GetActiveIndicatorTarget (view);
						switch (target) {
						case "HEIGHT":
							lug.UpdateHeight (scale);
							break;
						case "SIDEWIDTH":
							lug.UpdateSideWidth (scale);
							break;
						default:
							break;
						}
					}
				} else if (selected.Equals (bottom)) { // update bottom
					if (view.Equals ("FRONT")) {
					}
				}
			}
		}

		// new vertex update method after implementing the indicator
		public void UpdateVertex(GestureSample gesture, String view) {
			float scale = Scale (gesture);
			if (scale != 0) {
				
			}
		}

		// boundingbox update method
		public void UpdateBB() {
			if (selected.Equals (case_)) { // update case
				case_.SetBoundingBox();
				bezel.SetBoundingBox ();
				lug.SetBoundingBox ();
				bottom.SetBoundingBox ();
			} else if (selected.Equals (bezel)) { // update bezel
				bezel.SetBoundingBox();
			} else if (selected.Equals (lug)) { // update lug
				lug.SetBoundingBox();
			} else if (selected.Equals (bottom)) { // update bottom
				bottom.SetBoundingBox();
			}
		}

		private float Scale(GestureSample gesture) {
				// current position
				Vector2 a = gesture.Position;
				Vector2 b = gesture.Position2;
				float dist = Vector2.Distance (a, b);

				// previous position
				Vector2 aOld = gesture.Position - gesture.Delta;
				Vector2 bOld = gesture.Position2 - gesture.Delta2;
				float distOld = Vector2.Distance (aOld, bOld);

				float scaleRate = 0.05f;
				float scale = (dist - distOld) * scaleRate / 100;
				// operate function called here 
				return scale;

		}

		public void Xml() {
			List<List<Vector3>> triangleList = new List<List<Vector3>> ();
			triangleList.Add (case_.TriangleList());
			triangleList.Add (lug.TriangleList());
			triangleList.Add (bezel.TriangleList());
			triangleList.Add (bottom.TriangleList());
			XMLwriter.WriteXML ("trilist.xml", triangleList);
		}

		public void SwitchComponent(Ray ray) {
			PrimitiveC result = null;
			float? closestIntersection = float.MaxValue;
			foreach (var primitive in components) {
				var intersectionResult = ray.Intersects (primitive.box);
				if (intersectionResult != null && intersectionResult < closestIntersection) {
					closestIntersection = intersectionResult;
					result = primitive;
				}
			}

			if (result != null) {
				selected.color = selected.defColor;
				selected.Reset ();
				selected.Construct ();
				selected = result;
				selected.color = selColor;
				selected.Reset ();
				selected.Construct ();
			}
		}

		public void SwitchIndicator(Ray ray, String view) {
			selected.IndicatorIntersect (ray, view);
		}

		public void UpdateIndicator(String view) {
			selected.InitIndicators ();
			if (selected.Equals (case_)) { // update case
				if (view.Equals ("FRONT")) {
					lug.InitIndicators ();
				} else if (view.Equals ("SIDE")) {
					bezel.InitIndicators ();
				}
			}
		}
	}
}

