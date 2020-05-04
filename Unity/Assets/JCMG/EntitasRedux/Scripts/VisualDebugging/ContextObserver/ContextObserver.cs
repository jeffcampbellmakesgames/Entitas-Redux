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

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging
{
	public class ContextObserver
	{
		public IContext Context
		{
			get { return _context; }
		}

		public IGroup[] Groups
		{
			get { return _groups.ToArray(); }
		}

		public GameObject GameObject
		{
			get { return _gameObject; }
		}

		private readonly IContext _context;
		private readonly Stack<EntityBehaviour> _entityBehaviourPool = new Stack<EntityBehaviour>();
		private readonly GameObject _gameObject;
		private readonly List<IGroup> _groups;

		private StringBuilder _toStringBuilder = new StringBuilder();

		public ContextObserver(IContext context)
		{
			_context = context;
			_groups = new List<IGroup>();
			_gameObject = new GameObject();
			_gameObject.AddComponent<ContextObserverBehaviour>().Init(this);

			_context.OnEntityCreated += OnEntityCreated;
			_context.OnGroupCreated += OnGroupCreated;
		}

		public void Deactivate()
		{
			_context.OnEntityCreated -= OnEntityCreated;
			_context.OnGroupCreated -= OnGroupCreated;
		}

		private void OnEntityCreated(IContext context, IEntity entity)
		{
			var entityBehaviour = _entityBehaviourPool.Count > 0
				? _entityBehaviourPool.Pop()
				: new GameObject().AddComponent<EntityBehaviour>();

			entityBehaviour.Init(context, entity, _entityBehaviourPool);
			entityBehaviour.transform.SetParent(_gameObject.transform, false);
		}

		private void OnGroupCreated(IContext context, IGroup group)
		{
			_groups.Add(group);
		}

		public override string ToString()
		{
			_toStringBuilder.Length = 0;
			_toStringBuilder
				.Append(_context.ContextInfo.name)
				.Append(" (")
				.Append(_context.Count)
				.Append(" entities, ")
				.Append(_context.ReusableEntitiesCount)
				.Append(" reusable, ");

			if (_context.RetainedEntitiesCount != 0)
			{
				_toStringBuilder
					.Append(_context.RetainedEntitiesCount)
					.Append(" retained, ");
			}

			_toStringBuilder
				.Append(_groups.Count)
				.Append(" groups)");

			var str = _toStringBuilder.ToString();
			_gameObject.name = str;
			return str;
		}
	}
}
