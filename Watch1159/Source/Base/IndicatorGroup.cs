using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Watch1159
{
	public class IndicatorGroup
	{
		public bool active = false;

		private List<Indicator> indicators;
		public Dimension Target { get; set; }

		public IndicatorGroup (Dimension target)
		{
			indicators = new List<Indicator> ();
			this.Target = target;
		}

		public void AddToGroup(Indicator indicator) {
			indicators.Add (indicator);
		}

		public void Draw(Effect effect) {
			foreach (Indicator indicator in indicators) {
				indicator.Draw (effect);
			}
		}

		public void Inactive() {
			active = false;
			foreach (Indicator indicator in indicators)
				indicator.Inactive ();
		}

		public void Active() {
			active = true;
			foreach (Indicator indicator in indicators)
				indicator.Active ();
		}

		public float? Intersects(Ray ray) {
			float? closestIntersection = float.MaxValue;
			foreach (var indicator in indicators) {
				var intersectionResult = ray.Intersects (indicator.sphere);
				if (intersectionResult != null && intersectionResult < closestIntersection) {
					closestIntersection = intersectionResult;
				}
			}
			return closestIntersection;
		}
	}
}

