using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
using Android.ArchitectureComponents.BuildTasks;

namespace BuildTaskTestRunner
{
	class MainClass
	{
		public static void Main (string [] args)
		{
			var files = args.SelectMany (a => Directory.GetFiles (Path.GetDirectoryName (a), Path.GetFileName (a)));
			var source = new LifecycleCodeGenerator ().GenerateCode (files);
			Console.WriteLine (source);
			var p = new CSharpCodeProvider ();
			var options = new CompilerParameters ();
			options.ReferencedAssemblies.AddRange (args.Where (a => a.EndsWith (".dll", StringComparison.OrdinalIgnoreCase)).ToArray ());
			var ccu = new CodeSnippetCompileUnit () { Value = source };
			var result = p.CompileAssemblyFromDom (options, ccu);
			
			foreach (var s in result.Output)
				Console.WriteLine (s);
		}
	}
}
