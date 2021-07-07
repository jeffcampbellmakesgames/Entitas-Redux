#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)

public class Feature : JCMG.EntitasRedux.VisualDebugging.DebugSystems
{
	public Feature(string name) : base(name)
	{
	}

	public Feature() : base(true)
	{
		Initialize(GetType().Name);
	}
}

#elif (!ENTITAS_DISABLE_DEEP_PROFILING && DEVELOPMENT_BUILD)

public class Feature : JCMG.EntitasRedux.Systems
{
	System.Collections.Generic.List<string> _initializeSystemNames;
	System.Collections.Generic.List<string> _fixedUpdateSystemNames;
	System.Collections.Generic.List<string> _updateSystemNames;
	System.Collections.Generic.List<string> _lateUpdateSystemNames;
	System.Collections.Generic.List<string> _reactiveSystemNames;
	System.Collections.Generic.List<string> _cleanupSystemNames;
	System.Collections.Generic.List<string> _tearDownSystemNames;

	public Feature(string name) : this()
	{
	}

	public Feature()
	{
		_initializeSystemNames = new System.Collections.Generic.List<string>();
		_fixedUpdateSystemNames = new System.Collections.Generic.List<string>();
		_updateSystemNames = new System.Collections.Generic.List<string>();
		_lateUpdateSystemNames = new System.Collections.Generic.List<string>();
		_reactiveSystemNames = new System.Collections.Generic.List<string>();
		_cleanupSystemNames = new System.Collections.Generic.List<string>();
		_tearDownSystemNames = new System.Collections.Generic.List<string>();
	}

	public override JCMG.EntitasRedux.Systems Add(JCMG.EntitasRedux.ISystem system)
	{
		var systemName = system.GetType().FullName;

		if (system is JCMG.EntitasRedux.IInitializeSystem)
		{
			_initializeSystemNames.Add(systemName);
		}

		if (system is JCMG.EntitasRedux.IFixedUpdateSystem)
		{
			_fixedUpdateSystemNames.Add(systemName);
		}

		if (system is JCMG.EntitasRedux.IUpdateSystem)
		{
			_updateSystemNames.Add(systemName);
		}

		if (system is JCMG.EntitasRedux.ILateUpdateSystem)
		{
			_lateUpdateSystemNames.Add(systemName);
		}

		if (system is JCMG.EntitasRedux.IReactiveSystem)
		{
			_reactiveSystemNames.Add(systemName);
		}

		if (system is JCMG.EntitasRedux.ICleanupSystem)
		{
			_cleanupSystemNames.Add(systemName);
		}

		if (system is JCMG.EntitasRedux.ITearDownSystem)
		{
			_tearDownSystemNames.Add(systemName);
		}

		return base.Add(system);
	}

	public override void Initialize()
	{
		for (int i = 0; i < _initializeSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_initializeSystemNames[i]);
			_initializeSystems[i].Initialize();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void FixedUpdate()
	{
		for (int i = 0; i < _fixedUpdateSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_fixedUpdateSystemNames[i]);
			_fixedUpdateSystems[i].FixedUpdate();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void Update()
	{
		for (int i = 0; i < _updateSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_updateSystemNames[i]);
			_updateSystems[i].Update();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void LateUpdate()
	{
		for (int i = 0; i < _lateUpdateSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_lateUpdateSystemNames[i]);
			_lateUpdateSystems[i].LateUpdate();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void Activate()
	{
		for (int i = 0; i < _reactiveSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_reactiveSystemNames[i]);
			_reactiveSystems[i].Activate();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void Deactivate()
	{
		for (int i = 0; i < _reactiveSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_reactiveSystemNames[i]);
			_reactiveSystems[i].Deactivate();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void Clear()
	{
		for (int i = 0; i < _reactiveSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_reactiveSystemNames[i]);
			_reactiveSystems[i].Clear();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void Execute()
	{
		for (int i = 0; i < _reactiveSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_reactiveSystemNames[i]);
			_reactiveSystems[i].Execute();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void Cleanup()
	{
		for (int i = 0; i < _cleanupSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_cleanupSystemNames[i]);
			_cleanupSystems[i].Cleanup();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	public override void TearDown()
	{
		for (int i = 0; i < _tearDownSystems.Count; i++)
		{
			UnityEngine.Profiling.Profiler.BeginSample(_tearDownSystemNames[i]);
			_tearDownSystems[i].TearDown();
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}
}

#else

public class Feature : JCMG.EntitasRedux.Systems
{
	public Feature(string name)
	{
	}

	public Feature()
	{
	}
}

#endif
