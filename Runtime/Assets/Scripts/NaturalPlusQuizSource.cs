public class NaturalPlusQuizSource : NaturalQuizSource {
    public override string action_sign { get { return "+"; }}
    public override string action_name { get { return GetLocalizedString("addition"); }}
    public override string quiz_name { get { return "Addition"; }}
    public override int action(int a, int b) { return a + b; }
}
