using Sandbox;
using Sandbox.UI;

namespace ActionTag
{
	public partial class ActionTagHud : Sandbox.HudEntity<RootPanel>
	{
		public static ActionTagHud Instance { set; get; }
		public ActionTagStaticHud StaticHud;
		public ActionTagAliveHud AliveHud;

		public ActionTagHud()
		{
			if ( !IsClient )
			{
				return;
			}

			Instance = this;
			StaticHud = new ActionTagStaticHud( RootPanel );
			AliveHud = new ActionTagAliveHud( RootPanel );
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

				Parent.AddChild<RoundInfo>();
				Parent.AddChild<ChatBox>();
				Parent.AddChild<SpeedInfo>();
			}
		}

		/// <summary>
		/// This Hud is only active when the player is alive.
		/// </summary>
		public class ActionTagAliveHud : Panel
		{
			public ActionTagAliveHud( Sandbox.UI.Panel parent )
			{
				Parent = parent;
				
				Parent.AddChild<SpeedInfo>();
			}
		}
	}
}
