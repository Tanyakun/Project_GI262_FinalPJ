using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solution
{
    public class OOPMapGenerator : MonoBehaviour
    {
        [Header("Set MapGenerator")]
        public int X;
        public int Y;

        [Header("Set Player")]
        public OOPPlayer player;
        public Vector2Int playerStartPos;

        [Header("Set Exit")]
        public OOPExit Exit;

        [Header("Set Wall")]
        public Identity Wall; // sentinel สำหรับขอบนอกกริด (อยู่นอก mapdata)

        [Header("Set Prefab")]
        public GameObject[] floorsPrefab;
        public GameObject[] wallsPrefab;        // ขอบนอกกริด
        public GameObject[] demonWallsPrefab;   // สิ่งกีดขวางภายในกริด
        public GameObject[] itemsPrefab;
        public GameObject[] collectItemsPrefab;
        public GameObject[] EnemyPrefab;
        public GameObject[] SkillPrefab;

        [Header("Set Transform")]
        public Transform floorParent;
        public Transform wallParent;
        public Transform itemParent;
        public Transform enemyParent;

        [Header("Set object Count")]
        public int obsatcleCount;
        public int itemPotionCount;
        public int colloctItemCount;
        public int EnemyCount;
        public int SkillCount;

        public Identity[,] mapdata;

        [Header("Enemy on Map")]
        public List<OOPEnemy> EnemysOnMap = new List<OOPEnemy>();

        // block types
        [HideInInspector] public string empty = "";
        [HideInInspector] public string demonWall = "demonWall";
        [HideInInspector] public string potion = "potion";
        [HideInInspector] public string bonuesPotion = "bonuesPotion";
        [HideInInspector] public string exit = "exit";
        [HideInInspector] public string playerOnMap = "player";
        [HideInInspector] public string collectItem = "collectItem";
        [HideInInspector] public string enemy = "enemy";

        // --- OPTIONS ---
        [Header("Path Guard Options")]
        [Tooltip("ถ้า true ศัตรูจะถือว่าเป็นตัวกีดขวางเส้นทางด้วย")]
        public bool enemiesBlockPath = false;

        [Tooltip("ถ้า true จะสร้างเส้นทางสั้นสุด (Backbone) จาก Start ไป Exit แล้วล็อกไม่ให้วางสิ่งกีดขวางทับ")]
        public bool buildBackboneFirst = true;

        // เก็บช่องของ backbone ที่ห้ามวางของทับ
        private HashSet<Vector2Int> reservedBackbone = new HashSet<Vector2Int>();

        // ---------------- LIFECYCLE ----------------
        private void Awake()
        {
            CreateMap();
        }

        void Start()
        {
            StartCoroutine(SetUPMap());
        }

        IEnumerator SetUPMap()
        {
            SetUpPlayer();
            SetUpExit();

            if (buildBackboneFirst)
                BuildBackbonePath();

            // 1) วางสิ่งกีดขวางแบบมี Path-Guard
            PlaceObstaclesWithPathGuard(obsatcleCount, demonWallsPrefab, wallParent, demonWall);

            // 2) วางของอื่น ๆ ที่ไม่กันทาง (หรือกันทางก็ได้แต่ไม่เช็คเส้น)
            PlaceItemsOnMap(itemPotionCount, itemsPrefab, itemParent, potion);
            PlaceItemsOnMap(colloctItemCount, collectItemsPrefab, itemParent, collectItem);
            PlaceItemsOnMap(SkillCount, SkillPrefab, itemParent, collectItem);
            PlaceItemsOnMap(EnemyCount, EnemyPrefab, enemyParent, enemy);

            yield return new WaitForSeconds(0.5f);
            RandomDamageToListEnemies();
        }

        // ---------------- MAP CREATION ----------------
        private void CreateMap()
        {
            mapdata = new Identity[X, Y];

            for (int x = -1; x < X + 1; x++)
            {
                for (int y = -1; y < Y + 1; y++)
                {
                    if (x == -1 || x == X || y == -1 || y == Y)
                    {
                        int r = Random.Range(0, wallsPrefab.Length);
                        GameObject obj = Instantiate(wallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
                        obj.transform.parent = wallParent;
                        obj.name = "Wall_" + x + ", " + y;
                    }
                    else
                    {
                        int r = Random.Range(0, floorsPrefab.Length);
                        GameObject obj = Instantiate(floorsPrefab[r], new Vector3(x, y, 1), Quaternion.identity);
                        obj.transform.parent = floorParent;
                        obj.name = "floor_" + x + ", " + y;
                        mapdata[x, y] = null; // เริ่มว่าง
                    }
                }
            }
        }

        private void SetUpPlayer()
        {
            player.mapGenerator = this;
            player.positionX = playerStartPos.x;
            player.positionY = playerStartPos.y;
            player.transform.position = new Vector3(playerStartPos.x, playerStartPos.y, -0.1f);
            mapdata[playerStartPos.x, playerStartPos.y] = player;
        }

        private void SetUpExit()
        {
            // วาง Exit มุมขวาบน (X-1, Y-1)
            mapdata[X - 1, Y - 1] = Exit;
            Exit.positionX = X - 1;
            Exit.positionY = Y - 1;
            Exit.mapGenerator = this;
            Exit.transform.position = new Vector3(X - 1, Y - 1, 0);
        }

        // ---------------- PUBLIC HELPERS ----------------
        public Identity GetMapData(float x, float y)
        {
            if (x >= X || x < 0 || y >= Y || y < 0)
                return Wall; // sentinel: ขอบนอก

            return mapdata[(int)x, (int)y];
        }

        public void SetUpItem(int x, int y, GameObject[] _itemsPrefab, Transform parrent, string _name)
        {
            int r = Random.Range(0, _itemsPrefab.Length);
            GameObject obj = Instantiate(_itemsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = parrent;

            var id = obj.GetComponent<Identity>();
            mapdata[x, y] = id;
            id.positionX = x;
            id.positionY = y;
            id.mapGenerator = this;
            if (_name != collectItem) id.Name = _name;

            if (_name == enemy)
                EnemysOnMap.Add(obj.GetComponent<OOPEnemy>());

            obj.name = $"Object_{mapdata[x, y].Name} {x}, {y}";
        }

        public void SetUpItem(int x, int y, GameObject _itemsPrefab, Transform parrent, string _name)
        {
            _itemsPrefab.transform.parent = parrent;
            var id = _itemsPrefab.GetComponent<Identity>();

            mapdata[x, y] = id;
            id.positionX = x;
            id.positionY = y;
            id.mapGenerator = this;
            if (_name != collectItem) id.Name = _name;

            if (_name == enemy)
                EnemysOnMap.Add(_itemsPrefab.GetComponent<OOPEnemy>());

            _itemsPrefab.name = $"Object_{mapdata[x, y].Name} {x}, {y}";
        }

        public OOPEnemy[] GetEnemies() => EnemysOnMap.ToArray();

        public void MoveEnemies()
        {
            foreach (var enemy in EnemysOnMap)
                enemy.RandomMove();
        }

        public void RandomDamageToListEnemies()
        {
            Debug.Log($"Damage to {EnemysOnMap.Count} EnemysOnMap ");
            foreach (var enemy in EnemysOnMap)
            {
                int damage = Random.Range(1, 15);
                enemy.TakeDamage(damage);
            }
        }

        // ---------------- ITEM PLACEMENT (NON-BLOCKING) ----------------
        private void PlaceItemsOnMap(int count, GameObject[] prefab, Transform parent, string itemType, System.Action onComplete = null)
        {
            int placedCount = 0;
            int preventInfiniteLoop = 2000;

            while (placedCount < count && preventInfiniteLoop-- > 0)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);

                // กันทับ Player/Exit
                if ((x == playerStartPos.x && y == playerStartPos.y) || (x == X - 1 && y == Y - 1))
                    continue;

                // ถ้าเปิด backbone mode กันทับเส้นทางหลัก
                if (reservedBackbone.Contains(new Vector2Int(x, y)))
                    continue;

                if (mapdata[x, y] == null)
                {
                    SetUpItem(x, y, prefab, parent, itemType);
                    placedCount++;
                }
            }

            if (placedCount < count)
                Debug.LogWarning($"Could not place all {itemType}. placed {placedCount}/{count}");

            onComplete?.Invoke();
        }

        // ---------------- OBSTACLE PLACEMENT (BLOCKING WITH PATH-GUARD) ----------------
        private void PlaceObstaclesWithPathGuard(int count, GameObject[] prefab, Transform parent, string itemType)
        {
            int placed = 0;
            int attempts = 0;
            int maxAttempts = Mathf.Max(1000, count * 50);

            while (placed < count && attempts++ < maxAttempts)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                var p = new Vector2Int(x, y);

                // ห้ามทับ Player/Exit
                if (p == playerStartPos || (x == X - 1 && y == Y - 1))
                    continue;

                // ห้ามทับ backbone (หากเปิดใช้)
                if (reservedBackbone.Contains(p))
                    continue;

                if (mapdata[x, y] != null)
                    continue;

                // เช็ค Path-Guard: ถ้าวางที่ (x,y) แล้วทางจาก start → exit ยังมีอยู่ไหม
                if (HasPath(playerStartPos, new Vector2Int(X - 1, Y - 1), p))
                {
                    // ผ่าน → ค่อย Instantiate จริง
                    SetUpItem(x, y, prefab, parent, itemType);
                    placed++;
                }
                // ถ้าไม่ผ่านจะลองตำแหน่งใหม่
            }

            if (placed < count)
                Debug.LogWarning($"Obstacle placed {placed}/{count}. (ลดจำนวนสิ่งกีดขวางหรือขยายแผนที่)");
        }

        // ---------------- PATH CHECKING ----------------
        private bool InBounds(int x, int y) => (x >= 0 && x < X && y >= 0 && y < Y);

        private bool IsBlocking(Identity id)
        {
            if (id == null) return false;

            // demonWall กันทางเสมอ
            if (id.Name == demonWall) return true;

            // exit / item / collectItem / potion ฯลฯ เดินทับได้ตามเกมคุณ
            // enemy กันทางหรือไม่ ให้เลือกด้วยสวิตช์
            if (enemiesBlockPath && id.Name == enemy) return true;

            return false;
        }

        /// <summary>
        /// เช็คว่ามีทางจาก start → goal หรือไม่ โดยถือว่า tempBlocked (ถ้ามี) เป็นช่องที่ “บล็อก” ชั่วคราว
        /// </summary>
        private bool HasPath(Vector2Int start, Vector2Int goal, Vector2Int? tempBlocked = null)
        {
            if (!InBounds(start.x, start.y) || !InBounds(goal.x, goal.y)) return false;

            // ถ้า start/goal โดนบล็อกอยู่ก็ล้มเหลว
            if (IsBlocking(mapdata[start.x, start.y]) || IsBlocking(mapdata[goal.x, goal.y])) return false;

            var q = new Queue<Vector2Int>();
            var visited = new bool[X, Y];

            q.Enqueue(start);
            visited[start.x, start.y] = true;

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            while (q.Count > 0)
            {
                var p = q.Dequeue();
                if (p == goal) return true;

                for (int i = 0; i < 4; i++)
                {
                    int nx = p.x + dx[i];
                    int ny = p.y + dy[i];
                    if (!InBounds(nx, ny)) continue;

                    // ถ้าเป็นช่องบล็อกชั่วคราว ให้ข้าม
                    if (tempBlocked.HasValue && nx == tempBlocked.Value.x && ny == tempBlocked.Value.y)
                        continue;

                    if (visited[nx, ny]) continue;

                    var id = mapdata[nx, ny];
                    if (!IsBlocking(id))
                    {
                        visited[nx, ny] = true;
                        q.Enqueue(new Vector2Int(nx, ny));
                    }
                }
            }
            return false;
        }

        // ---------------- BACKBONE PATH (OPTIONAL) ----------------
        private void BuildBackbonePath()
        {
            reservedBackbone.Clear();

            var start = playerStartPos;
            var goal = new Vector2Int(X - 1, Y - 1);

            var prev = new Dictionary<Vector2Int, Vector2Int>();
            var q = new Queue<Vector2Int>();
            var visited = new bool[X, Y];

            q.Enqueue(start);
            visited[start.x, start.y] = true;

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            bool found = false;

            while (q.Count > 0)
            {
                var p = q.Dequeue();
                if (p == goal) { found = true; break; }

                for (int i = 0; i < 4; i++)
                {
                    int nx = p.x + dx[i];
                    int ny = p.y + dy[i];
                    if (!InBounds(nx, ny)) continue;
                    if (visited[nx, ny]) continue;

                    var id = mapdata[nx, ny];
                    if (!IsBlocking(id))
                    {
                        visited[nx, ny] = true;
                        var np = new Vector2Int(nx, ny);
                        prev[np] = p;
                        q.Enqueue(np);
                    }
                }
            }

            if (!found) return;

            // ไล่ย้อนทางสั้นสุด goal → start แล้วบันทึกเป็นช่องที่ “กันสุ่มทับ”
            var cur = goal;
            reservedBackbone.Add(cur);
            while (prev.ContainsKey(cur))
            {
                cur = prev[cur];
                reservedBackbone.Add(cur);
            }
        }
    }
}