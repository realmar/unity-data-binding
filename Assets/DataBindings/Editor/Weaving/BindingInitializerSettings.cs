namespace Realmar.DataBindings.Editor.Weaving
{
	internal readonly struct BindingInitializerSettings
	{
		public bool ThrowOnFailure { get; }

		public BindingInitializerSettings(bool throwOnFailure)
		{
			ThrowOnFailure = throwOnFailure;
		}
	}
}
