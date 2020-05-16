using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

using System.Collections;

public class QuizRegistry : MonoBehaviour {
    StringTable m_translations;
    public string GetLocalizedString(string id, params object[] args) {
        if( !m_translations ) {
            return string.Format(id, args);
        }
        return m_translations.GetEntry(id).GetLocalizedString(args);
    }
    IEnumerator Start() {
        yield return LocalizationSettings.InitializationOperation;
        AsyncOperationHandle handle = LocalizationSettings.Instance.GetStringDatabase().GetTableAsync("quiz");
        yield return handle;
        m_translations = (StringTable) handle.Result;
    }
    public IEnumerable<IQuizSource> sources
    {
        get
        {
            foreach(QuizSource src in GetComponentsInParent<QuizSource>()) {
                yield return src;
            }
        }
    }
    public void DebugRandomQuiz() {
        IQuiz quiz = random_quiz();
        Debug.Log(string.Format(">>>>>> quiz: {0} {1} {2}", quiz.question, string.Join(", ", quiz.choices), quiz.answer));
    }
    public IQuiz random_quiz() {
        List<IQuizSource> sources_list = new List<IQuizSource>(sources);
        int index = (int) Random.Range(0f, (float)sources_list.Count);
        IQuizSource src = sources_list[index];
        return src.create_quiz(src.random_id());
    }
    public string[] categories() {
        /// Returns an array of all categories in the registry
        SortedSet<string> ret = new SortedSet<string>();
        foreach(IQuizSource src in sources) {
            ret.Add(src.category);
        }
        string[] result = new string[ret.Count];
        ret.CopyTo(result, 0);
        return result;
    }
    public string[] themes(string category) {
        /// Returns an array of all themes in the category in the registry
        SortedSet<string> ret = new SortedSet<string>();
        foreach(IQuizSource src in sources) {
            if(src.category == category)
                ret.Add(src.theme);
        }
        string[] result = new string[ret.Count];
        ret.CopyTo(result, 0);
        return result;
    }
    public string[] quiz_names(string category, string theme) {
        /// Returns an array of all names in the theme in the category in the registry
        SortedSet<string> ret = new SortedSet<string>();
        foreach(IQuizSource src in sources) {
            if(src.category == category && src.theme == theme)
                ret.Add(src.quiz_name);
        }
        string[] result = new string[ret.Count];
        ret.CopyTo(result, 0);
        return result;
    }
    public IQuizSource[] quiz_sources(string category, string theme, string quiz_name) {
        /// Returns an array of all quiz sources having this name in the theme in the category in the registry
        SortedSet<IQuizSource> ret = new SortedSet<IQuizSource>();
        foreach(IQuizSource src in sources) {
            if(src.category == category && src.theme == theme && src.quiz_name == quiz_name)
                ret.Add(src);
        }
        IQuizSource[] result = new IQuizSource[ret.Count];
        ret.CopyTo(result, 0);
        return result;
    }
}

public interface IQuizSource
{
    string category { get; } // Category of the generated quiz to select from the interface
    string theme { get; } // Theme of the generated quiz to select from the interface
    string quiz_name { get; } // Name of the generated quiz to select from the interface
    int random_id(); // Generate a random ID to create a particular quiz
    IQuiz create_quiz(int id); // Generate a quiz from the ID, always the same quiz for the same ID
    QuizRegistry registry { get; } // Return register where it is registered
}

public interface IQuiz {
    IQuizSource source { get; } // Source of the quiz
    int id { get; } // ID of the generated quiz
    string question { get; } // Question of the quiz
    string answer { get; } // Proper answer of the quiz
    string[] choices { get; } // Several choices (with the proper one among all) to select from
}

abstract public class QuizSource : MonoBehaviour, IQuizSource {
    // Quiz source which is a component and can be installed
    [Tooltip("Number of returned choices")]
    public int ChoicesCount = 3;
    public string GetLocalizedString(string id, params object[] args) {
        if( !registry ) {
            Debug.Log("No registry!");
            return string.Format(id, args);
        }
        return registry.GetLocalizedString(id, args);
    }
    public QuizRegistry registry {
        get {
            return (QuizRegistry) GetComponentInParent<QuizRegistry>();
        }
    }
    public class Quiz : IQuiz {
        // Some simplest quiz
        QuizSource m_source;
        int m_id;
        string m_question;
        string m_answer;
        string[] m_choices;
        public Quiz(int id, QuizSource source) {
            m_id = id;
            m_source = source;
            m_question = m_source.question(m_id);
            m_answer = m_source.answer(m_id);
            List<string> answers = new List<string>();
            answers.Add(m_answer);
            for(int i=0; answers.Count < m_source.ChoicesCount; i++) {
                string a = m_source.answer(m_source.random_id());
                if( !answers.Contains(a) ) {
                    answers.Add(a);
                }
            }
            answers.Sort( (string x, string y) => Random.value > 0.5 ? 1:-1 );
            m_choices = new string[answers.Count];
            answers.CopyTo(m_choices);
        }
        public int id { get { return m_id; } }
        public IQuizSource source { get { return m_source; } }
        public string question { get {return m_question; } }
        public string answer { get {return m_answer; } }
        public string[] choices { get {return m_choices; } }
    }
    public IQuiz create_quiz(int id) {
        return new Quiz(id, this);
    }
    public abstract string category { get; }
    public abstract string theme { get; }
    public abstract string quiz_name { get; }
    public abstract int random_id();
    public abstract string question(int id);
    public abstract string answer(int id);
}
