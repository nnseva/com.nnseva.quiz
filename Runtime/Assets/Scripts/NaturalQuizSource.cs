using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

abstract public class NaturalQuizSource : QuizSource {
    // Natural counting quiz source
    [Tooltip("Number of digits in the test values")]
    public int Digits = 1;
    public override string category { get { return GetLocalizedString("Natural numbers"); } }
    public override string theme { get { return GetLocalizedString("Performing {0} of {1}-digits numbers", action_name, Digits); } }
    virtual public int a(int id) {
        return (int)(id / Mathf.Pow(10f, (float)Digits));
    }
    virtual public int b(int id) {
        return (int)(id % Mathf.Pow(10f, (float)Digits));
    }
    public override int random_id() {
        float mn = Mathf.Pow(10f, (float)(Digits - 1));
        float mx = Mathf.Pow(10f, (float)(Digits));
        int x = (int)Random.Range(mn, mx);
        int y = (int)Random.Range(mn, mx);
        return x * (int) Mathf.Pow(10f, Digits) + y;
    }
    public override string question(int id) {
        return GetLocalizedString("How much will {0} {1} {2}?", a(id), action_sign, b(id));
    }
    public override string answer(int id) {
        return GetLocalizedString("{0}", action(a(id), b(id)));
    }
    abstract public int action(int a, int b);
    abstract public string action_sign { get; } // Action sign: +,-,*,:
    abstract public string action_name { get; } // Action name like "addition"
}
