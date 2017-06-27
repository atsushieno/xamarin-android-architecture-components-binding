using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Android.Arch.Lifecycles;

namespace Android.ArchitectureComponents.BuildTasks
{
	public class LifecycleCodeGenerator
	{
		string [] assemblies;

		const string CommonAssemblyName = "android.arch.lifecycle_common_1.0.0-alpha1";

		const string lifecycle_ns = "Android.Arch.Lifecycles";

		public string GenerateCode (IEnumerable<string> assemblies)
		{
			this.assemblies = assemblies.ToArray ();

			var types = GetTargetTypes ();

			var writer = new StringWriter ();

			Action<int,int, string,string> write = (evt, iv, jle, mName) => {
				if ((evt & iv) != 0)
					writer.WriteLine (@"
			if (evt == global::Android.Arch.Lifecycles.Lifecycle.Event.{0})
				mReceiver.{1} ();
			", jle, mName);
			};
			
			foreach (var t in types) {
				writer.WriteLine (@"namespace {2}
{{
	[global::Android.Runtime.Register (""{3}{4}{5}_LifecycleAdapter"")]
	class {0}_LifecycleAdapter  : global::Java.Lang.Object, global::Android.Arch.Lifecycles.IGenericLifecycleObserver
	{{
		readonly global::{1} mReceiver;
	
		public {0}_LifecycleAdapter (global::{1} receiver)
		{{
			this.mReceiver = receiver;
		}}
	
		public global::Java.Lang.Object Receiver => mReceiver;
	
		public void OnStateChanged(global::Android.Arch.Lifecycles.ILifecycleOwner owner, global::Android.Arch.Lifecycles.Lifecycle.Event evt)
		{{
", t.Name, t.FullName, t.Namespace, t.Namespace.ToLowerInvariant (), t.Namespace.Length > 0 ? "." : "", t.Name);
				foreach (var method in t.Methods.Where (m => m.CustomAttributes.Any (ca => ca.AttributeType.Name == "OnLifecycleEventAttribute" && ca.AttributeType.Namespace == lifecycle_ns))) {
					var attr = method.CustomAttributes.First (ca => ca.AttributeType.Name == "OnLifecycleEventAttribute" && ca.AttributeType.Namespace == lifecycle_ns);
					var evt = (int) attr.ConstructorArguments.First ().Value;
					if (evt == OnLifecycleEvent.OnAny) {
						writer.WriteLine (@"
			mReceiver.OnAny (owner, evt);
			");
						continue;
					}
					write (evt, OnLifecycleEvent.OnCreate, "OnCreate", method.Name);
					write (evt, OnLifecycleEvent.OnStart, "OnStart", method.Name);
					write (evt, OnLifecycleEvent.OnPause, "OnPause", method.Name);
					write (evt, OnLifecycleEvent.OnResume, "OnResume", method.Name);
					write (evt, OnLifecycleEvent.OnStop, "OnStop", method.Name);
					write (evt, OnLifecycleEvent.OnDestroy, "OnDestroy", method.Name);
				}
				writer.WriteLine (@"
		}
	}
}
");
			}
			
			return writer.ToString ();
		}
		
		public IEnumerable<TypeDefinition> GetTargetTypes ()
		{
			var assemblyDefinitions = this.assemblies.Select (a => AssemblyDefinition.ReadAssembly (a, new ReaderParameters ())).ToArray ();
			// We don't have to inspect assemblies that don't reference the assembly that contains ILifecycleObserver.
			// No need to inspect system/framework assemblies.
			var assembliesToInspect = assemblyDefinitions.Where (a => ReferencesAssembly (assemblyDefinitions, a, CommonAssemblyName)).ToArray ();
			
			var types = assembliesToInspect.SelectMany (a => a.Modules).SelectMany (m => m.Types);
			var targets = types.Where (t => ImplementsInterface (types, t, "ILifecycleObserver", lifecycle_ns));
			return targets;
		}

		bool ReferencesAssembly (IEnumerable<AssemblyDefinition> assemblies, AssemblyDefinition referrer, string targetAssemblyName)
		{
			if (referrer == null)
				return false;
			var refs = referrer.Modules.SelectMany (m => m.AssemblyReferences).Distinct ();
			if (refs.Any (r => r.Name == targetAssemblyName))
				return true;
			return refs.Any (r => ReferencesAssembly (assemblies, assemblies.FirstOrDefault (a => a.Name.Name == r.Name), targetAssemblyName));
		}
		
		bool ImplementsInterface (IEnumerable<TypeDefinition> types, TypeDefinition type, string name, string ns)
		{
			if (type == null || type.Name == "Object" && (type.Namespace == "Java.Lang" || type.Namespace == "System"))
				return false;
			if (type.Interfaces.Any (i => i.Name == name && i.Namespace == ns || ImplementsInterface (types, Resolve (types, i), name, ns)))
				return true;
			if (type.BaseType == null)
				return false;
			return ImplementsInterface (types, Resolve (types, type.BaseType), name, ns);
		}

		TypeDefinition Resolve (IEnumerable<TypeDefinition> types, TypeReference tr)
		{
			return types.FirstOrDefault (t => t.Name == tr.Name && t.Namespace == tr.Namespace);
		}
	}
}
