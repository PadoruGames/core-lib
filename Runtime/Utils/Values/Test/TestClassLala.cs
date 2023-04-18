using Padoru.Core;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

public class TestClassLala : MonoBehaviour
{
    [Subscribable]
    public string lolo = "Lele";
    
    // Start is called before the first frame update
    void Start()
    {
        var token = lolo.Subscribe(x => lolo, OnLoloChanged);
        lolo = "Lolo";
        lolo.Notify(x => lolo);
        lolo = "Lulu";
        lolo.Notify(x => lolo);
        token?.Invoke();
        lolo = "Lele";
        lolo.Notify(x => lolo);
    }

    private void OnLoloChanged(string obj)
    {
        Debug.LogError(obj);
    }
}
