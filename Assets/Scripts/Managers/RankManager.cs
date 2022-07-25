using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager : Singleton<RankManager>
{
    public int ranking;
    // Start is called before the first frame update
    void Start()
    {
        ranking = 1;
    }

    public int GetRank() { return ranking++; }

}
