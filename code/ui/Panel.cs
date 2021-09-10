namespace ActionTag
{
	public class Panel : Sandbox.UI.Panel
	{
		private bool _isEnabled = true;
		public bool Enabled
		{
			get => _isEnabled;
			set
			{
				if ( value == _isEnabled )
				{
					return;
				}

				_isEnabled = value;
				SetClass("disabled", !_isEnabled);
			}
		}

		public Panel(Sandbox.UI.Panel parent = null) : base(parent)
		{
			Parent = parent ?? Parent;

			StyleSheet.Load("/ui/Panel.scss");

			SetClass("panel", true);
		}
	}
}
