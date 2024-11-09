using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Музыка")]
    [SerializeField] private AudioClip[] gameTracks; // Массив треков для игрового процесса
    [SerializeField] private AudioClip deathScreenMusic; // Трек для экрана смерти
    [SerializeField] private AudioSource musicSource;

    [Header("Звуковые эффекты")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip katanaSound;
    [SerializeField] private AudioClip enemyShootSound;
    [SerializeField] private AudioClip playerHurtSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioSource sfxSource;

    public static AudioManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        PlayRandomGameTrack(); // Запускаем случайный трек в начале
    }

    private void PlayRandomGameTrack()
    {
        if (gameTracks.Length > 0)
        {
            int randomIndex = Random.Range(0, gameTracks.Length);
            musicSource.clip = gameTracks[randomIndex];
            musicSource.Play();
        }
    }

    public void PlayDeathScreenMusic()
    {
        musicSource.clip = deathScreenMusic;
        musicSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Отдельные методы для вызова нужного звука
    public void PlayShootSound() => PlaySoundEffect(shootSound);
    public void PlayKatanaSound() => PlaySoundEffect(katanaSound);
    public void PlayEnemyShootSound() => PlaySoundEffect(enemyShootSound);
    public void PlayPlayerHurtSound() => PlaySoundEffect(playerHurtSound);
    public void PlayJumpSound() => PlaySoundEffect(jumpSound);
    public void PlayDashSound() => PlaySoundEffect(dashSound);

    // Метод для смены трека после завершения текущего
    private void Update()
    {
        if (!musicSource.isPlaying && musicSource.clip != deathScreenMusic)
        {
            PlayRandomGameTrack();
        }
    }
}
