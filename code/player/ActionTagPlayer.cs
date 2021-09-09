﻿using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer : Sandbox.Player
	{
		public int HorizontalSpeed { get => (int)Velocity.WithZ(0).Length; }

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

			base.Respawn();
		}
		
		public override void OnKilled()
		{
			base.OnKilled();
			
			BecomeRagdollOnServer( _lastDamageInfo.Force, GetHitboxBone( _lastDamageInfo.HitboxIndex ) );

			MakeSpectator();
		}

		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			
			SimulateActiveChild( cl, ActiveChild );
		}
	}
}
