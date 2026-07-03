# Neon Survivor

![Game Banner](https://via.placeholder.com/800x400/0A0A2A/00FFCC?text=Neon+Survivor) <!-- Thay bằng ảnh thực tế khi có -->

**Neon Survivor** là một tựa game **Action Roguelike Bullet Heaven (Wave Survival)** theo phong cách top-down 2D, lấy cảm hứng từ Vampire Survivors. Người chơi sẽ sống sót qua vô số đợt quái vật trong môi trường neon huyền ảo, liên tục nâng cấp và mở rộng sức mạnh để chinh phục những thử thách ngày càng khốc liệt.

---

## 🎮 Giới thiệu

**Neon Survivor** mang đến trải nghiệm bắn đạn "điên cuồng" với hàng trăm kẻ địch cùng lúc trên màn hình. Game tập trung vào:

- **Core Loop** mượt mà: Di chuyển né tránh → Tự động tấn công → Nhặt EXP → Lên cấp → Chọn kỹ năng mạnh mẽ.
- Hệ thống **Roguelike** phong phú với hàng loạt kỹ năng và nâng cấp ngẫu nhiên.
- **Tối ưu hiệu năng** mạnh mẽ để duy trì hiệu suất ổn định khi có 200–500 quái vật cùng lúc.

### Đặc điểm nổi bật

- **Phong cách Neon Cyberpunk** bắt mắt
- **Hệ thống kỹ năng đa dạng** (đạn tự động, sét, laser, bão đạn...)
- **Wave Survival** tăng độ khó liên tục + Boss
- **Tối ưu hóa cao**: Object Pooling, Spatial Partitioning, Addressables
- **Kiến trúc code sạch**: MVC/MVP, Observer, State Pattern, Factory + ScriptableObjects

---

## 🛠 Công nghệ & Kiến trúc

Dự án được xây dựng để thể hiện kinh nghiệm **Unity Developer 2 năm** với trọng tâm **Clean Architecture**, **Scalability** và **Performance Optimization**.

### Kiến trúc chính
- **Design Patterns**: MVC/MVP, Observer (Event/Delegate), State Pattern (FSM), Factory Pattern
- **Data Management**: Scriptable Objects (PlayerStatsSO, EnemyDataSO, SkillSO...), Save/Load JSON
- **Core Systems**:
  - Generic **Object Pooling** cho đạn, quái, EXP, VFX
  - Physics tối ưu (OverlapCircleNonAlloc thay vì Collision callback)
  - Addressables cho asset loading
  - Unity New Input System

### Công nghệ sử dụng
- **Unity**: 2022.3 LTS + URP (2D)
- **Ngôn ngữ**: C#
- **Input**: Unity New Input System
- **Save**: JsonUtility / Newtonsoft.Json

---

## 📋 Lộ trình phát triển (Milestones)

### Mốc 1: Kiến trúc & Core Player
- Project 2D URP + cấu trúc thư mục chuẩn
- Di chuyển top-down mượt mà
- Game State Machine (FSM)
- Scriptable Object cơ bản

### Mốc 2: Tối ưu & Kẻ địch
- Generic ObjectPoolManager
- Enemy AI + Wave Spawner
- Spawn ngoài tầm nhìn camera

### Mốc 3: Hệ thống Chiến đấu
- Auto-attack + Auto-aim
- HealthComponent + Observer Pattern
- Tối ưu vật lý

### Mốc 4: Roguelike Elements
- Hệ thống EXP hút tự động
- Level Up + chọn kỹ năng ngẫu nhiên (3 lựa chọn)

### Mốc 5: UI & Save/Load
- UI theo MVC/MVP, Static & Dynamic Canvas
- High Score + Save JSON

### Mốc 6: Đánh bóng & Hoàn thiện
- Addressables, Audio Pool, Custom Editor
- Profiling, tối ưu GC, Build production

---

## 📸 Screenshots

*(Đang phát triển - sẽ cập nhật ảnh gameplay thực tế)*

<!-- 
![Gameplay 1](path/to/screenshot1.png)
![Level Up Screen](path/to/screenshot2.png)
![Massive Wave](path/to/screenshot3.png)
-->

---

## 🚀 Cách chạy project

1. Clone repository:
   ```bash
   git clone https://github.com/Semicolirt/neon-survivor.git
