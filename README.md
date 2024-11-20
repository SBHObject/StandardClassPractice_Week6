# StandardClassPractice_Week6
 스탠다드반 꾸준실습 6주차

## Q1

### O/X 퀴즈
- 앵커와 피벗은 같은 기능을 한다. (O/X)
  - X
  - 앵커는 캔버스의 각 꼭지점 네개의점에서 UI의 각 꼭지점이 얼마나 떨어져 있을지를 정한다.
  - 피벗은 해당 UI의 기준점(원점)의 워치를 정한다.  
- 피벗을 왼쪽 상단으로 설정하면, UI 요소는 화면의 왼쪽 상단을 기준으로 위치가 고정된다. (O/X)
  - X
  - 피벗을 왼쪽 상단에 설정하면 요소를 그리는 기준이 왼쪽 상단이 되어 Width를 늘릴경우 UI의 오른쪽이 길어지며, Height를 늘릴경우 UI의 아래쪽으로 길어지게된다.
  - 왼쪽 상단을 기준으로 위치를 고정시켜주는것은 앵커의 역할이다
- 피벗을 UI 요소의 중심에 설정하면, 회전 시 UI 요소가 중심을 기준으로 회전한다. (O/X)
  - O
  - 피벗은 UI의 기준점을 정하게 됨, 중심에 피벗이 있으면 UI의 중심을 기준으로 회전한다.
  - 피벗이 왼쪽상단에 있을경우 Y축 회전시 왼쪽 세로축 기준으로 회전하고, X 축 회전시 상단 가로축 기준으로 회전, Z 축 회전시 왼쪽 위 꼭지점을 기준으로 회전하게된다.
 
### 생각해보기
- 게임의 상단바와 같이 화면에 특정 영역에 꽉 차게 구성되는 UI와 화면의 특정 영역에 특정한 크기로 등장하는 UI의 앵커 구성이 어떻게 다른 지 설명해보세요.
  - 특정 영역에 꽉 차게 구성할경우, 가로축으로 꽉 차야하면 앵커의 X값 Min 을 0로 Max를 1으로 설정하고 세로축으로 차야하면 Y의 Min 을 0으로, Max 를 1으로 설정하게됨
  - 특정 영역에 특정한 크기로 등장하는 UI의 경우, UI가 생성되는 위치에 따라 앵커의 위치를 결정하게됨, 우측 하단을 기준으로 생성되야 할 경우, X의 Min, Max를 1, Y의 Min, Max 를 0으로 설정하게됨
  - 특정한 위치에 지정한 크기로 나타날경우 앵커를 한 점에 모으게되며, 특정한 영역에 차도록 구성해야할 경우 앵커를 화면 양 끝으로 분리하게됨
- 돌아다니는 몬스터의 HP 바와 늘 고정되어있는 플레이어의 HP바는 Canvas 컴포넌트의 어떤 설정이 달라질 지 생각해보세요.
  - 돌아다니는 몬스터의 경우, Canvas를 WorldSpace 로 설정한 뒤, 몬스터의 머리 위 등에 Canvas의 위치를 지정하여 체력바를 표시하게됨
  - 플레이어의 체력바는 시점, 컨셉에 따라 달라짐
    - 플레이어가 3인칭 시점, 탑다운 방식으로 존재할경우 몬스터처럼 캐릭터 상단에 WorldSpace 로 Canvas를 사용하여 표기할 수 있음
    - 1인칭 시점일 경우 HUD 형식의 UI를 사용하게 되어 Screen Space - Overlay, Screen Space - Camera를 사용하여 카메라에 체력바를 직접 보여주는 방식을 선택하게됨

### 확장문제

- Resume Game이라는 텍스트가 들어있는 버튼을 만들고, 그 버튼을 누르면 게임이 재개되게 하세요.

![일시정지](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/9e25a6fc-4970-49e6-953c-0df483865e92/2024-11-19_12-24-41.gif?table=block&id=14343507-6cd3-80e9-91d1-e4b05dae50a1&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1732075200000&signature=zXuPqTfvrIZVYw7QJR04Br_StQYFjeY4bnjyg5tvu8I)

- ESC 키를 누르면 일시정지
- Time.TimeScale = 0 으로 하는 방법으로 일시정지를 구현
- Resume 버튼을 누르거나, ESC를 다시 누르면 이어하기

## Q2

### O/X 퀴즈

- 코루틴은 비동기 작업을 처리하기위해 사용된다.
  - O
  - 실제 처리는 비동기 방식이 아니지만, 비동기와 유사하게 작동하여 비동기처리를 하기 위해 사용됨
