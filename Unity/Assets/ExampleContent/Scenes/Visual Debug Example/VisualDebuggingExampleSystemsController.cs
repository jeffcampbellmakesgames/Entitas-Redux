using JCMG.EntitasRedux;
using UnityEngine;

namespace ExampleContent.VisualDebugging
{
	public class VisualDebuggingExampleSystemsController : MonoBehaviour
	{
		private class SomeSystems : Feature
		{
			public SomeSystems(Contexts contexts)
			{
				Add(new SlowInitializeSystem());
				Add(new SlowInitializeUpdateSystem());
				Add(new FastSystem());
				Add(new SlowSystem());
				Add(new RandomDurationUpdateSystem());
				Add(new RandomDurationLateUpdateSystem());
				Add(new AReactiveSystem(contexts));

				Add(new RandomValueSystem(contexts));
				Add(new ProcessRandomValueSystem(contexts));
				Add(new CleanupSystem());
				Add(new TearDownSystem());
				Add(new MixedSystem());
			}
		}

		private Contexts _contexts;
		private Systems _systems;

		private void Start()
		{
			_contexts = new Contexts();
			_systems = CreateSystems();
			_systems.Initialize();

			//// Test call
			_contexts.VisualDebug.CreateEntity().AddMyString("");
		}

		private void OnDestroy()
		{
			_systems.TearDown();
		}

		private void FixedUpdate()
		{
			_systems.FixedUpdate();
		}

		private void Update()
		{
			_contexts.VisualDebug.GetGroup(VisualDebugMatcher.MyString)
				.GetSingleEntity()
				.ReplaceMyString(Random.value.ToString());

			_systems.Update();
			_systems.Execute();
		}

		private void LateUpdate()
		{
			_systems.LateUpdate();
			_systems.Cleanup();
		}

		/// <summary>
		/// Creates all systems for visual debugging
		/// </summary>
		/// <returns></returns>
		private Systems CreateSystems()
		{
			var systems = new Systems();

			// Create Nested Systems
			systems.Add(CreateNestedSystems());

			// All System Types Combinations
			systems.Add(CreateAllTypesOfSystems());

			// Create Empty Systems
			systems.Add(CreateEmptySystems());

			// Duplicate systems added to same Feature
			systems.Add(CreateSameInstance());

			// Add per-context cleanup systems
			systems.Add(new VisualDebugCleanupFeature());

			return systems;
		}

		private Systems CreateNestedSystems()
		{
			var systems1 = new Feature("Nested 1");
			var systems2 = new Feature("Nested 2");
			var systems3 = new Feature("Nested 3");

			systems1.Add(systems2);
			systems1.Add(CreateSomeSystems());
			systems2.Add(systems3);

			return new Feature("Nested Systems")
				.Add(systems1);
		}

		private Systems CreateSomeSystems()
		{
			return new SomeSystems(_contexts);
		}

		private Systems CreateAllTypesOfSystems()
		{
			var allSystems = new Feature("All System Combinations")
				.Add(new SomeInitializeSystem())
				.Add(new SomeUpdateSystem())
				.Add(new SomeReactiveSystem(_contexts))
				.Add(new SomeMultiReactiveSystem(_contexts))
				.Add(new SomeInitializeUpdateSystem())
				.Add(new SomeInitializeReactiveSystem(_contexts));

			var subSystems = new Feature("Sub Systems").Add(allSystems);

			return new Feature("Systems with SubSystems")
				.Add(allSystems)
				.Add(allSystems)
				.Add(subSystems)
				.Add(subSystems);
		}

		private Systems CreateEmptySystems()
		{
			var systems1 = new Feature("Empty 1");
			var systems2 = new Feature("Empty 2");
			var systems3 = new Feature("Empty 3");

			systems1.Add(systems2);
			systems2.Add(systems3);

			return new Feature("Empty Systems")
				.Add(systems1);
		}

		private Systems CreateSameInstance()
		{
			var system = new RandomDurationUpdateSystem();
			return new Feature("Same System Instances")
				.Add(system)
				.Add(system)
				.Add(system);
		}
	}
}
