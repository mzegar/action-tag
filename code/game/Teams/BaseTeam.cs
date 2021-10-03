using Sandbox;

namespace ActionTag.Teams
{
	public abstract partial class BaseTeam : BaseNetworkable
	{
		public virtual string ScoreboardName => "";
		public virtual string Name => "";
		public virtual string Emoji => "";
		public virtual int Index => 0;

		public virtual void OnJoin( ActionTagPlayer player )
		{
			Log.Info($"${player.Client.Name} joined {Name}");
		}
		
		public virtual void OnLeave( ActionTagPlayer player )
		{
			Log.Info($"${player.Client.Name} left {Name}");
		}
	}
}
