using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject objManager;
    public ObjManager Objmanager;

    public bool isBreak = false;
    public bool isAttack = false;
    public bool isActive = false;
    public bool isBlocking = false;

    bool isDash = true;
    bool overheating = false;
    bool isFar = true;
    bool move = false;
    public float hp;

    public bool boomActive = false;
    public float speed = 1;
    public bool isStun = false;
    public int attackNum;
    public int lastAttack = 0;
    public int count;
    public bool isturnhead = false;
    public float bossSpeed = 1f;
    public int isSpecial = 0;
    public int barrierNum = 3;
    public int remainAttack = 5;

    bool sp = false;

    Animator anim;
    void Start()
    {
        Objmanager= objManager.GetComponent<ObjManager>();
        Physics.gravity = Physics.gravity;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isSpecial==0)
        {
            float dir = Vector3.Distance(transform.position, player.transform.position);

            DisCheck();
            if (!isActive && Input.GetKeyDown(KeyCode.Space) && dir < 4)
            {
                anim.SetBool("isActive", true);
                StartCoroutine(deley(3f));
            }
            if (move)
            {
                if (dir < 7)
                {
                    move = false;
                }
                anim.SetBool("isWalk", true);
                StartCoroutine(TurnHead());
            }
            if (isActive && !isAttack && !isBreak && !isStun)
            {

                if (isFar && !isturnhead)
                {
                    if (count == 3)
                        StartCoroutine(Attack());
                    else
                        StartCoroutine(TurnHead());
                }
                else
                {
                    StartCoroutine(Attack());
                }
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                StartCoroutine(TurnHead());
            }
        }
        else if (isSpecial == 1) 
        {
            isSpecial = 0;
            isAttack = true;
            StartCoroutine(SpecialAttack1());
        }
        else if (isSpecial == 2)
        {
            isSpecial = 0;
            isAttack = true;
            StartCoroutine(SpecialAttack2());
        }
    }
    void DisCheck()
    {
        float dir = Vector3.Distance(transform.position, player.transform.position);
        if (dir < 7)
        {
            isFar = false;
            anim.SetBool("isFar", false);
        }
        else
        {
            isFar = true;
            anim.SetBool("isFar", true);
        }

    }
    IEnumerator Attack() // ���� ���� ������ ����
    {
        if (!isAttack && isActive)
        {
            if (count == 3 && !overheating)
            {
                attackNum = UnityEngine.Random.Range(9, 11);
                isAttack = true;
                yield return StartCoroutine(TurnHead());
                yield return StartCoroutine(TurnHead());
                anim.SetBool("isAttack", true);
                switch (attackNum)
                {
                    case 9:
                        StartCoroutine(ElementAttack1());
                        break;
                    case 10:
                        StartCoroutine(ElementAttack2());
                        break;
                }
                StartCoroutine(TurnHead());
            }
            else
            {
                attackNum = UnityEngine.Random.Range(1, 7);
                if (attackNum != lastAttack)
                {
                    isAttack = true;
                    yield return StartCoroutine(TurnHead());
                    anim.SetBool("isAttack", true);
                    switch (attackNum)
                    {
                        case 1:
                            StartCoroutine(Attack1());
                            break;
                        case 2:
                             StartCoroutine(Attack2());
                            break;
                        case 3:
                            if (!boomActive)
                                StartCoroutine(Attack3());
                            else isAttack = false;
                            break;
                        case 4:
                            StartCoroutine(Attack4());
                            break;
                        case 5:
                            StartCoroutine(Attack5());
                            break;
                        case 6:
                            StartCoroutine(Attack6());
                            break;
                        case 7:
                            break;
                        case 8:
                            break;
                    }
                    StartCoroutine(TurnHead());
                    lastAttack = attackNum;
                    count++;
                }
                else
                    isAttack = false;
            }
        }
    }
    IEnumerator deley(float i) //���� ���� ��
    {
        if(!isActive)
        {
            yield return new WaitForSeconds(i);
            isActive = true;
            yield break;
        }
        anim.SetBool("isBreak", true);
        isBreak = true;
        yield return new WaitForSeconds(i / bossSpeed);
        DisCheck();
        anim.SetBool("isBreak", false);
        isBreak = false;
        anim.SetBool("isAttack", false);
        isAttack = false;
        yield break;
    }
    IEnumerator Attack1()
    {
        move = true;
        while (isFar) {
            yield return new WaitForFixedUpdate();
                }
        anim.SetInteger("AttackType", 1);
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern1"))
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        anim.SetInteger("AttackType", 0);
        yield return StartCoroutine(deley(1));
        yield break;
    }
    IEnumerator Attack2()
    {
        anim.SetInteger("AttackType", 2);
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern2") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
                break;
            yield return new WaitForEndOfFrame();
        }
        Objmanager.BigRockActive();
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern2") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.93f)
                break;
            yield return new WaitForEndOfFrame();
        }
        Objmanager.BigRockInactive();
        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern2"))
                break;
            yield return new WaitForEndOfFrame();
        }
        anim.SetInteger("AttackType", 0);
        yield return StartCoroutine(deley(2));
        yield break;
    }
    IEnumerator Attack3()
    {
        anim.SetInteger("AttackType", 3);
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern3") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f)
                break;
            yield return new WaitForEndOfFrame();
        }
        Objmanager.BombActive();
        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern3"))
                break;
            yield return new WaitForEndOfFrame();
        }
        anim.SetInteger("AttackType", 0);
        yield return StartCoroutine(TurnHead());
        DisCheck();
        yield return StartCoroutine(Attack1());
        yield break;
    }// 점찍으로 고쳐보기
    IEnumerator Attack4()
    {
        anim.SetInteger("AttackType", 4);
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern4") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
                break;
            yield return new WaitForEndOfFrame();
        }
        for (int i=0;i<6;i++)
        {
            Objmanager.DropGear(i);
        }
        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern4"))
                break;
            yield return new WaitForEndOfFrame();
        }
        anim.SetInteger("AttackType", 0);
        yield return StartCoroutine(deley(2f));
        yield break;
    }
    IEnumerator Attack5()
    {
        anim.SetInteger("AttackType", 5);
        {
            while (true)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern5") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
                    break;
                yield return new WaitForEndOfFrame();
            }
        }
        Objmanager.DropRockActive(3, 0.15f);
        {
            while (true)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern5") &&
                    anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.45f)
                    break;
                yield return new WaitForEndOfFrame();
            }
        }
        Objmanager.DropRockActive(3, 0.15f);
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern5")&&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.8f)
                break;
            yield return new WaitForEndOfFrame();
        }
        Objmanager.DropRockActive(8, 0.15f);
        anim.SetInteger("AttackType", 0);
        yield return StartCoroutine(deley(1.5f));
        yield break;
    }
    IEnumerator Attack6()
    {
        anim.SetInteger("AttackType", 6);
        yield return new WaitForSeconds(0.3f);
        while (true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Pattern6"))
                break;
            yield return new WaitForEndOfFrame();
        }
        anim.SetInteger("AttackType", 0);
        yield return StartCoroutine(deley(1.5f));
        yield break;
    }
    IEnumerator ElementAttack1()
    {
        anim.SetInteger("AttackType", 9);
        yield return new WaitForSeconds(1);

        while (true)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Element1-1"))
                break;
            if(isStun)
            {
                anim.SetBool("isStun", true);
                yield return new WaitForSeconds(0.3f);
                anim.SetBool("isStun", false);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        while (true)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Element1-2") &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)
            {
                anim.SetInteger("AttackType", 0);
                for (int i = 0; i < 16; i++)
                {
                    Objmanager.SpawnFire(i);
                    yield return new WaitForSeconds(0.1f / bossSpeed);
                }
                break;
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Stun"))
            {
                yield return new WaitForSeconds(3 / bossSpeed);
                anim.SetFloat("StunMultiplier", 1);
                isStun = false;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(deley(3));
        isAttack = false;
        count = 0;
     }
    IEnumerator ElementAttack2()
    {
        yield return new WaitForSeconds(1);

        anim.SetInteger("AttackType", 10);
        yield return new WaitForSeconds(0.5f);

        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Element2-1"))
        {
            StartCoroutine(TurnHead());
            yield return new WaitForEndOfFrame();
        }
        isDash = true;
        int i = 0;
        while (i<30/bossSpeed)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Element2-1")&&isDash)
            {
                    transform.position += Vector3.Normalize(transform.forward) * 20 / 30 / bossSpeed;
            }
            i++;
            yield return new WaitForEndOfFrame();
        }

        anim.SetBool("isBreak", true);
        anim.SetInteger("AttackType", 0);
        StartCoroutine(deley(3));   
        StartCoroutine(TurnHead());
        DisCheck();
        isAttack = false;
        attackNum = 0;
        count = 0;
    }
    IEnumerator SpecialAttack1()
    {
        StopCoroutine(OverHeat());
        sp = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        overheating = false;
        anim.SetFloat("AttackSpeed", 1);
        anim.SetInteger("SpacialAttack", 1);
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("SpacialAttack", 0);
    }
    IEnumerator SpecialAttack2()
    {
        StopCoroutine(OverHeat());
        sp = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        overheating = false;
        anim.SetFloat("AttackSpeed", 1);
        anim.SetInteger("SpacialAttack", 2);
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("SpacialAttack", 0);
    }
    public IEnumerator TurnHead()
    {
        if (!isturnhead)
        {
            isturnhead = true;
            Vector3 playerPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

            for (float i = 0; i < 25; i++)
            {
                Vector3 t_dir = (playerPos - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, t_dir, 0.04f);
                yield return new WaitForFixedUpdate();
            }
            isturnhead = false;
        }
    }
    public IEnumerator Stun(float i)
    {
        isStun = true;
        transform.GetComponent<BossAttack>().Disable1();
        anim.SetFloat("AttackSpeed", 1);
        anim.SetBool("isStun", true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isStun", false);
        yield return new WaitForSeconds(i);
        anim.SetFloat("StunMultiplier", 1);
        overheating = false;
        isAttack = false;
        isStun = false;
        attackNum = 0;
        bossSpeed = 1;
        if (sp)
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
            sp = false;
        }
    }
    public IEnumerator OverHeat()
    {
        overheating = true;
        yield return new WaitForSeconds(15);
        //boss damage
        StartCoroutine(Stun(5));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
            isDash = false;
    }
}
