public class StackOverflow
{
    public PostsManager postsManager { get; } = new PostsManager();
    public PostsSearcher postsSearcher { get; } = new PostsSearcher();
    public UserEvaluator userEvaluator { get; } = new UserEvaluator();
}
