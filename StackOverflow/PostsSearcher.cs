public class PostsSearcher
{
    public Dictionary<string, List<Post>> tagPosts { get; private set; } = [];
    public Dictionary<User, List<Post>> userPosts { get; private set; } = [];
    //todo: trie<string,post[]>
}
