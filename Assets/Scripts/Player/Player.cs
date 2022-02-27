using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] List<GameObject> pizzaGameObjectsInStack;
    private int pizzaCountInStack;
    private int currentPizzaCount;

    [Header("Crash Obstacle Prefabs")]
    [SerializeField] GameObject emptyPizzaBox;
    [SerializeField] GameObject pizzaSlice;

    [Header("Engine Audio Source")]
    [SerializeField] AudioSource engineAudioSource;
    [SerializeField] AudioClip engineAudio;

    [Header("Main Audio Source")]
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip pickSound;
    [SerializeField] AudioClip greenAreaSound;
    [SerializeField] AudioClip redAreaSound;
    [SerializeField] AudioClip finishLineSound;
    [SerializeField] List<AudioClip> throwPizzaSounds;

    float engineVolume = 0.5f;

    private void Start()
    {
        pizzaCountInStack = pizzaGameObjectsInStack.Count;
        mainAudioSource = GetComponent<AudioSource>();

        StartCoroutine(StartEngine());
    }

    IEnumerator StartEngine()
    {
        yield return new WaitForSeconds(1f);

        FindObjectOfType<GameManager>().StartLevel();

        engineAudioSource.clip = engineAudio;
        engineAudioSource.volume = 0;
        engineAudioSource.Play();

        float totalTime = 3f; // fade audio in over 3 seconds
        float currentTime = 0;

        while (engineAudioSource.volume < engineVolume)
        {
            currentTime += Time.deltaTime;
            engineAudioSource.volume = Mathf.Lerp(0, engineVolume, currentTime / totalTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pizza"))
        {
            mainAudioSource.PlayOneShot(pickSound, 1f);

            currentPizzaCount++;
            UpdatePizzaStackActiveStatus();
            StartCoroutine(PizzaAddAnimation());
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Obstacle"))
        {
            mainAudioSource.PlayOneShot(crashSound, 1f);

            if (currentPizzaCount - 1 >= 0)
            {
                currentPizzaCount--;
                UpdatePizzaStackActiveStatus();

                StartCoroutine(ThrowPizzaBox(other.gameObject));
            }
        }

        if(other.gameObject.CompareTag("Area"))
        {
            Area area = other.GetComponent<Area>();
            OPERATION OPR;
            OPR = area.GetOperation();
            int value = area.GetOperationValue();

            DoOperation(OPR, value);

            AREACOLOR areaColor = area.GetAreaColor();

            if (areaColor == AREACOLOR.GREEN) mainAudioSource.PlayOneShot(greenAreaSound, 1f);
            else mainAudioSource.PlayOneShot(redAreaSound, 1f);
        }
    }

    IEnumerator ThrowPizzaBox(GameObject other)
    {
        GameObject temp = Instantiate(emptyPizzaBox, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
        Destroy(temp, 2f);

        temp = Instantiate(pizzaSlice, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
        Destroy(temp, 2f);

        temp = Instantiate(pizzaSlice, pizzaGameObjectsInStack[currentPizzaCount - 1].transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(ForceVector(), ForceMode.Impulse);
        temp.GetComponent<Rigidbody>().AddTorque(ForceVector() * 5, ForceMode.Impulse);
        Destroy(temp, 2f);

        yield return new WaitForSeconds(0.12f);

        animator.SetTrigger("hit");
        FindObjectOfType<PlayerStackPosition>().Hit();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FinishLine"))
        {
            StartCoroutine(VolumeDown());
            Invoke("Finish", 0.5f);
        }
    }

    IEnumerator VolumeDown()
    {
        float totalTime = 1; // fade audio out over 3 seconds
        float currentTime = 0;

        while (engineAudioSource.volume > 0)
        {
            currentTime += Time.deltaTime;
            engineAudioSource.volume = Mathf.Lerp(engineVolume, 0, currentTime / totalTime);
            yield return null;
        }

        engineAudioSource.Stop();
    }

    void Finish()
    {
        mainAudioSource.PlayOneShot(finishLineSound, 1f);
        FindObjectOfType<GameManager>().FinishLevel();
        FindObjectOfType<PlayerStackPosition>().FinishLevel();
        animator.SetTrigger("levelFinished");
        Invoke("InvokeChangeCamera", 0.5f);
        StartCoroutine(PizzaServeAnimation());
    }

    void InvokeChangeCamera()
    {
        FindObjectOfType<ChangeCamera>().FinishLineCamera();
    }

    void DoOperation(OPERATION opr, int value)
    {
        if (opr == OPERATION.ADDITION) currentPizzaCount += value;
        else if (opr == OPERATION.SUBTRACTION) currentPizzaCount -= value;
        else if (opr == OPERATION.MULTIPLICATION) currentPizzaCount *= value;
        else
        {
            if (value == 0) return;
            float temp = (float)currentPizzaCount / value;
            currentPizzaCount = Mathf.FloorToInt(temp);
        }

        if (currentPizzaCount < 0) currentPizzaCount = 0;

        UpdatePizzaStackActiveStatus();
        StartCoroutine(PizzaAddAnimation());
    }

    void UpdatePizzaStackActiveStatus()
    {
        for(int i = 0; i < pizzaCountInStack; i++)
        {
            if(i < currentPizzaCount)
                pizzaGameObjectsInStack[i].SetActive(true);
            else
                pizzaGameObjectsInStack[i].SetActive(false);
        }
    }

    IEnumerator PizzaAddAnimation()
    {
        int temp = currentPizzaCount;
        if (temp > pizzaCountInStack) temp = pizzaCountInStack;

        for (int i = temp - 1; i >= 0; i--)
        {
            pizzaGameObjectsInStack[i].transform.DORewind();
            pizzaGameObjectsInStack[i].transform.DOPunchScale(new Vector3(0.3f, 0.5f, 0.3f), .25f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator PizzaServeAnimation()
    {
        yield return new WaitForSeconds(1f);

        List<Transform> tablePositions = FindObjectOfType<TablePositions>().GetTablePositions();
        int counter = 0;

        int temp = currentPizzaCount;
        if (temp > pizzaCountInStack) temp = pizzaCountInStack;

        for (int i = temp - 1; i >= 0; i--)
        {
            mainAudioSource.PlayOneShot(throwPizzaSounds[Random.Range(0, throwPizzaSounds.Count)], 0.7f);
            pizzaGameObjectsInStack[i].transform.DOJump(tablePositions[counter].position, 2f, 1, 1f);
            
            counter++;
            if (counter >= tablePositions.Count) counter = 0;

            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(1f);
        FindObjectOfType<GameManager>().LoadNextLevel();
    }

    Vector3 ForceVector()
    {
        return new Vector3(RandomNumber() * MinusCounter(), RandomNumber(), -RandomNumber());
    }

    float RandomNumber()
    {
        return Random.Range(5, 15);
    }

    float MinusCounter()
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
            return 1;
        else
            return -1;
    }
}
