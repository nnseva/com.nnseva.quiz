public class NaturalProductQuizSource : NaturalQuizSource {
    public override string action_sign { get { return "*"; }}
    public override string action_name { get { return GetLocalizedString("multiplication"); }}
    public override string quiz_name { get { return GetLocalizedString("Multiplication"); }}
    public override int action(int a, int b) { return a * b; }
}
