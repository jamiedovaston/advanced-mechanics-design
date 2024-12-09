public interface IPossessable
{
    public void Init(TankSO _data);

    public void Accelerate(float _inAccelerate);
	public void Steer(float _inSteer, bool _isSteering);
	public void Fire(bool _isFiring);
	public void Aim();
	public void Zoom();
}
