all:
	msbuild Binding/Xamarin.Android.ArchitectureComponents.BuildTasks/Xamarin.Android.ArchitectureComponents.BuildTasks.csproj || exit 1
	msbuild Binding/xamarin-android-architecture-components-binding.sln || exit 1
