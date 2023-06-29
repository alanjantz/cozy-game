using UnityEngine;

public static class InputUtils
{
    public static bool IsMovingLeft() => Input.GetAxisRaw("Horizontal") < 0;
    public static bool IsMovingRight() => Input.GetAxisRaw("Horizontal") > 0;
    public static bool IsIdle() => Input.GetAxisRaw("Horizontal") == 0;
}