- yield return new WaitForSeconds(1);는 코루틴을 1초 동안 대기시킨다.
  - O
  - WaitForSeconds 는 () 내부 숫자에 해당하는 초만큼 대기함.
  - WaitForFrame 등을 사용하여 특정 프레임동안 대기하는 방식을 사용할 수도 있음
- 코루틴은 void를 반환하는 메소드의 형태로 구현된다
  - X
  - 반환값이 IEnumerator 인 메소드의 형태로 구현됨

### 생각해보기

- 코루틴을 이미 실행중이라면 추가로 실행하지 않으려면 어떻게 처리해주면 될까요?
  - Coroutine 변수를 필드에 만들어준 뒤, 이 필드 값이 null일경우에만 새로운 코루틴을 실행하도록 구현한다.
  - Coroutine nowCoroutine 변수를 만들었다고 가정할 때, nowCoroutine = StartCoroutine(코루틴 이름) 으로 사용하여 변수에 코루틴을 채워준다
  - 코루틴의 마지막 열에 nowCoroutine = null 을 하여 코루틴이 종료되면 이후 다음 코루틴이 실행될 수 있도록 만들어준다.
- 코루틴 실행 중 게임오브젝트가 파괴되더라도 코루틴의 실행이 정상적으로 지속될까요?
  - 코루틴은 MonoBehaviour 스크립트를 기반으로 실행되기 때문에, 코루틴이 담긴 게임오브젝트가 파괴되면 코루틴이 멈추게된다.
  - 이때, 해당 코루틴을 호출하게되면 오류가 발생하게된다.
 
### 확장문제

- 웨이브 10, 30, 50, …에 부여되는 랜덤 디버프를 만들어봅시다.
  
![웨이브 시작](https://file.notion.so/f/f/f3d7f86c-cdab-4d84-9092-b767f79f7186/bcc6eff7-1170-4abd-a3b3-8e718b867b74/2024-11-20_12-13-22.gif?table=block&id=14443507-6cd3-8034-95a8-dcae8cc96071&spaceId=f3d7f86c-cdab-4d84-9092-b767f79f7186&expirationTimestamp=1732161600000&signature=pRkNwrVPn_WD-iI3EPxb-jMjOFAaC30Qd0dycCdPZn8)

- GameManager에서 웨이브를 관리중이기 때문에, GameManager에 데미지를 주는 로직 생성
- 현재 데미지를 받으면 무적시간이 주어지지만, 이 디버프로 인해 무적시간을 받는것은 어울리지 않다고 판단하여 HealthSystem에 무적시간 없이 데미지를 주는 메서드 추가
- 임시로 모든 웨이브에서 데미지를 받도록 설정하여 확인한 결과 웨이브 시작과 동시에 데미지를 받는것을 확인할 수 있음
- 해당 메서드를 현재 웨이브가 10일때 작동하는 메서드에 추가(포션 제공 메서드 위치)

## Q3

### O/X 퀴즈

- 추상 클래스는 new를 통해 인스턴스화(instantiation)할 수 없다.
  - O
  - 추상 클래스는 하위 클래스를 필수로 하는 클래스, 객체를 생성할 수 없는클래스다.
  - 추상클래스를 상속받는 하위 클래스를 통해서 객체를 생성하게된다.
- 추상 클래스는 다른 클래스처럼 일반 메서드와 속성을 포함할 수 있다.
  - O
  - 추상 클래스에 일반 메서드와 속성을 선언한 뒤, 일반 메서드가 추상메서드를 호출할경우 자식 클래스에서 재정의된 추상 메서드를 추상클래스의 일반 메서드가 호출하는 식으로 사용된다.
- 추상 클래스를 상속받은 클래스는 추상 클래스의 모든 추상 메서드를 구현해야 한다.
  - O
  - 추상 클래스에 있는 모든 추상메서드를 구현해야한다.
- C#에서 한 클래스는 여러 개의 추상 클래스를 상속받을 수 있다.
  - X
  - 추상 클래스도 클래스이기때문에, 단일 상속을 원칙으로한다.
  - 추가적인 내용을 상속받고싶으면 인터페이스를 이용해야한다.
 
### 생각해보기

- 추상 클래스를 사용하지 않고 동일한 기능을 구현하려면 어떤 문제가 발생할 수 있는지 설명해보세요.
  - 동일한 기능이지만, 다른 클래스 객체를 생성한 뒤, 해당 기능을 메서드에서 호출해야한다.
  - 해당 기능을 필요로하는 클래스가, 기능을 가진 각자의 클래스의 객체를 생성하여 메서드 기능을 사용하게 되고, 어떤 클래스의 메서드를 사용할지 결정하는 연산이 추가되게 된다.
