using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FollowMousePosition : MonoBehaviour//, IObservable<float>
{
    [SerializeField] InputActionReference _mousePosition;
    [SerializeField] Camera _referenceCamera;

    //UnityAction _action;
    //[SerializeField] UnityEvent _event;


    //Func<int, string, float> function;

    //public event UnityAction OnClick
    //{
    //    add => _onClick.AddListener(value);
    //    remove => _onClick.RemoveListener(value);
    //}
    //[SerializeField] UnityEvent _onClick;

    //public Action bipboup;
    //public event Action bipboup2;

    //private void Awake()
    //{
    //    bipboup += () => { };
    //    bipboup2 += () =s> { };

    //    bipboup = null;
    //    bipboup2 = null;

    //    Input.GetButtonDown
    //}

    //float Hello2(int arg1, string arg2)
    //{

    //    return 0f;

    //}

    //float Hello(int arg1, string arg2="")
    //{
    //    return 0f;
    //}

    void Update()
    {

        //_event.AddListener()

        //function = Hello;
        //function = Hello2;
        //function.Invoke(12, "HelloWorld");

        var p = _referenceCamera.ScreenToWorldPoint(_mousePosition.action.ReadValue<Vector2>());
        transform.position = new Vector3(p.x, p.y, 0);

    }

    //IObserver<float> toCall;
    //public IDisposable Subscribe(IObserver<float> observer)
    //{

    //    toCall = observer;


    //    toCall.OnNext(12f);

    //    return new DisposeCall();
    //}

    //class DisposeCall : IDisposable
    //{
    //    private bool disposedValue;

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposedValue)
    //        {
    //            if (disposing)
    //            {
    //                // TODO: supprimer l'�tat manag� (objets manag�s)
    //            }

    //            // TODO: lib�rer les ressources non manag�es (objets non manag�s) et substituer le finaliseur
    //            // TODO: affecter aux grands champs une valeur null
    //            disposedValue = true;
    //        }
    //    }

    //    public DisposeCall()
    //    {

    //    }

    //    // // TODO: substituer le finaliseur uniquement si 'Dispose(bool disposing)' a du code pour lib�rer les ressources non manag�es
    //    ~DisposeCall()
    //    {
    //        // Ne changez pas ce code. Placez le code de nettoyage dans la m�thode 'Dispose(bool disposing)'
    //        Dispose(disposing: false);
    //    }

    //    public void Dispose()
    //    {
    //        // Ne changez pas ce code. Placez le code de nettoyage dans la m�thode 'Dispose(bool disposing)'
    //        Dispose(disposing: true);
    //        GC.SuppressFinalize(this);
    //    }
    //}

}