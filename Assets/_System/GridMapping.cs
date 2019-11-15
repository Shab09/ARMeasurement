using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridMapping : MonoBehaviour
{
    public struct point{
        public float x;
        public float y;
        public float z;
    }
    public struct distance{
        public point one;
        public point two;
        public float distanceBWpoints;
        public string nameofdistance;
    }
    public List<distance> storage = new List<distance>();
}
