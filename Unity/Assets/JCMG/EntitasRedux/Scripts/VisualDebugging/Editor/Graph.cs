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

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace JCMG.EntitasRedux.VisualDebugging.Editor
{
	internal sealed class Graph
	{
		private const float ANCHOR_RADIUS = 1f;
		private const string AXIS_FORMAT = "{0:0.0}";
		private const float AXIS_ROUNDING = 1f;
		private const int GRID_LINES = 1;
		private const string LABEL_FORMAT = "{0:0.0}";
		private const int RIGHT_LINE_PADDING = -15;
		private const float X_BORDER = 48f;
		private const float Y_BORDER = 12f;

		private readonly Vector3[] _cachedLinePointVertices;
		private readonly GUIStyle _centeredStyle;
		private readonly GUIStyle _labelTextStyle;
		private readonly Vector3[] _linePoints;

		public Graph(int dataLength)
		{
			_labelTextStyle = new GUIStyle(GUI.skin.label);
			_labelTextStyle.alignment = TextAnchor.UpperRight;

			_centeredStyle = new GUIStyle();
			_centeredStyle.alignment = TextAnchor.UpperCenter;
			_centeredStyle.normal.textColor = Color.white;

			_linePoints = new Vector3[dataLength];
			_cachedLinePointVertices = new []
			{
				new Vector3(-1f, 1f, 0.0f) * ANCHOR_RADIUS,
				new Vector3(1f, 1f, 0.0f) * ANCHOR_RADIUS,
				new Vector3(1f, -1f, 0.0f) * ANCHOR_RADIUS,
				new Vector3(-1f, -1f, 0.0f) * ANCHOR_RADIUS
			};
		}

		public void Draw(float[] data, float height, Color lineColor)
		{
			var controlRect = EditorGUILayout.GetControlRect();
			var rect = GUILayoutUtility.GetRect(controlRect.width, height);
			var top = rect.y + Y_BORDER;
			var floor = rect.y + rect.height - Y_BORDER;
			var availableHeight = floor - top;
			var max = data.Length != 0 ? data.Max() : 0.0f;
			if (Math.Abs(max % (double)AXIS_ROUNDING) > 0.001)
			{
				max = (float)(max + (double)AXIS_ROUNDING - max % (double)AXIS_ROUNDING);
			}

			DrawGridLines(
				top,
				rect.width,
				availableHeight,
				max);

			DrawAverage(
				data,
				top,
				floor,
				rect.width,
				availableHeight,
				max);

			DrawLine(
				data,
				floor,
				rect.width,
				availableHeight,
				max,
				lineColor);
		}

		public void Draw(float[][] data, float width, float height, Color[] lineColors)
		{
			var rect = GUILayoutUtility.GetRect(width, height);
			var top = rect.y + Y_BORDER;
			var floor = rect.y + rect.height - Y_BORDER;
			var availableHeight = floor - top;
			var max = Mathf.Max(0.0f, data.Select(x => x.Max()).Max());
			if (Math.Abs(max % (double)AXIS_ROUNDING) > 0.001)
			{
				max = (float)(max + (double)AXIS_ROUNDING - max % (double)AXIS_ROUNDING);
			}

			DrawGridLines(
				top,
				rect.width,
				availableHeight,
				max);

			for (var i = 0; i < data.Length; i++)
			{
				var newData = data[i];

				DrawLine(
					newData,
					floor,
					rect.width,
					availableHeight,
					max,
					lineColors[i]);
			}
		}

		private void DrawGridLines(float top, float width, float availableHeight, float max)
		{
			var color = Handles.color;
			Handles.color = Color.grey;
			var num1 = GRID_LINES + 1;
			var num2 = availableHeight / num1;
			for (var index = 0; index <= num1; ++index)
			{
				var num3 = top + num2 * index;
				Handles.DrawLine(new Vector2(X_BORDER, num3), new Vector2(width - RIGHT_LINE_PADDING, num3));
				GUI.Label(
					new Rect(
						0.0f,
						num3 - 8f,
						X_BORDER - 2f,
						50f),
					string.Format(AXIS_FORMAT, (float)(max * (1.0 - index / (double)num1))),
					_labelTextStyle);
			}

			Handles.color = color;
		}

		private void DrawAverage(
			float[] data,
			float top,
			float floor,
			float width,
			float availableHeight,
			float max)
		{
			var color = Handles.color;
			Handles.color = Color.yellow;
			var num1 = data.Average();
			var num2 = floor - availableHeight * (num1 / max);
			Handles.DrawLine(new Vector2(X_BORDER, num2), new Vector2(width - RIGHT_LINE_PADDING, num2));
			Handles.color = color;
		}

		private void DrawLine(
			float[] data,
			float floor,
			float width,
			float availableHeight,
			float max,
			Color lineColor)
		{
			var num1 = (width - X_BORDER - RIGHT_LINE_PADDING) / data.Length;
			var color = Handles.color;
			var rect = new Rect();
			var flag = false;
			var num2 = 0.0f;
			Handles.color = lineColor;
			Handles.matrix = Matrix4x4.identity;
			HandleUtility.handleMaterial.SetPass(0);
			for (var index = 0; index < data.Length; ++index)
			{
				var num3 = data[index];
				var num4 = floor - availableHeight * (num3 / max);
				var vector21 = new Vector2(X_BORDER + num1 * index, num4);
				_linePoints[index] = new Vector3(vector21.x, vector21.y, 0.0f);
				if (!flag)
				{
					var num6 = ANCHOR_RADIUS * 3f;
					var num7 = ANCHOR_RADIUS * 6f;
					var vector22 = vector21 - Vector2.up * 0.5f;
					rect = new Rect(
						vector22.x - num6,
						vector22.y - num6,
						num7,
						num7);
					if (rect.Contains(Event.current.mousePosition))
					{
						flag = true;
						num2 = num3;
					}
				}

				Handles.matrix = Matrix4x4.TRS(_linePoints[index], Quaternion.identity, Vector3.one);
				Handles.DrawAAConvexPolygon(_cachedLinePointVertices);
			}

			Handles.matrix = Matrix4x4.identity;
			Handles.DrawAAPolyLine(2f, data.Length, _linePoints);
			if (flag)
			{
				ref var local1 = ref rect;
				local1.y = local1.y - 16f;
				ref var local2 = ref rect;
				local2.width = local2.width + 50f;
				ref var local3 = ref rect;
				local3.x = local3.x - 25f;
				GUI.Label(rect, string.Format(LABEL_FORMAT, num2), _centeredStyle);
			}

			Handles.color = color;
		}
	}
}
