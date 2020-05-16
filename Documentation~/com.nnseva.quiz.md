# Quiz Generation Environment

## Installation

Use Package Manager to add the package from the https://github.com/nnseva/com.nnseva.quiz.git

Select the package in the package manager and install internalization support using `Install` button in the `Samples` section.

Don't forget to generate locales using `Generate Locale` tool of the `Assets/Localization Settings`.

*Note* Supported locales for now: `en`, `ru`.

## Using in the code

Use quiz registry `Packages/Quiz Generator Environment/Runtime/Assets/Scripts/QuizRegistry` script instance on your component,
and add necessary quiz sources `Packages/Quiz Generator Environment/Runtime/Assets/Scripts/*QuizSource` script instances to
the same component to generate correspondent quizes.

Tune added quiz source instances to adopt quiz complexity. In order to generate a quiz, just ask `random_quiz()` method
of the quiz registry instance.

You can use different sets of quiz sources on different components.

The `random_quiz()` returns an instance implementing `IQuiz` interface:

```C#
public interface IQuiz {
    IQuizSource source { get; } // Source of the quiz
    int id { get; } // ID of the generated quiz
    string question { get; } // Question of the quiz
    string answer { get; } // Proper answer of the quiz
    string[] choices { get; } // Several choices (with the proper one among all) to select from
}
```

The `IQuiz` instance having the same `source` and `id` will return the same question and proper answer.
The `choices` attrbute will be different as it is generated every time when the `IQuiz` is created.

See the package source in the [project repository](https://github.com/nnseva/com.nnseva.quiz)
