using System.Collections.Generic;
using UnityEngine;

public class Subject<T> : MonoBehaviour
{
    private readonly List<IObserver<T>> observers = new List<IObserver<T>>();

    public void AddObserver(IObserver<T> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(IObserver<T> observer)
    {
        observers.Remove(observer);
    }

    protected void Notify(T data)
    {
        var observersCopy = new List<IObserver<T>>(observers);
        foreach (var observer in observersCopy)
        {
            observer.OnNotify(data);
        }
    }
}