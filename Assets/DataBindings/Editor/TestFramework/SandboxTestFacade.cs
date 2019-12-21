using System;
using System.Collections.Generic;
using static Realmar.DataBindings.Editor.YeetHelpers;

namespace Realmar.DataBindings.Editor.TestFramework
{
	internal class SandboxTestFacade : IDisposable
	{
		private readonly UUTSandboxFactory _uutSandboxFactory = new UUTSandboxFactory();
		private readonly WeaverTestFacade _weaverTestFacade = new WeaverTestFacade();
		private readonly Dictionary<Type, IUnitUnderTestSandbox> _sandboxes = new Dictionary<Type, IUnitUnderTestSandbox>();

		public void Dispose()
		{
			_sandboxes.Clear();
			_weaverTestFacade?.Dispose();
			_uutSandboxFactory?.Dispose();
		}

		internal IUnitUnderTestSandbox GetSandboxForTest(Type testType, string testName)
		{
			YeetIfNull(testType, nameof(testType));
			YeetIfNull(testName, nameof(testName));

			if (_sandboxes.TryGetValue(testType, out var sandbox) == false)
			{
				var weavedPath = _weaverTestFacade.CompileAndWeave(testType);
				sandbox = AllocateNewSandbox(weavedPath);
				_sandboxes[testType] = sandbox;
			}

			ConfigureSandboxForTest(sandbox, testType, testName);

			return sandbox;
		}

		private IUnitUnderTestSandbox AllocateNewSandbox(string assemblyPath)
		{
			YeetIfNull(assemblyPath, nameof(assemblyPath));

			var sandbox = _uutSandboxFactory.CreateSandbox();
			sandbox.InitializeSandbox(assemblyPath);

			return sandbox;
		}

		private void ConfigureSandboxForTest(IUnitUnderTestSandbox sandbox, Type testType, string testName)
		{
			sandbox.ChangeNamespace(_weaverTestFacade.GetNamespaceForTest(testType, testName));
		}
	}
}
