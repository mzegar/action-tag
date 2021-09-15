using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer : Sandbox.Player
	{
		public bool IsSpectator { get => Camera is not ActionTagFirstPersonCamera; }
		public int HorizontalSpeed { get => (int)Velocity.WithZ(0).Length; }
		
		public new ActionTagWalkController Controller
		{
			get => (ActionTagWalkController) base.Controller;
			private set => base.Controller = value;
		}

		public ActionTagPlayer()
		{
			Inventory = new BaseInventory( this );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			
			Controller = new ActionTagWalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ActionTagFirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

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
		
		
	}
}
