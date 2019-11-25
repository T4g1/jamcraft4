/**
 * Can be hurt
 * Can die
 * Has life
 */
interface IAlive
{
    int HitPoints {
        get;
        set;
    }
    bool IsAlive {
        get;
        set;
    }

    void TakeDamage(int amount);
    void Die();
}