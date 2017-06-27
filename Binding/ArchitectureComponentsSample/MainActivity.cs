using Android.App;
using Android.Widget;
using Android.OS;
using Android.Arch.Lifecycles;
using System;
using Android.Content;
using Java.Lang;

namespace ArchitectureComponentsSample
{
	[Activity (Label = "ArchitectureComponentsSample", MainLauncher = true)]
	// FIXME: this requiring [Register] attribute it really stupid and Xamarin.Android team should immediately stop package name mangling into MD5
	// or fix PackageNamingPolicy related bug that it is disregarded at all.
	[global::Android.Runtime.Register ("architecturecomponentssample.MainActivity")]
	public class MainActivity : LifecycleActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			var button = FindViewById<Button>(Resource.Id.button1);
			button.Click += (sender, e) => button.Text = this.Lifecycle.CurrentState.Name();
			Lifecycle.AddObserver (new TestObserver ());
		}
	}

	// FIXME: this should not be public. [InternalsVisibleTo] is generated for the extensibility dll
	// within this app before build, but it does not seem to work yet.
	// FIXME: this requiring [Register] attribute it really stupid and Xamarin.Android team should immediately stop package name mangling into MD5
	// or fix PackageNamingPolicy related bug that it is disregarded at all.
	[global::Android.Runtime.Register ("architecturecomponentssample.TestObserver")]
	public class TestObserver : Java.Lang.Object, ILifecycleObserver
	{
		[OnLifecycleEvent (OnLifecycleEvent.OnAny)]
		public void OnAny (ILifecycleOwner owner, Lifecycle.Event evt)
		{
			Console.WriteLine ("!!!!! OnAny invoked.");
		}

		[OnLifecycleEvent (OnLifecycleEvent.OnStop)]
		public void OnStopped ()
		{
			Console.WriteLine ("!!!!! OnStopped invoked.");
		}

		[OnLifecycleEvent (OnLifecycleEvent.OnStart)]
		public void OnStarted ()
		{
			Console.WriteLine ("!!!!! OnStarted invoked.");
		}

		[OnLifecycleEvent (OnLifecycleEvent.OnStart | OnLifecycleEvent.OnStop)]
		public void OnStartedStopped ()
		{
			Console.WriteLine ("!!!!! OnStartedStopped invoked.");
		}
	}

	/*
	
	// Then our code generator should generate the following code:
		
	public class TestObserver_LifecycleAdapter : Java.Lang.Object, IGenericLifecycleObserver
	{
		readonly TestObserver mReceiver;

		public TestObserver_LifecycleAdapter (TestObserver receiver)
		{
			this.mReceiver = receiver;
		}

		public Java.Lang.Object Receiver => mReceiver;

		public void OnStateChanged(ILifecycleOwner owner, Lifecycle.Event evt)
		{
			mReceiver.OnAny(owner, evt);

			if (evt == Lifecycle.Event.OnStart)
			{
				mReceiver.OnStarted();
			}
			if (evt == Lifecycle.Event.OnStart)
			{
				mReceiver.OnStartedStopped ();
			}
			if (evt == Lifecycle.Event.OnStop)
			{
				mReceiver.OnStartedStopped ();
			}
			if (evt == Lifecycle.Event.OnStop)
			{
				mReceiver.OnStopped ();
			}
		}
	}
	*/
}
