using System;
namespace Android.Arch.Lifecycles
{
	public partial class Lifecycle
	{
		public partial class Event
		{
			public static implicit operator Event (int e)
			{
				switch (e)
				{
				case OnLifecycleEvent.OnCreate:
					return Event.OnCreate;
				case OnLifecycleEvent.OnStart:
					return Event.OnStart;
				case OnLifecycleEvent.OnPause:
					return Event.OnPause;
				case OnLifecycleEvent.OnResume:
					return Event.OnResume;
				case OnLifecycleEvent.OnStop:
					return Event.OnStop;
				case OnLifecycleEvent.OnDestroy:
					return Event.OnDestroy;
				}
				return Event.OnAny;
			}
		}
	}

	public partial class OnLifecycleEventAttribute
	{
		public int Target { get; set; } = OnLifecycleEvent.OnAny;

		public OnLifecycleEventAttribute (int target)
		{
			Target = target;
		}
	}
}
