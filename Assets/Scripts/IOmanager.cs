using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IOmanager : MonoBehaviour
{
    public static IOmanager IO;

    [SerializeField]
    private Sprite idle, happy, angry, shocked, talk;

    [SerializeField]
    private RawImage image;

    private void Start()
    {
        Application.runInBackground = true;

        IO = this;
        image.texture = idle.texture;
    }

    private IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(2);
        Idle();
    }

    public void Idle()
    {
        image.texture = idle.texture;
    }

    public void Angry()
    {
        image.texture = angry.texture;
        StartCoroutine(ReturnToIdle());
    }

    public void Happy()
    {
        image.texture = happy.texture;
        StartCoroutine(ReturnToIdle());
    }

    public void Shocked()
    {
        image.texture = shocked.texture;
        StartCoroutine(ReturnToIdle());
    }

    public void Talk()
    {
        image.texture = talk.texture;
        StartCoroutine(ReturnToIdle());
    }
}