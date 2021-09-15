using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer : Player
	{
		/// <summary>
		/// The clothing container is what dresses the citizen
		/// </summary>
		private readonly Clothing.Container _clothing = new();

		
		public bool IsSpectator { get => Camera is not ActionTagFirstPersonCamera; }
		public int HorizontalSpeed { get => (int)Velocity.WithZ(0).Length; }
		
		public new ActionTagWalkController Controller
		{
			get => (ActionTagWalkController) base.Controller;
			private set => base.Controller = value;
		}
		
		/// <summary>
		/// Default init
		/// </summary>
		public ActionTagPlayer()
		{
			Inventory = new BaseInventory( this );
		}

		/// <summary>
		/// Initialize using this client
		/// </summary>
		public ActionTagPlayer( Client cl ) : this()
		{
			_clothing.LoadFromClient( cl );
		}

		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );
			
			Controller = new ActionTagWalkController();
			Animator = new StandardPlayerAnimator();
			Camera = new ActionTagFirstPersonCamera();
			
			_clothing.DressEntity( this );

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

		public override void TakeDamage( DamageInfo info )
		{
			// No damage allowed outside of the playground.
			if ( ActionTagGame.Instance?.Round is not PlayRound )
			{
				return;
			}

			// Only damage people that are not on your team.
			if ( info.Attacker is ActionTagPlayer player && player.Team.Name == Team.Name )
			{
				return;
			}
			
			base.TakeDamage( info );
		}
	}
}
