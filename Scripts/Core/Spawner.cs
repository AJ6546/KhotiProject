using UnityEngine;

namespace KhotiProject.Scripts.Combat
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] int numberOfOpponents;
        [SerializeField] GameObject opponentPrefab;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] Terrain terrain;

        [SerializeField] float xOffSet = 5;
        [SerializeField] float yOffSet = 0.5f;
        [SerializeField] float zOffSet = 5;

        void Start()
        {
            Vector3 terrainSize = terrain.terrainData.size;
            var xPos = terrain.transform.position.x;
            var yPos = terrain.transform.position.y;
            var zPos = terrain.transform.position.z;

            for (int i = 0; i < numberOfOpponents; i++)
            {
                Spawn(terrainSize, xPos, yPos, zPos, opponentPrefab);
            }

            Spawn(terrainSize, xPos, yPos, zPos, playerPrefab);

            GetComponent<TurnManager>().InitiateGame();
        }

        private void Spawn(Vector3 terrainSize, float xPos, float yPos, float zPos, GameObject spawnObject)
        {
            var randomXPosition = Random.Range(xPos + xOffSet, xPos + terrainSize.x - xOffSet);
            var randomZPosition = Random.Range(zPos + zOffSet, zPos + terrainSize.z - zOffSet);
            Instantiate(spawnObject, new Vector3(randomXPosition, yPos + yOffSet, randomZPosition), Quaternion.identity);
        }
    }
}
