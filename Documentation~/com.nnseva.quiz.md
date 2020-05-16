# Quiz Generation Environment

Use quiz registry `QuizRegistry` instance on your component, and add necessary quiz sources `QuizSource` ancestors instances to the same component to generate correspondent quizes.

Tune added quiz source instances to adopt quiz complexity. In order to generate a quiz, just ask `random_quiz()` method of the quiz registry instance.

You can use different sets of quiz sources on different components.

The `random_quiz()` returns an instance implementing `IQuiz` interface containing:

- `question` property returns a question
- `answer` property corresponds to the proper answer to the question
- `choices` property contains an array of answers, where the only one is a proper

`answer` always contained in the `choices` array, but on the unknown place.

See the package source in the [project repository](https://github.com/nnseva/com.nnseva.quiz)
