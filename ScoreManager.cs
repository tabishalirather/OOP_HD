﻿using oop_custom_program;

using System.Collections.Generic;

namespace oop_custom_program
{
    public class ScoreManager : ISubject
    {
        private List<IObserver> _observers = new List<IObserver>();
        private int _score;

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                Notify();
            }
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
    }
}
