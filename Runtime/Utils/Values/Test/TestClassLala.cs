using Padoru.Core;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

public class TestClassLala : MonoBehaviour
{
    [Subscribable]
    public int lolo = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        var token = lolo.Subscribe(x => lolo, OnLoloChanged);
        lolo = 2;
        lolo.Notify(x => lolo);
        lolo = 3;
        lolo.Notify(x => lolo);
        token?.Invoke();
        lolo = 4;
        lolo.Notify(x => lolo);
    }

    private void OnLoloChanged(int obj)
    {
        Debug.LogError(obj);
    }
}
