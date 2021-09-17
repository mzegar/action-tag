using System;
using ActionTag.Teams;
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
			
			GlowDistanceStart = 70;
			GlowDistanceEnd = int.MaxValue;
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

			if ( controller is not ActionTagWalkController actionTagController )
			{
				return;
			}

			GlowActive = Team is ChasersTeam || actionTagController.IsFrozen;
			if ( Team is ChasersTeam )
			{
				GlowColor = Color.Red;
				return;
			}

			if ( actionTagController.IsFrozen )
			{
				GlowColor = Color.Cyan;
				return;
			}
		}
		
		public void OnFrozen()
		{
			ActionTagGame.Instance?.OnTagged( this );
		}

	}
}
