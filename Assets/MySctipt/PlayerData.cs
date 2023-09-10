[System.Serializable]
public class PlayerData
{
    // Player‚Ì‚Á‚Ä‚¢‚éƒ`ƒbƒv”
    public uint _chipNum=100;
    private bool firstPlay = false;
    public bool GetFirstPlayFlag()
    {
        return firstPlay;
    }
    public void InitData()
    {
        _chipNum = 100;
        firstPlay = true;
    }
}

