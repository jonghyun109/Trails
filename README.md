<div align="center">


# [<img width="100" height="100" alt="Youtube_logo" src="https://github.com/user-attachments/assets/995e39b4-fafe-4e74-ac05-868ae72fde6a" />](https://www.youtube.com/watch?v=mAKaPSP8ITU) TRAILS
### 친구와 함께 하며 스토리의 비밀을 풀어가는 스토리 협동 게임!

</div>





<table>
  <tr>
    <td align="center" width="33%">
      <img src="https://github.com/user-attachments/assets/b5fcaaca-80a7-4c0c-ad30-049e09331e9f" alt="게임 로비 화면" width="400" />
    </td>
    <td align="center" width="33%">
      <img src="https://github.com/user-attachments/assets/dd25c53c-113e-424c-8ebb-5dbb5e9bddb9" alt="보스전 화면" width="400" />
    </td>
    <td align="center" width="33%">
      <img src="https://github.com/user-attachments/assets/97a723cf-5aca-4520-9b46-35ff1295ffb7" alt="승리 화면" width="400" />
    </td>
  </tr>
</table>

<br>

⭐ **봄, 여름, 가을, 겨울 챕터 중 여름 챕터의 보스 방을 소개**  
⭐ **챕터마다 플랫포머, 쿼터뷰, 탑뷰 등 다양한 플랫폼 경험을 제공하는 것이 특징**

<br>

---

</div>

<br>
<br>

## 📋 목차

