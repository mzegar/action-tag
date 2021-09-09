using Sandbox;

namespace ActionTag
{
	public abstract class BaseTeam
	{
		public virtual string Name => "";

		public void Join( ActionTagPlayer player )
		{
			OnJoin( player );
		}

		public void Leave( ActionTagPlayer player )
		{
			OnLeave( player );
		}

		public virtual void OnTick() { }

		public virtual void OnTick( ActionTagPlayer player ) { }

		public virtual void OnLeave( ActionTagPlayer player  ) { }

		public virtual void OnJoin( ActionTagPlayer player  ) { }

		public virtual void OnStart( ActionTagPlayer player ) { }

		public virtual void OnTakeDamageFromPlayer( ActionTagPlayer player, ActionTagPlayer attacker, DamageInfo info ) { }

		public virtual void OnDealDamageToPlayer( ActionTagPlayer player, ActionTagPlayer target, DamageInfo info ) { }

		public virtual void OnPlayerKilled( ActionTagPlayer player ) { }

		public virtual bool PlayPainSounds( ActionTagPlayer player )
		{
			return false;
		}
	}
}
