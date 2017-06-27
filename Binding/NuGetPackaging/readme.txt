
This is a Xamarin.Android binding library for Android Architecture Components.
https://developer.android.com/topic/libraries/architecture/index.html

Right now only Lifecycle annotations are supported.

Here is an example:
		
	class TestObserver : Java.Lang.Object, ILifecycleObserver
	{
		[OnLifecycleEvent (OnLifecycleEvent.OnAny)]
		public void OnAny (ILifecycleOwner owner, Lifecycle.Event evt)
		{
			Console.WriteLine ("OnAny invoked.");
		}
		
		[OnLifecycleEvent (OnLifecycleEvent.OnStop)]
		public void OnStopped ()
		{
			Console.WriteLine ("OnStopped invoked.");
		}
		
		[OnLifecycleEvent (OnLifecycleEvent.OnStart)]
		public void OnStarted ()
		{
			Console.WriteLine ("OnStarted invoked.");
		}
		
		[OnLifecycleEvent (OnLifecycleEvent.OnStart | OnLifecycleEvent.OnStop)]
		public void OnStartedStopped ()
		{
			Console.WriteLine ("OnStartedStopped invoked.");
		}
	}
		
	[Activity (Label = "ArchitectureComponentsSample", MainLauncher = true)]
	public class MainActivity : LifecycleActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			var button = FindViewById<Button> (Resource.Id.button1);
			button.Click += (sender, e) => button.Text = this.Lifecycle.CurrentState.Name ();
			Lifecycle.AddObserver (new TestObserver ());
		}
	}
		
