**UPDATE**: we don't need this MSBuild post processing task anymore. Google's AAC works just with annotations, and Xamarin component team offers AAC bindings with additional attributes. Write code like `[Lifecycle.Event.OnResume][Export]` instead of `[OnLifecycleEvent(Event.OnResume)]` and you are all set!

This is a proof-of-concept binding and post-processing MSBuild targets for
Android Architecture Component libraries for Xamarin.Android.

If you are using Xamarin.Android before 92984e6 (which is, as of Sep. 2017,
only in xamarin-android master, no packaged product), you need some
additional [Register] attributes which makes almost everything useless.

So far, have a look at [Binding/ArchitectureComponentsSample/MainActivity.cs](Binding/ArchitectureComponentsSample/MainActivity.cs) to see how users code would look like.
Disregard the commented code - it is to show what the post-processing
MSBuild task generates.

The sample .csproj imports `name.atsushieno.architecturecomponents.targets`
which is going to be the NuGet package name for this proof-of-concept library.

The custom build task depends on some non-public build artifacts in
Xamarin.Android.Common.targets that basically "we" (outsiders) cannot
depend on for future compatibility. But anyhow here is what the task does:

- Generate C# source code after we get the actual app dll.
- Call csc to compile it into another dll.
- Add the resulting dll into the resulting apk.

The weird binding assembly names are due to my [binding automator tool](https://github.com/atsushieno/xamarin-android-binding-automator).

Last but not least, current post-processing task deals with Lifecycle only.
