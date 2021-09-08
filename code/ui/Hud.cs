using Sandbox.UI;

namespace ActionTag
{
	public partial class Hud : Sandbox.HudEntity<RootPanel>
	{
		public Hud()
		{
			if ( IsClient )
			{
				RootPanel.SetTemplate( "/ui/Hud.html" );
			}
		}
	}
}
