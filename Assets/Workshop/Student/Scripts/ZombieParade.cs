using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class ZombieParade : OOPEnemy
    {
        // �� LinkedList 㹡�èѴ�����ǹ�ͧ�����ͻ���Է���Ҿ㹡������/ź
        // �� LinkedList 㹡�èѴ�����ǹ�ͧ�����ͻ���Է���Ҿ㹡������/ź
        private LinkedList<GameObject> Parade = new LinkedList<GameObject>();
        public int SizeParade = 3;
        int timer = 0;
        public GameObject[] bodyPrefab; // Prefab �ͧ��ǹ�ӵ�ǧ�
        public float moveInterval = 0.5f; // ��ǧ����㹡������͹��� (0.5 �Թҷ�)

        private Vector3 moveDirection;

        public override void SetUP()
        {
            base.SetUP();
            moveDirection = Vector3.up;
            // ����� Coroutine ����Ѻ�������͹���
            positionX = (int)transform.position.x;
            positionX = (int)transform.position.y;
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
        // Coroutine ����Ѻ�������͹�����Ъ�ͧ
        IEnumerator MoveParade()
        {      
            //0. ���ҧ��ǧ�
            Parade.AddFirst(this.gameObject);
            while (isAlive)
            {
                // 1. �֧��ǹ�á�ͧ���͡��
                LinkedListNode<GameObject> firstNode = Parade.First;
                GameObject firstPart = firstNode.Value;

                // 2. �֧��ǹ�ش���¢ͧ���͡��
                LinkedListNode<GameObject> lastNode = Parade.Last;
                GameObject lastPart = lastNode.Value;
             
                // 3. ź��ǹ�ش�����͡�ҡ LinkedList
                Parade.RemoveLast();

                // 5. ��˹����˹���з�ȷҧ�ͧ��ǹ���١����������
                // ������������˹觢ͧ��ǹ��ǧ� (����������͹��������ͤ���)
                int toX = 0;
                int toY = 0;

                bool isCollide = true;
                while (isCollide == true)
                {
                    moveDirection = RandomizeDirection();
                    toX = (int)(firstPart.transform.position.x + moveDirection.x);
                    toY = (int)(firstPart.transform.position.y + moveDirection.y);
                    isCollide = IsCollision(toX, toY);
                }
                //6. ����͹���
                mapGenerator.mapdata[positionX, positionY] = null;
                positionX = toX;
                positionY = toY;
                lastPart.transform.position = new Vector3(positionX, positionY, 0);
                lastPart.GetComponent<SpriteRenderer>().flipX = moveDirection == Vector3.right;
                mapGenerator.mapdata[positionX, positionY] = lastPart.GetComponent<Identity>();


                // 7. ������ǹ��鹡�Ѻ��������ǹ����ͧ�ͧ LinkedList
                // (��觡�����ǹ�á�ͧ�ӵ��)
                Parade.AddFirst(lastNode);
                // �͵�����ҷ���˹���͹�������͹�����駵���
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
            // 4. ��Ǩ�ͺ��觡մ��ҧ
            if (HasPlacement(x, y))
            {
                return true;
            }
            return false;
        }
        
        // �ѧ��ѹ����Ѻ������ǹ�ͧ�� (Grow)
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
