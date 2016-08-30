using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Watch1159
{
	public class IndicatorGroup
	{
		private List<Indicator> indicators;
		String Target { get; set; }

		public IndicatorGroup (String target)
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
			foreach (Indicator indicator in indicators)
				indicator.Inactive ();
		}

		public void Active() {
			foreach (Indicator indicator in indicators)
				indicator.Active ();
		}
	}
}

