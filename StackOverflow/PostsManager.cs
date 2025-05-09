using System.Collections.Concurrent;

public class PostsManager(IPostObserver[] observers)
{
    public ConcurrentDictionary<int, Post> posts { get; } = [];

    int AddPost(User u, string question, string[] tags) { }
    bool EditPost(User u, int postId, string? question = null, string[]? removedTags = null, string[]? addedTags = null) { }
    bool RemovePost(User u, int postId) { }

    int AddAnswer(User u, int postId, string answer) { }
    bool EditAnswer(User u, int postId, int answerId, string answer) { }
    bool RemoveAnswer(User u, int postId, int answerId) { }

    int AddComment(User u, int postId, int paragraphId, string comment) { }
    bool EditComment(User u, int postId, int paragraphId, int commentId, string comment) { }
    bool RemoveComment(User u, int postId, int paragraphId, int commentId) { }

    //todo: notify
}
