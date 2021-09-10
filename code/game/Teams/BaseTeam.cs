using Sandbox;

namespace ActionTag.Teams
{
	public abstract partial class BaseTeam
	{
		public int Index { get; internal set; }

		public virtual string Name => "";

		public void Join( Player player )
		{
			OnJoin( player );
		}
		
		public void Leave( Player player )
		{
			OnLeave( player );
		}
		
		public virtual void OnJoin( Player player  ) { }
		public virtual void OnLeave( Player player  ) { }
	}
}
