This is a proof-of-concept binding and post-processing MSBuild targets for
Android Architecture Component libraries for Xamarin.Android.

Right now there are couple of stupid limitations that I'm not sure if
they are going to be fixed. I hope to have fixes but there are
controvertial objections to fix issues in xamarin-android. We need to
persuade them to do The Right Thing(tm).

Right now there is no working nuget package yet, but there will be
hopefully soon.

So far, have a look at [ArchitectureComponentsSample/MainActivity.cs](ArchitectureComponentsSample/MainActivity.cs) to see how users code would look like.
Disregard the commented code - it is to show what the post-processing
MSBuild task generates.

The sample .csproj imports `name.atsushieno.architecturecomponents.targets`
which is going to be the NuGet package name for this proof-of-concept library.

The custom build task depends on some non-public build artifacts in
Xamarin.Android.Common.targets that basically "we" (outsiders) cannot
depend on for future compatibility. But anyhow here is what the task does:

- Generate C# source code after we get the actual app dll.
- Call csc to compile it into another dll.
- Add the resulting dll to the following build steps.

The weird binding assembly names are due to my [binding automator tool](https://github.com/atsushieno/xamarin-android-binding-automator).

Last but not least, current post-processing task deals with Lifecycle only.
