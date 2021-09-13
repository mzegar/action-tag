using Sandbox;

namespace ActionTag.Teams
{
	public abstract partial class BaseTeam : NetworkComponent
	{
		public virtual string Name => "";

		public virtual void OnJoin( ActionTagPlayer player )
		{
			Log.Info($"${player.GetClientOwner().Name} joined {Name}");
		}
		
		public virtual void OnLeave( ActionTagPlayer player )
		{
			
			Log.Info($"${player.GetClientOwner().Name} left {Name}");
		}
	}
}
