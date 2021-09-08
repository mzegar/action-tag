using Sandbox;
using Sandbox.UI;

namespace ActionTag.UI
{
	public partial class ActionTagHud : Sandbox.HudEntity<RootPanel>
	{
		public static ActionTagHud Instance { set; get; }
		public ActionTagStaticHud StaticHud;

		public ActionTagHud()
		{
			if ( !IsClient )
			{
				return;
			}

			Instance = this;
			StaticHud = new ActionTagStaticHud(RootPanel);
		}
		
		[Event.Hotload]
		public static void OnHotReloaded()
		{
			if ( !Host.IsClient )
			{
				return;
			}

			Local.Hud?.Delete();
			_ = new ActionTagHud();
		}

		/// <summary>
		/// This Hud always exists.
		/// </summary>
		public class ActionTagStaticHud : Panel
		{
			public ActionTagStaticHud( Sandbox.UI.Panel parent )
			{
				Parent = parent;

				Parent.AddChild<RoundStatus>();
			}
		}
	}
}
