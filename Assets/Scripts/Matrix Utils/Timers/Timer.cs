namespace MatrixUtils.Timers
{
    using System;
    using UnityEngine;
    public abstract class Timer<T> : ITimer, IDisposable where T : Timer<T>
    {
        bool m_disposed;
        float m_time;
        public float CurrentTime
        {
            get => m_time;
            protected set
            {
                m_time = value;
                if (IsRunning) m_onTimeUpdated.Invoke(value);
            }
        }

        public bool IsRunning { get; private set; }
        protected float InitialTime = 0;
        public float Progress => Mathf.Clamp01(CurrentTime / InitialTime);
        bool UseUnscaledTime { get; set; }
        Action<float> m_onTimeUpdated = delegate { };
        Action m_onTimerStart = delegate { };
        Action m_onTimerStop = delegate { };
        Action m_onTimerPause = delegate { };
        Action m_onTimerResume = delegate { };

        public void Start()
        {
            CurrentTime = InitialTime;
            if (IsRunning) return;
            IsRunning = true;
            TimerManager.RegisterTimer(this);
            m_onTimerStart.Invoke();
        }

        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            TimerManager.DeregisterTimer(this);
            m_onTimerStop.Invoke();
        }

        public abstract void Tick();

        public abstract bool IsFinished { get; }

        public void Resume()
        {
            IsRunning = true;
            m_onTimerResume.Invoke();
        }

        public void Pause()
        {
            IsRunning = false;
            m_onTimerPause.Invoke();
        }

        public virtual void Reset() => CurrentTime = InitialTime;

        public virtual void Reset(float newTime)
        {
            InitialTime = newTime;
            Reset();
        }

        protected float GetDeltaTime()
        {
            return UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        }

        /// <summary>
        /// Adds a callback to be invoked when the timer starts
        /// </summary>
        public T OnStart(Action callback)
        {
            m_onTimerStart += callback;
            return this as T;
        }

        /// <summary>
        /// Adds a callback to be invoked when the timer stops/completes
        /// </summary>
        public T OnComplete(Action callback)
        {
            m_onTimerStop += callback;
            return this as T;
        }

        /// <summary>
        /// Adds a callback to be invoked when the timer is paused
        /// </summary>
        public T OnPause(Action callback)
        {
            m_onTimerPause += callback;
            return this as T;
        }

        /// <summary>
        /// Adds a callback to be invoked when the timer is resumed
        /// </summary>
        public T OnResume(Action callback)
        {
            m_onTimerResume += callback;
            return this as T;
        }

        /// <summary>
        /// Sets whether this timer uses unscaled time (ignores Time.timeScale)
        /// </summary>
        public T SetUseUnscaledTime(bool useUnscaled)
        {
            UseUnscaledTime = useUnscaled;
            return this as T;
        }

        public T OnTimeUpdated(Action<float> callback)
        {
            m_onTimeUpdated += callback;
            return this as T;
        }
        
        /// <summary>
        /// Resets the timer to its default state, clearing all callbacks and settings
        /// </summary>
        protected virtual void ResetState()
        {
            Stop();
            m_onTimerStart = delegate { };
            m_onTimerStop = delegate { };
            m_onTimerPause = delegate { };
            m_onTimerResume = delegate { };
            UseUnscaledTime = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(m_disposed) return;
            if (disposing)
            {
                TimerManager.DeregisterTimer(this);
            }
            m_disposed = true;
        }

        ~Timer()
        {
            Dispose(false);
        }
    }
}