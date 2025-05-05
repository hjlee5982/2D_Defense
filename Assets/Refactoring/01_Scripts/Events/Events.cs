using UnityEngine;



// UI_SpawnAlly -> JGameManager 
public class StartSpawnAllyEvent
{
    public int BtnIdx = -1;
    public StartSpawnAllyEvent(int btnIdx) 
    {
        BtnIdx = btnIdx;
    }
}



// JUIManager¿¡ ÀÖ´Â StartButton -> JGameManager 
public class StartRoundEvent
{
    public StartRoundEvent() { }
}





// JGameManager -> AllySpawner
public class BeginSpawnAllyEvent
{
    public int BtnIdx = -1;
    public BeginSpawnAllyEvent(int btnIdx) 
    {
        BtnIdx = btnIdx;
    }
}



// JGameManager -> MonsterSpawner
public class BeginSpawnMonsterEvent
{ 

}
