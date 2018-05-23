using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility
{
	public class Timer : IDisposable
	{
		class InnerTImer : MonoBehaviour
		{
			public List<int> keys = new List<int>();
			public Dictionary<int, float> times = new Dictionary<int, float>();
			public Dictionary<int, Action> timeouts = new Dictionary<int, Action>();

			private void Update()
			{
				foreach (var k in keys)
				{
					if (times[k] <= 0) continue;

					times[k] -= Time.deltaTime;
					if (times[k] > 0) continue;

					timeouts[k]?.Invoke();
				}
			}
		}

		static InnerTImer innerTimer;
		static InnerTImer InnerTimer
		{
			get
			{
				if (innerTimer != null)
					return innerTimer;

				innerTimer = GlobalObject.GetOrAddComponent<InnerTImer>();
				return innerTimer;
			}
		}

		private int timerID;

		public event Action OnTimeOut;

		public Timer()
		{
			timerID = GetHashCode();
			InnerTimer.keys.Add(timerID);
			InnerTimer.times.Add(timerID, 0);
			InnerTimer.timeouts.Add(timerID, OnTimeOut);
		}

		public bool IsReachedTime()
		{
			return InnerTimer.times[timerID] <= 0;
		}

		public void Start(float time)
		{
			InnerTimer.times[timerID] = time;
		}

		public void Dispose()
		{
			InnerTimer.keys.Remove(timerID);
			InnerTimer.times.Remove(timerID);
			InnerTimer.timeouts.Remove(timerID);
		}
	}
}

