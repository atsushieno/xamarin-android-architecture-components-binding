using System;
namespace Android.Arch.Lifecycles
{
	// I hoped to have this as an enum type, but then our post-processing MSBuild tasks
	// cannot deserialize the object value from the attributes on the methods in the app assembly
	// (which is under Xamarin.Android framework, unlike the desktop framework for the MSBuild tasks).
	public static class OnLifecycleEvent
	{
		public const int OnCreate = 1,
		OnStart = 2,
		OnPause = 4,
		OnResume = 8,
		OnStop = 16,
		OnDestroy = 32,
		OnAny = 63;
	}
}
