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
        public Identity Wall;

        [Header("Set Prefab")]
        public GameObject[] floorsPrefab;
        public GameObject[] wallsPrefab;
        public GameObject[] demonWallsPrefab;
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

        [HideInInspector] public string empty = "";
        [HideInInspector] public string demonWall = "demonWall";
        [HideInInspector] public string potion = "potion";
        [HideInInspector] public string bonuesPotion = "bonuesPotion";
        [HideInInspector] public string exit = "exit";
        [HideInInspector] public string playerOnMap = "player";
        [HideInInspector] public string collectItem = "collectItem";
        [HideInInspector] public string enemy = "enemy";

        [Header("Path Guard Options")]
        public bool enemiesBlockPath = false;
        public bool buildBackboneFirst = true;

        private HashSet<Vector2Int> reservedBackbone = new HashSet<Vector2Int>();

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

            yield return StartCoroutine(PlaceObstaclesWithPathGuardCoroutine(obsatcleCount, demonWallsPrefab, wallParent, demonWall));

            PlaceItemsOnMap(itemPotionCount, itemsPrefab, itemParent, potion);
            PlaceItemsOnMap(colloctItemCount, collectItemsPrefab, itemParent, collectItem);
            PlaceItemsOnMap(SkillCount, SkillPrefab, itemParent, collectItem);
            PlaceItemsOnMap(EnemyCount, EnemyPrefab, enemyParent, enemy);

            yield return new WaitForSeconds(0.5f);
            RandomDamageToListEnemies();
        }

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
                        mapdata[x, y] = null;
                    }
                }
            }
        }

        private void SetUpPlayer()
        {
            // 🌀 สุ่มตำแหน่งเริ่มต้นของ Player
            playerStartPos = GetRandomEmptyPosition();
            player.mapGenerator = this;
            player.positionX = playerStartPos.x;
            player.positionY = playerStartPos.y;
            player.transform.position = new Vector3(playerStartPos.x, playerStartPos.y, -0.1f);
            mapdata[playerStartPos.x, playerStartPos.y] = player;
        }

        private void SetUpExit()
        {
            // 🌀 สุ่มตำแหน่ง Exit โดยให้ห่างจาก Player อย่างน้อย 5 ช่อง (หรือปรับค่าได้)
            int minDistance = Mathf.Max(X, Y) / 3; // ปรับระยะได้ตามใจท่านจอมมาร
            Vector2Int exitPos;

            int loopGuard = 500;
            do
            {
                exitPos = GetRandomEmptyPosition();
                loopGuard--;
            }
            while (Vector2Int.Distance(exitPos, playerStartPos) < minDistance && loopGuard > 0);

            Exit.positionX = exitPos.x;
            Exit.positionY = exitPos.y;
            Exit.mapGenerator = this;
            Exit.transform.position = new Vector3(exitPos.x, exitPos.y, 0);
            mapdata[exitPos.x, exitPos.y] = Exit;
        }

        private Vector2Int GetRandomEmptyPosition()
        {
            int tries = 0;
            while (tries < 1000)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == null)
                    return new Vector2Int(x, y);
                tries++;
            }

            // ถ้าหาไม่ได้จริง ๆ ก็ fallback
            return new Vector2Int(0, 0);
        }

        public Identity GetMapData(float x, float y)
        {
            if (x >= X || x < 0 || y >= Y || y < 0)
                return Wall;
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

        private void PlaceItemsOnMap(int count, GameObject[] prefab, Transform parent, string itemType, System.Action onComplete = null)
        {
            int placedCount = 0;
            int preventInfiniteLoop = 2000;

            while (placedCount < count && preventInfiniteLoop-- > 0)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);

                if ((x == playerStartPos.x && y == playerStartPos.y) || (x == X - 1 && y == Y - 1))
                    continue;

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

        private IEnumerator PlaceObstaclesWithPathGuardCoroutine(int count, GameObject[] prefab, Transform parent, string itemType)
        {
            int placed = 0;
            int attempts = 0;
            int maxAttempts = Mathf.Max(500, count * 20);

            Debug.Log($"Start placing {count} obstacles on {X}x{Y} map");

            while (placed < count && attempts < maxAttempts)
            {
                attempts++;

                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                var p = new Vector2Int(x, y);

                if (p == playerStartPos || (x == X - 1 && y == Y - 1)) continue;
                if (reservedBackbone.Contains(p)) continue;
                if (mapdata[x, y] != null) continue;

                if (HasPath(playerStartPos, new Vector2Int(X - 1, Y - 1), p))
                {
                    SetUpItem(x, y, prefab, parent, itemType);
                    placed++;
                }

                if (attempts % 100 == 0)
                    yield return null;
            }

            if (placed < count)
                Debug.LogWarning($"Obstacle placed {placed}/{count}. (ลดจำนวนสิ่งกีดขวางหรือขยายแผนที่)");

            yield return null;
        }

        private bool InBounds(int x, int y) => (x >= 0 && x < X && y >= 0 && y < Y);

        private bool IsBlocking(Identity id)
        {
            if (id == null) return false;
            if (id.Name == demonWall) return true;
            if (enemiesBlockPath && id.Name == enemy) return true;
            return false;
        }

        // 🌀 เพิ่มฟังก์ชันสุ่มทิศทาง
        private void ShuffleDirections(int[] dx, int[] dy)
        {
            for (int i = 0; i < dx.Length; i++)
            {
                int r = Random.Range(i, dx.Length);
                (dx[i], dx[r]) = (dx[r], dx[i]);
                (dy[i], dy[r]) = (dy[r], dy[i]);
            }
        }

        private bool HasPath(Vector2Int start, Vector2Int goal, Vector2Int? tempBlocked = null)
        {
            if (!InBounds(start.x, start.y) || !InBounds(goal.x, goal.y)) return false;
            if (IsBlocking(mapdata[start.x, start.y]) || IsBlocking(mapdata[goal.x, goal.y])) return false;

            var q = new Queue<Vector2Int>();
            var visited = new bool[X, Y];

            q.Enqueue(start);
            visited[start.x, start.y] = true;

            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };

            ShuffleDirections(dx, dy); // 🔧 เพิ่มสุ่มทิศทางตรงนี้

            while (q.Count > 0)
            {
                var p = q.Dequeue();
                if (p == goal) return true;

                for (int i = 0; i < 4; i++)
                {
                    int nx = p.x + dx[i];
                    int ny = p.y + dy[i];
                    if (!InBounds(nx, ny)) continue;

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

            ShuffleDirections(dx, dy); // 🔧 เพิ่มสุ่มทิศทางตรงนี้

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
