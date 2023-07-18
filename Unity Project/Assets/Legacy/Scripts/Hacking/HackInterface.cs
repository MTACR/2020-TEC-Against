using System;
using UnityEngine;
using UnityEngine.UI;

public class HackInterface : MonoBehaviour
{

    public float maxDist = 5f;
    public float holdTime = 2f;

    public HackParticle particle;
    public AudioSource audioNoTarget;
    public GameObject portal;
    
    private Image bar;
    private RectTransform rect;
    private Canvas canvas;
    private Hackable target;
    private Player2D player;
    private float startTime;
    private float countTime;
    private bool held;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        bar = GetComponentInChildren<Image>(true);
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    void Update()
    {
        if (GameController.isPaused || !GameController.canPause)
            return;

        //if (Input.GetButtonDown("Fire2") && !held)
        if (KeyBindingManager.GetKeyDown(KeyAction.hack) && !held)
        {
            startTime = Time.time;
            countTime = startTime;

            FindTarget();

            if (target == null)
            {
                audioNoTarget.Play();
            }
            else
            {
                if (target.IsHacked())
                {
                    target.ExecuteAction();
                    held = false;
                    target = null;
                    return;
                }
                else
                    canvas.enabled = true;
            }
        }

        if (KeyBindingManager.GetKey(KeyAction.hack) && !held && target != null)
        {
            countTime += Time.deltaTime;

            if (player != null)
                player.LookAt(target.transform.position);

            held = SetProgress(countTime, startTime);
        }

        if (KeyBindingManager.GetKeyUp(KeyAction.hack))
        {
            CancelHacking();
        }
    }

    public void CancelHacking()
    {
        canvas.enabled = false;
        held = false;
        target = null;
    }

    void OnGUI()
    {
        if (target != null)
            rect.position = new Vector3(target.transform.position.x, target.transform.position.y + 1f, 0);
    }

    public bool SetProgress(float countTime, float startTime)
    {

        float counter = (countTime - startTime) / holdTime;

        if (counter > 0 && counter < 1)
        {
            if (Vector2.Distance(target.transform.position, player.transform.position) <= maxDist)
            {
                int c = (int)(counter * 100);

                bar.fillAmount = counter;

                if (c % 10 == 0)
                {
                    Instantiate(particle, player.hand.position, Quaternion.identity).FlyTo(target.transform);
                }
            }
            else
            {
                canvas.enabled = false;
                target = null;
            }
        }

        if (countTime > (startTime + holdTime) && target != null)
        {
            target.Hack();

            canvas.enabled = false;

            foreach (Bullet b in FindObjectsOfType<Bullet>())
                Destroy(b);

            target = null;

            return true;
        }

        return false;
    }

    void FindTarget()
    {
        Hackable[] objs = GameObject.FindObjectsOfType<Hackable>();
        player = FindObjectOfType<Player2D>();

        Hackable result = null;
        Vector3 currentPos = player.transform.position;

        float minDist = Mathf.Infinity;

        foreach (Hackable h in objs)
        {

            Character c = h.GetComponent<Character>();

            if (c != null)
                if (c.IsDead())
                    continue;

            float dist = Vector3.Distance(h.transform.position, currentPos);

            if (dist < minDist && dist <= maxDist)
            {
                result = h;
                minDist = dist;
            }
        }

        target = result;
    }

}