- [게임 소개](#-게임-소개)
- [주요 스크립트](#-주요-스크립트)
  - [게임 관리](#-게임-관리)
  - [플레이어 시스템](#-플레이어-시스템)
  - [보스 시스템](#-보스-시스템)
  - [스킬 시스템](#-스킬-시스템)
  - [멀티플레이어](#-멀티플레이어)
  - [UI 시스템](#-ui-시스템)
  - [기타 시스템](#-기타-시스템)
- [기술 스택](#-기술-스택)
- [개발자](#-개발자)

<br>
<br>

---

## 🎯 게임 소개

**TRAILS**는 친구와 함께 협동하여 스토리의 비밀을 풀어가는 멀티플레이어 협동 게임입니다.  
각 챕터마다 다른 플랫폼 경험(플랫포머, 쿼터뷰, 탑뷰)을 제공하여 다양한 게임플레이를 즐길 수 있습니다.

<br>
<br>

---

## 💻 주요 스크립트

### 🎮 게임 관리

#### [GameManager.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/GameManager.cs)
💡 **게임 전체 흐름 관리**

- **주요 기능:**
  - 보스 스폰 관리 (마스터 클라이언트만)
  - 플레이어 스폰 및 초기화
  - 카메라 설정 (Cinemachine)
  - 플레이어별 UI 활성화
  - 보스 HP 슬라이더 연결

- **주요 메서드:**
  - `SpawnBossIfMaster()`: 마스터 클라이언트가 보스를 생성
  - `SpawnPlayer()`: 플레이어를 ActorNumber에 따라 스폰

<br>

#### [Manager.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/Manager.cs)
📌 **빈 매니저 클래스** (현재 미사용)

<br>

---

### 👤 플레이어 시스템

#### [WalkerBase.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/WalkerBase.cs)
💡 **플레이어 베이스 클래스** - 모든 플레이어의 기본 기능 제공

- **주요 기능:**
  - HP 관리 (기본 6)
  - 이동 시스템 (IWalker 인터페이스 구현)
  - 데미지 처리 및 무적 시간
  - 사망 및 리스폰 시스템
  - HP UI 동기화 (Photon RPC)

- **주요 메서드:**
  - `Walk(Vector3 dir)`: 이동 처리
  - `TakeDamage(int amount)`: 데미지 받기
  - `ForceRespawn()`: 강제 리스폰
  - `IsDead()`: 사망 상태 확인

<br>

#### [Player3DContoller.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/Player3DContoller.cs)
💡 **3D 플레이어 컨트롤러** - 쿼터뷰/탑뷰 전용

- **주요 기능:**
  - 3D 공간 이동 (WASD)
  - 점프 시스템
  - 달리기 (LeftShift)
  - 애니메이션 제어
  - 방향 전환 (스케일 반전)

- **주요 메서드:**
  - `Jump()`: 점프 실행
  - `CheckGround()`: 지면 체크
  - `Move(Vector3 movement)`: Rigidbody 기반 이동

<br>

#### [PlayerMove.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/PlayerMove.cs)
💡 **2D 플레이어 컨트롤러** - 플랫포머 전용

- **주요 기능:**
  - 2D 좌우 이동
  - 점프 시스템
  - 달리기 (LeftShift)
  - 지면 감지

- **주요 메서드:**
  - `Move(Vector3 movement)`: Rigidbody2D 기반 이동

<br>

#### [IWalker.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/IWalker.cs)
📌 **이동 인터페이스**

- 이동 속도, 이동 가능 여부, 방향 등을 정의하는 인터페이스

<br>

---

### 👹 보스 시스템

#### [BossCommand.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/BossCommand.cs)
💡 **보스 패턴 시스템** - 커맨드 패턴 기반 AI

- **주요 기능:**
  - 패턴 큐 시스템 (Queue<ICommand>)
  - HP 관리 및 슬라이더 동기화
  - 타겟 추적 (가장 가까운 플레이어)
  - 패턴 실행 및 완료 처리
  - Photon 네트워크 동기화

- **주요 메서드:**
  - `SetupPattern()`: 패턴 설정 (추상 메서드)
  - `NextAction()`: 다음 패턴 실행
  - `BossTakeDamage(int amount)`: 보스 데미지 처리
  - `UpdateTarget()`: 가장 가까운 플레이어를 타겟으로 설정

- **패턴 클래스:**
  - `IdleCommand`: 대기 패턴
  - `ChaseCommand`: 추적 패턴
  - `DashSkillCommand`: 돌진 스킬 패턴
  - `JumpAttackCommand`: 점프 공격 패턴

<br>

#### [Boss_Toad.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/Boss_Toad.cs)
💡 **개구리 보스 구현**

- **주요 기능:**
  - 이동 속도: 3.5
  - 최대 HP: 50
  - 패턴: Idle → Chase → DashSkill
  - 실시간 타겟 추적 및 회전

- **주요 메서드:**
  - `SetupPattern()`: 개구리 보스 전용 패턴 설정

<br>

#### [IBossDamageable.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/IBossDamageable.cs)
📌 **보스 데미지 인터페이스**

- 보스가 데미지를 받을 수 있도록 하는 인터페이스

<br>

---

### ⚡ 스킬 시스템

#### [Skill.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/Skill.cs)
💡 **스킬 오브젝트** - 폭발형 스킬

- **주요 기능:**
  - 타겟 위치로 이동 후 폭발
  - 번개에 의한 즉시 폭발 지원
  - 보스 데미지 처리
  - 이펙트 풀링 연동
  - Photon 네트워크 동기화

- **주요 메서드:**
  - `SkillBoom(Vector3 targetPos)`: 스킬 발사 (RPC)
  - `TriggerExplosion(bool isLightning)`: 폭발 트리거
  - `Explode(int effectId)`: 폭발 처리 및 데미지 적용

<br>

#### [SkillAimer.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/SkillAimer.cs)
💡 **스킬 조준 시스템**

- **주요 기능:**
  - 마우스 클릭으로 조준
  - 범위 표시기 (Range Indicator)
  - 최대 사거리 제한 (8f)
  - 쿨타임 시스템 (3초)
  - 플레이어별 다른 스킬:
    - 플레이어 1 (마스터): 폭발 스킬 (Boom)
    - 플레이어 2: 번개 스킬 (Lightning)

- **주요 메서드:**
  - `AimAndFire()`: 조준 및 발사 코루틴

<br>

#### [Lightning.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/Lightning.cs)
💡 **번개 스킬**

- **주요 기능:**
  - 경고 이펙트 표시 (1초)
  - 번개 타격 이펙트
  - 범위 내 폭탄 즉시 폭발 트리거
  - 이펙트 풀링 사용

- **주요 메서드:**
  - `StrikeLightning(Vector3 targetPos)`: 번개 타격 (RPC)

<br>

---

### 🌐 멀티플레이어

#### [LobbyManager.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/LobbyManager.cs)
💡 **로비 및 멀티플레이어 관리**

- **주요 기능:**
  - Photon 네트워크 연결
  - 방 생성 (랜덤 5자리 코드)
  - 방 입장 (코드 입력)
  - 플레이어 수 표시
  - 게임 시작 버튼 (2명일 때만 활성화)
  - 씬 전환

- **주요 메서드:**
  - `OnCreateRoomButton()`: 방 생성
  - `OnJoinRoomButton()`: 방 입장
  - `SceneChange()`: 게임 씬으로 전환
  - `MoveTitleUp()`: 타이틀 애니메이션

<br>

#### [RespawnManager.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/RespawnManager.cs)
💡 **리스폰 관리 시스템**

- **주요 기능:**
  - 플레이어 사망 시 리스폰 체크
  - 3초 대기 후 리스폰
  - 모든 플레이어 사망 시 로비로 복귀
  - 마스터 클라이언트가 관리

- **주요 메서드:**
  - `CheckRespawn()`: 리스폰 체크 시작
  - `AnyAlivePlayer()`: 생존 플레이어 확인
  - `RPC_TriggerLocalRespawn()`: 리스폰 트리거 (RPC)

<br>

---

### 🖼️ UI 시스템

#### [HPUI.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/HPUI.cs)
💡 **HP UI 관리**

- **주요 기능:**
  - 플레이어별 HP 하트 표시
  - 하트 스프라이트 변경 (0~2 HP)
  - Photon RPC로 동기화

- **주요 메서드:**
  - `TryUpdateHp(int actorNumber, int newHp)`: HP 업데이트 시도
  - `UpdateHearts(int syncedHp)`: 하트 UI 업데이트 (RPC)

<br>

#### [Ending.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/Ending.cs)
💡 **게임 종료 처리**

- **주요 기능:**
  - 보스 HP가 0이 되면 승리 처리
  - 승리 패널 페이드 인 애니메이션
  - 승리 오브젝트 활성화

- **주요 메서드:**
  - `Win()`: 승리 체크 및 처리
  - `Paneltransparency()`: 패널 투명도 애니메이션

<br>

---

### 🎥 기타 시스템

#### [CameraMove.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/CameraMove.cs)
💡 **카메라 뷰 전환**

- **주요 기능:**
  - F키로 뷰 전환
  - 탑뷰 ↔ 플랫포머 뷰
  - Cinemachine 가상 카메라 우선순위 변경
  - 플레이어 회전 애니메이션

- **주요 메서드:**
  - `SmoothRotate()`: 부드러운 회전 애니메이션

<br>

#### [ObjectPool.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/ObjectPool.cs)
💡 **오브젝트 풀링 시스템**

- **주요 기능:**
  - 이펙트 오브젝트 풀링
  - 메모리 최적화
  - 싱글톤 패턴

- **주요 메서드:**
  - `GetEffect(int index)`: 풀에서 이펙트 가져오기
  - `SpawnEffect(int index, Vector3 position)`: 이펙트 스폰

<br>

#### [TeleportWall.cs](https://github.com/jonghyun109/Trails/blob/Develop/Assets/Scripts/TeleportWall.cs)
💡 **텔레포트 벽**

- **주요 기능:**
  - 플레이어가 벽에 닿으면 리스폰 위치로 텔레포트
  - 텔레포트 시 데미지 (6)
  - 리스폰 체크 트리거

<br>
<br>

---

## 🛠️ 기술 스택

- **게임 엔진:** Unity
- **멀티플레이어:** Photon PUN (Photon Unity Networking)
- **카메라:** Cinemachine
- **언어:** C#
- **플랫폼:** Windows

<br>
<br>

---

## 👨‍💻 개발자

<a href="https://github.com/jonghyun109">
  <img src="https://img.shields.io/badge/GitHub-jonghyun109-181717?style=flat-square&logo=GitHub" alt="GitHub Profile" />
</a>

<br>
<br>

---

<div align="center">

**TRAILS** - 친구와 함께하는 협동 모험 게임 🎮

</div>
