using NUnit.Framework;
using Realmar.DataBindings.Editor.TestFramework.BaseTests;

namespace Realmar.DataBindings.Editor.Tests
{
	[TestFixture]
	internal class Positive_Weaver_UnityTests : WeaverTest
	{
		[Test]
		public void NonVirtual_OneWay_MonoBehaviorToPOCO() => AssetNoThrow();

		[Test]
		public void NonVirtual_OneWay_MonoBehaviorToMonoBehavior() => AssetNoThrow();

		[Test]
		public void NonVirtual_TwoWay_MonoBehaviorToMonoBehavior() => AssetNoThrow();

		[Test]
		public void Virtual_TwoWay_OverrideToOverride_BothMonoBehaviors() => AssetNoThrow();
	}
}
