using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneLoader.Load(GameScene.Cozy);
    }
}
