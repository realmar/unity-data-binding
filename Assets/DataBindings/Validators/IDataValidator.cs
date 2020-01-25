using System;

namespace Realmar.DataBindings.Validators
{
	public interface IDataValidator<in TData, out TTResult>
	{
		TTResult Validate(TData data);
	}

	public enum ValidationStatus
	{
		Ok = 0,
		Failed = 1
	}

	public struct ValidationResult
	{
		public ValidationStatus Status;
		public string Message;
	}

	public interface IConfigurableValidator<in TData, out TResult, TConfiguration> : IDataValidator<TData, TResult>
	{
		TConfiguration Configuration { get; set; }
	}

	public struct ValidationConfiguration
	{
		public event Action<ValidationResult> OnValidationFailed;
		public event Action<ValidationResult> OnValidationSucceeded;
	}

	public interface IBindingValidator<in TData> : IConfigurableValidator<TData, ValidationResult, ValidationConfiguration>
	{
	}

	public class ValidatorManager<TData>
	{
		private IBindingValidator<TData>[] Validators;
	}

	public interface IVali<TData, TResult>
	{
		Func<TData, TResult> Validator { get; }
	}
}
