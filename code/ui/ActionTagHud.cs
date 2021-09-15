using ActionTag.UI;
using Sandbox;
using Sandbox.UI;

namespace ActionTag
{
	public partial class ActionTagHud : Sandbox.HudEntity<RootPanel>
	{
		public ActionTagHud()
		{
			if ( !IsClient )
			{
				return;
			}

			RootPanel.StyleSheet.Load( "/ui/ActionTagHud.scss" );
			RootPanel.AddChild<ActionTagStaticHud>();
			RootPanel.AddChild<ActionTagAliveHud>();
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
		/// This Hud always shows.
		/// </summary>
		public class ActionTagStaticHud : Panel
		{
			public ActionTagStaticHud()
			{
				AddChild<PlayerInfo>();
				AddChild<RoundInfo>();
				AddChild<ChatBox>();
				AddChild<VoiceList>();
				AddChild<PlayerVoice>();
			}
		}

		/// <summary>
		/// This Hud only shows if the player is alive.
		/// </summary>
		public class ActionTagAliveHud : Panel
		{
			public ActionTagAliveHud()
			{
				AddChild<SpeedInfo>();
				AddChild<Crosshair>();
			}

			public override void Tick()
			{
				base.Tick();

				Enabled = Local.Pawn is ActionTagPlayer {IsSpectator: false};
			}
		}
	}
}
