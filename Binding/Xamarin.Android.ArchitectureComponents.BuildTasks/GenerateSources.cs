using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Android.ArchitectureComponents.BuildTasks
{
	public class GenerateSources : Task
	{
		[Required]
		public ITaskItem [] Assemblies { get; set; }

		public ITaskItem OutputSource { get; set; }

		public override bool Execute ()
		{
			Log.LogMessage (MessageImportance.Low, "GenerateSources Task:");
			Log.LogMessage (MessageImportance.Low, "  Assemblies:");
			foreach (var asm in Assemblies)
				Log.LogMessage (MessageImportance.Low, "    " + asm);
			Log.LogMessage (MessageImportance.Low, "  OutputSource: " + OutputSource);
			
			var source = new LifecycleCodeGenerator ().GenerateCode (Assemblies.Select (a => a.ItemSpec));
			Directory.CreateDirectory (Path.GetDirectoryName (OutputSource.ItemSpec));
			File.WriteAllText (OutputSource.ItemSpec, source);
			var p = new Microsoft.CSharp.CSharpCodeProvider ();
			var options = new System.CodeDom.Compiler.CompilerParameters ();
			options.ReferencedAssemblies.AddRange (Assemblies.Select (a => a.ItemSpec).Where (a => a.EndsWith (".dll", StringComparison.OrdinalIgnoreCase)).ToArray ());
			var ccu = new System.CodeDom.CodeSnippetCompileUnit () { Value = source };
			p.CompileAssemblyFromDom (options, ccu);
			return true;
		}
	}
}
