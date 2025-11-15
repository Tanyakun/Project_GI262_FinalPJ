using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class ZombieParade : OOPEnemy
    {
        // ร่างขบวนถูกจัดการด้วย LinkedList ของ GameObject (หัว -> หาง)
        // แต่ละองค์ประกอบเป็นส่วนของร่างกายที่ตามหัว
        private LinkedList<GameObject> Parade = new LinkedList<GameObject>();
        public int SizeParade = 3;
        int timer = 0;
        public GameObject[] bodyPrefab; // Prefab ที่ใช้สร้างส่วนของร่างกาย
        public float moveInterval = 0.5f; // เวลาระหว่างการเคลื่อน (วินาที)

        private Vector3 moveDirection;

        public override void SetUP()
        {
            base.SetUP();
            moveDirection = Vector3.up;
            // เริ่ม Coroutine ที่ควบคุมการเคลื่อนของขบวน
            positionX = (int)transform.position.x;
            positionY = (int)transform.position.y;
            StartCoroutine(MoveParade());
        }
        private Vector3 RandomizeDirection()
        {
            List<Vector3> possibleDirections = new List<Vector3>
            {
                Vector3.up,
                Vector3.down,
                Vector3.left,
                Vector3.right
            };

            return possibleDirections[Random.Range(0, possibleDirections.Count)];
        }
        // Coroutine ที่อัปเดตการเคลื่อนของขบวนในแต่ละจังหวะ
        IEnumerator MoveParade()
        {      
            // 0. เพิ่มหัว (GameObject นี้) เป็นส่วนแรก
            Parade.AddFirst(this.gameObject);
            while (isAlive)
            {
                // 1. ดึงส่วนแรก (หัว)
                LinkedListNode<GameObject> firstNode = Parade.First;
                GameObject firstPart = firstNode.Value;

                // 2. ดึงส่วนสุดท้าย (หาง)
                LinkedListNode<GameObject> lastNode = Parade.Last;
                GameObject lastPart = lastNode.Value;

                // จำตำแหน่งหางเดิมเอาไว้ก่อน
                int tailOldX = (int)lastPart.transform.position.x;
                int tailOldY = (int)lastPart.transform.position.y;
             
                // 3. เอาส่วนสุดท้ายออกจากรายการ (ส่วนนี้จะย้ายไปเป็นหัว)
                Parade.RemoveLast();

                // 4. หาตำแหน่งเป้าหมายที่จะย้ายหัว
                // เลือกทิศทางแบบสุ่มและตรวจการชน
                int toX = 0;
                int toY = 0;
                
                // ป้องกันการเกิด infinity loop โดยการ ตั้งจำนวนครั้งในการหาทิศทางเอาไว้อย่างจำกัด
                bool foundMove = false;
                for (int attempt = 0; attempt < 8; attempt++)
                {
                    moveDirection = RandomizeDirection();
                    toX = (int)(firstPart.transform.position.x + moveDirection.x);
                    toY = (int)(firstPart.transform.position.y + moveDirection.y);
                    if (!IsCollision(toX, toY))
                    {
                        foundMove = true;
                        break;
                    }
                }

                // ถ้าไม่พบตำแหน่งที่สามารถย้ายได้ ให้คืนหางกลับไปที่เดิม
                if (!foundMove)
                {
                    Parade.AddLast(lastNode);
                    yield return new WaitForSeconds(moveInterval);
                    continue;
                }

                // ล้างช่องเก่าของหางจากข้อมูลแผนที่
                mapGenerator.mapdata[tailOldX, tailOldY] = null;

                // 5. ทำการย้าย: ล้างช่องของหางเก่า แล้ววางหางที่ย้ายไปตำแหน่งหัวใหม่
                positionX = toX;
                positionY = toY;
                lastPart.transform.position = new Vector3(positionX, positionY, 0);
                lastPart.GetComponent<SpriteRenderer>().flipX = moveDirection == Vector3.right;
                mapGenerator.mapdata[positionX, positionY] = lastPart.GetComponent<Identity>();

                // 6. แทรกส่วนที่ย้ายแล้วเป็นหัวของ LinkedList
                Parade.AddFirst(lastNode);
                // ถ้าขนาดขบวนยังน้อยกว่าเป้าหมาย ให้จัดการตัวนับ size
                if (Parade.Count < SizeParade) {
                    timer++;
                    if (timer > 3)
                    {
                        //Grow();
                        timer = 0;
                    }
                }
                yield return new WaitForSeconds(moveInterval);
            }
        }
        private bool IsCollision(int x, int y)
        {
            // คืนค่า true ถ้าตำแหน่ง (x,y) ถูกวางสิ่งของไปแล้วหรือเป็นอุปสรรค
            if (HasPlacement(x, y))
            {
                return true;
            }
            return false;
        }
        //private void Grow()
        //{
            //GameObject newPart = Instantiate(bodyPrefab[0]);
            // ��˹����˹�������鹢ͧ��ǹ����������������ǡѺ��ǹ�ش���¢ͧ��
            //GameObject lastPart = Parade.Last.Value;
            //newPart.transform.position = lastPart.transform.position;
            //mapGenerator.SetUpItem(positionX,positionY, newPart, mapGenerator.enemyParent, mapGenerator.enemy);
            //newPart.transform.rotation = lastPart.transform.rotation;
            // ������ǹ��������� Linked List
            //Parade.AddLast(newPart);
        //}
    }
}
