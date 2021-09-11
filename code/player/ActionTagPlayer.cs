using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer : Sandbox.Player
	{
		public bool IsSpectator { get => Camera is not ActionTagFirstPersonCamera; }
		public int HorizontalSpeed { get => (int)Velocity.WithZ(0).Length; }

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			
			Controller = new ActionTagWalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ActionTagFirstPersonCamera();

			Team = ActionTagGame.Instance.NoneTeam;

			EnableAllCollisions = false;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;
			EnableTouch = true;
			EnableTouchPersists = true;

			RemoveRagdollEntity();

			ActionTagGame.Instance?.Round?.OnPlayerSpawn( this );
			base.Respawn();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );
			SimulateActiveChild( cl, ActiveChild );
		}
		
		public override void Touch(Entity other)
		{
			base.StartTouch(other);
			if ( Team is not ChasersTeam ) return;
			if ( other is not ActionTagPlayer otherPlayer || otherPlayer.Team is not RunnerTeam ) return;
			if ( ActionTagGame.Instance?.Round is not PlayRound ) return;
			other.TakeDamage( DamageInfo.Generic( 1000 ) );
		}
	}
}
