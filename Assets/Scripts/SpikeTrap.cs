using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Hitbox hitbox;

    private const float RETRACTED_POSITION = -0.5f;
    private const float ATTACK_POSITION = 0.5f;

    [SerializeField] private Transform _spikesTransform;

    public float attackSpeed = 1f;
    public float retractSpeed = 1f;
    public float secondsWaitingUp = 1f;
    public float secondsToAttack = 1f;

    [SerializeField] private BoxCollider _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        hitbox.e_OnHit += () => enabled = _boxCollider.enabled = false;
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        float interpolation = 0;

        while (_spikesTransform.localPosition.y < ATTACK_POSITION)
        {
            interpolation += Time.deltaTime * attackSpeed;
            var yPos = Mathf.Lerp(RETRACTED_POSITION, ATTACK_POSITION, interpolation);
            _spikesTransform.localPosition = new Vector3(_spikesTransform.localPosition.x, yPos, _spikesTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(secondsWaitingUp);

        _spikesTransform.localPosition = new Vector3(_spikesTransform.localPosition.x, ATTACK_POSITION, _spikesTransform.localPosition.z);
        StartCoroutine(Retract());
    }

    IEnumerator Retract()
    {
        _boxCollider.isTrigger = false;

        float interpolation = 0;

        while (_spikesTransform.localPosition.y > RETRACTED_POSITION)
        {
            interpolation += Time.deltaTime * retractSpeed;
            var yPos = Mathf.Lerp(ATTACK_POSITION, RETRACTED_POSITION, interpolation);
            _spikesTransform.localPosition = new Vector3(_spikesTransform.localPosition.x, yPos, _spikesTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }

        _spikesTransform.localPosition = new Vector3(_spikesTransform.localPosition.x, RETRACTED_POSITION, _spikesTransform.localPosition.z);

        _boxCollider.isTrigger = true;
        yield return new WaitForSeconds(secondsToAttack);

        StartCoroutine(Attack());
    }
}
