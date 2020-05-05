/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using JCMG.Genesis.Editor;

namespace JCMG.EntitasRedux.VisualDebugging.Editor.Plugins
{
	internal sealed class FeatureClassGenerator : ICodeGenerator
	{
		private const string FEATURE_TEMPLATE =
			@"#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

public class Feature : JCMG.EntitasRedux.VisualDebugging.DebugSystems {

    public Feature(string name) : base(name) {
    }

    public Feature() : base(true) {

        Initialize(GetType().Name);
    }
}

#elif (!ENTITAS_DISABLE_DEEP_PROFILING && DEVELOPMENT_BUILD)

public class Feature : JCMG.EntitasRedux.Systems {

    System.Collections.Generic.List<string> _initializeSystemNames;
    System.Collections.Generic.List<string> _executeSystemNames;
    System.Collections.Generic.List<string> _cleanupSystemNames;
    System.Collections.Generic.List<string> _tearDownSystemNames;

    public Feature(string name) : this() {
    }

    public Feature() {
        _initializeSystemNames = new System.Collections.Generic.List<string>();
        _executeSystemNames = new System.Collections.Generic.List<string>();
        _cleanupSystemNames = new System.Collections.Generic.List<string>();
        _tearDownSystemNames = new System.Collections.Generic.List<string>();
    }

    public override JCMG.EntitasRedux.Systems Add(JCMG.EntitasRedux.ISystem system) {
        var systemName = system.GetType().FullName;

        if (system is JCMG.EntitasRedux.IInitializeSystem) {
            _initializeSystemNames.Add(systemName);
        }

        if (system is JCMG.EntitasRedux.IExecuteSystem) {
            _executeSystemNames.Add(systemName);
        }

        if (system is JCMG.EntitasRedux.ICleanupSystem) {
            _cleanupSystemNames.Add(systemName);
        }

        if (system is JCMG.EntitasRedux.ITearDownSystem) {
            _tearDownSystemNames.Add(systemName);
        }

        return base.Add(system);
    }

    public override void Initialize() {
        for (int i = 0; i < _initializeSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_initializeSystemNames[i]);
            _initializeSystems[i].Initialize();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void Execute() {
        for (int i = 0; i < _executeSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_executeSystemNames[i]);
            _executeSystems[i].Execute();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void Cleanup() {
        for (int i = 0; i < _cleanupSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_cleanupSystemNames[i]);
            _cleanupSystems[i].Cleanup();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }

    public override void TearDown() {
        for (int i = 0; i < _tearDownSystems.Count; i++) {
            UnityEngine.Profiling.Profiler.BeginSample(_tearDownSystemNames[i]);
            _tearDownSystems[i].TearDown();
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}

#else

public class Feature : JCMG.EntitasRedux.Systems {

    public Feature(string name) {
    }

    public Feature() {
    }
}

#endif
";

		public string Name => NAME;

		public int Priority => 0;

		public bool RunInDryMode => true;

		private const string NAME = "Feature Class";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			return new[]
			{
				new CodeGenFile(
					"Feature.cs",
					FEATURE_TEMPLATE,
					GetType().FullName)
			};
		}
	}
}
