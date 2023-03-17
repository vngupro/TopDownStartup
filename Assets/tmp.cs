using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class tmp : MonoBehaviour
{

    PlayerReference _pr;

    [SerializeField] UnityEvent<int, int, float> _event;
    [SerializeField] List<SerializableEvent> _event2;
    [SerializeField] List<SerializableEvent<int>> _event3;

    CinemachineBrain cb;

    private void Awake()
    {

        _event.Invoke(12, 3712, 3f);

        _event2.Invoke();

        _event3.Invoke(12);
    }


    public void coucou()
    {

    }

    public void coucou2(int a)
    {

    }

    public void coucou3(int a, int b)
    {

    }

    public void coucou4(int a, int b, float f)
    {

    }

}
