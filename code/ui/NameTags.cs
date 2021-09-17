using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class BaseNameTag : Panel
	{
		public Label TeamLabel;
		ActionTagPlayer player;

		public BaseNameTag( ActionTagPlayer player )
		{
			StyleSheet.Load("/ui/NameTags.scss");
			this.player = player;
			TeamLabel = Add.Label( $"" );
		}

		public virtual void UpdateFromPlayer( ActionTagPlayer player )
		{
			TeamLabel.Text = player.Controller.IsFrozen ? $"❄️{player.Team?.Emoji}" : $"{player.Team?.Emoji}";
		}
	}

	public class NameTags : Panel
	{
		Dictionary<ActionTagPlayer, BaseNameTag> ActiveTags = new Dictionary<ActionTagPlayer, BaseNameTag>();

		public NameTags()
		{
			StyleSheet.Load( "/ui/nametags/NameTags.scss" );
		}

		public override void Tick()
		{
			base.Tick();


			var deleteList = new List<ActionTagPlayer>();
			deleteList.AddRange( ActiveTags.Keys );

			int count = 0;
			foreach ( var player in Entity.All.OfType<ActionTagPlayer>().OrderBy( x => Vector3.DistanceBetween( x.Position, CurrentView.Position ) ) )
			{
				if ( UpdateNameTag( player ) )
				{
					deleteList.Remove( player );
					count++;
				}
			}

			foreach( var player in deleteList )
			{
				ActiveTags[player].Delete();
				ActiveTags.Remove( player );
			}

		}

		public virtual BaseNameTag CreateNameTag( ActionTagPlayer player )
		{
			if ( player.GetClientOwner() == null )
				return null;

			var tag = new BaseNameTag( player );
			tag.Parent = this;
			return tag;
		}

		public bool UpdateNameTag( ActionTagPlayer player )
		{
			// Don't draw local player
			if ( player == Local.Pawn )
				return false;

			if ( player.LifeState != LifeState.Alive )
				return false;

			//
			// Where we putting the label, in world coords
			//
			var head = player.GetAttachment( "hat" ) ?? new Transform( player.EyePos );

			var labelPos = head.Position + head.Rotation.Up * 5;

			float dist = labelPos.Distance( CurrentView.Position );

			//
			// Are we looking in this direction?
			//
			var lookDir = (labelPos - CurrentView.Position).Normal;
			if ( CurrentView.Rotation.Forward.Dot( lookDir ) < 0.5 )
				return false;

			// If I understood this I'd make it proper function
			var objectSize = 0.05f / dist / (2.0f * MathF.Tan( (CurrentView.FieldOfView / 2.0f).DegreeToRadian() )) * 1500.0f;

			objectSize = objectSize.Clamp( 0.05f, 1.0f );

			if ( !ActiveTags.TryGetValue( player, out var tag ) )
			{
				tag = CreateNameTag( player );
				if ( tag != null )
				{
					ActiveTags[player] = tag;
				}
			}

			if ( tag == null )
				return false;

			tag.UpdateFromPlayer( player );

			var screenPos = labelPos.ToScreen();

			tag.Style.Left = Length.Fraction( screenPos.x );
			tag.Style.Top = Length.Fraction( screenPos.y );

			var transform = new PanelTransform();
			transform.AddTranslateY( Length.Fraction( -1.1f ) );
			transform.AddScale( objectSize );
			transform.AddTranslateX( Length.Fraction( -0.49f ) );

			tag.Style.Transform = transform;
			tag.Style.Dirty();

			return true;
		}
	}
}
