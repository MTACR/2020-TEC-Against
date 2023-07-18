using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAssistant : MonoBehaviour //TODO: talvez criar uma função própria para troca de facção
{
    private ArrayList enemyAI;
    private ArrayList players; //and Allies


    void Start()
    {
        enemyAI = new ArrayList();
        players = new ArrayList();
    }

    public void EnabledAI(Character character, bool enabled)
    {
        if (character.CompareTag("Player"))
        {
            if (enabled)
                players.Add(character);
            else
                players.Remove(character);
        }
        else
        {
            if (enabled)
                enemyAI.Add(character);
            else
                enemyAI.Remove(character);
        }
    }

    public Character GetTarget(Vector3 position, string faction)
    {
        Character target = null;
        ArrayList targetList;
        float distance;
        float minDistance = float.MaxValue;

        if (faction.Equals("Player"))
            targetList = enemyAI;
        else
            targetList = players;

        foreach (Character instance in targetList)
        {
            if (instance && !instance.IsDead())
            {
                distance = Vector2.Distance(instance.transform.position, position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = instance;
                }
            }
        }

        return target;
    }

    //public void GotDestroyed();
}
