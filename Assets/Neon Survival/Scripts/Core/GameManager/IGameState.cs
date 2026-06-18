using System.Collections;
using UnityEngine;

public interface IGameState
{
    // Chuyển sang IEnumerator để GameManager có thể đợi (yield return) cho đến khi State khởi tạo xong
    IEnumerator Enter(); 
    void Update();
    IEnumerator Exit(); // Đợi dọn dẹp xong mới rời State
}
