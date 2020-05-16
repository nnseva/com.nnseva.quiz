public class NaturalMinusQuizSource : NaturalQuizSource {
    public override string action_sign { get { return "-"; }}
    public override string action_name { get { return GetLocalizedString("substructing"); }}
    public override string quiz_name { get { return "Substructing"; }}
    public override int action(int a, int b) { return a - b; }
    public override int a(int id) {
        int a = base.a(id);
        int b = base.b(id);
        if(a > b) return a;
        return b;
    }     
    public override int b(int id) {
        int a = base.a(id);
        int b = base.b(id);
        if(a > b) return b;
        return a;
    }     
}
