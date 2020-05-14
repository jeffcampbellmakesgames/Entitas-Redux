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
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging
{
	[ExecuteInEditMode]
	public class EntityBehaviour : MonoBehaviour
	{
		public IContext Context => _context;

		public IEntity Entity => _entity;

		private string _cachedName;

		private IContext _context;
		private IEntity _entity;
		private Stack<EntityBehaviour> _entityBehaviourPool;

		public void Init(IContext context, IEntity entity, Stack<EntityBehaviour> entityBehaviourPool)
		{
			_context = context;
			_entity = entity;
			_entityBehaviourPool = entityBehaviourPool;
			_entity.OnEntityReleased += OnEntityReleased;
			gameObject.hideFlags = HideFlags.None;
			gameObject.SetActive(true);
			Update();
		}

		private void OnEntityReleased(IEntity e)
		{
			_entity.OnEntityReleased -= OnEntityReleased;
			gameObject.SetActive(false);
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			_entityBehaviourPool.Push(this);
			_cachedName = null;
			name = string.Empty;
		}

		private void Update()
		{
			if (_entity != null && _cachedName != _entity.ToString())
			{
				name = _cachedName = _entity.ToString();
			}
		}

		private void OnDestroy()
		{
			if (_entity != null)
			{
				_entity.OnEntityReleased -= OnEntityReleased;
			}
		}
	}
}
