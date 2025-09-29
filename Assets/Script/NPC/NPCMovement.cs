using UnityEngine;
using UnityEngine.AI; // Penting untuk menggunakan NavMeshAgent

public class NPCMovement : MonoBehaviour
{
    // Deklarasi variabel
    private NavMeshAgent agent;
    
    [Header("Pengaturan Pergerakan")]
    [Tooltip("Daftar posisi (Waypoint) yang akan didatangi NPC.")]
    public Transform[] patrolPoints; 
    
    [Tooltip("Kecepatan normal pergerakan NPC.")]
    public float movementSpeed = 3.5f;
    
    [Tooltip("Jarak terdekat ke tujuan sebelum dianggap sudah sampai.")]
    public float stoppingDistance = 0.5f; 
    
    private int currentPointIndex = 0; // Indeks waypoint saat ini
    
    void Start()
    {
        // Mendapatkan komponen NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        
        // Pastikan NavMeshAgent ditemukan
        if (agent == null)
        {
            Debug.LogError("Komponen NavMeshAgent tidak ditemukan pada GameObject ini!");
            return;
        }

        // Terapkan kecepatan yang ditentukan
        agent.speed = movementSpeed;
        agent.stoppingDistance = stoppingDistance;

        // Mulai pergerakan ke titik pertama jika ada
        if (patrolPoints.Length > 0)
        {
            SetNextDestination(patrolPoints[currentPointIndex].position);
        }
        else
        {
            Debug.LogWarning("Daftar Patrol Points kosong. NPC tidak akan bergerak.");
        }
    }

    void Update()
    {
        // Cek apakah NPC sudah sangat dekat dengan tujuan (Waypoint) saat ini
        if (agent.isActiveAndEnabled && !agent.pathPending)
        {
            // agent.remainingDistance < agent.stoppingDistance
            // Serta pathnya sudah terhitung sempurna
            if (agent.remainingDistance <= agent.stoppingDistance && 
                (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
            {
                // Pindah ke waypoint berikutnya
                MoveToNextPoint();
            }
        }
    }

    // Fungsi untuk menetapkan tujuan baru
    void SetNextDestination(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
    
    // Fungsi untuk berpindah ke titik patroli berikutnya
    void MoveToNextPoint()
    {
        // Hitung indeks berikutnya (berputar kembali ke 0 jika sudah mencapai akhir)
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;

        // Tetapkan tujuan baru
        SetNextDestination(patrolPoints[currentPointIndex].position);
    }
}