using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private enum ExplosionType
    {
        Small, //int 0
        Medium, //int 1
        Large //int 2
    }

    [SerializeField]
    private ExplosionType _explosionType;

    void OnEnable()
    {
        StartCoroutine(DeactivationRoutine());
    }

    public int GetExplosionType()
    {
        return (int)_explosionType;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    IEnumerator DeactivationRoutine()
    {
        yield return new WaitForSeconds(1f);
        Hide();
    }
}
