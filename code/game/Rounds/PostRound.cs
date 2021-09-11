namespace ActionTag
{
	public class PostRound : BaseRound
	{
		public override string RoundName => "Post";
		public override int RoundDuration
		{
			get => ActionTagGameSettings.PostRoundDuration;
		}

		protected override void OnTimeUp()
		{
			base.OnTimeUp();

			ActionTagGame.Instance?.ChangeRound(new PreRound());
		}
	}
}
