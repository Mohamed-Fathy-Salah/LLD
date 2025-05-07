# Designing Stack Overflow

## Requirements
1. Users can post questions, answer questions, and comment on questions and answers.
2. Users can vote on questions and answers.
3. Questions should have tags associated with them.
4. Users can search for questions based on keywords, tags, or user profiles.
5. The system should assign reputation score to us based on their activity and the quality of their contributions.
6. The system should handle concurrent access and ensure data consistency.

## Exit Gate Sequence Diagram
```mermaid
classDiagram
    class StackOverflow {
        PostsManager postsManager
        PostsSearcher postsSearcher
        UserEvaluator UserEvaluator
    }

    class PostsSearcher {
        Map~string,Post[]~ tagPosts
        Map~User,Post[]~ userPosts
        Trie~string,Post[]~
        Post[] GetAllFiltered(string[] keywords, string[] tags, User[] u)
    }

    class UserEvaluator {
        bool AddVote(User user, Post p, Votable v, int voteId)
        bool RemoveVote(User user, Post p, Votable v, int voteId)
    }

    class PostsManager {
        Map~int,Post~ posts
        Observers[] o
        int AddPost(User u, string question, string[] tags)
        bool EditPost(User u, int postId, string? question, string[] removedTags, string[] addedTags)
        bool RemovePost(User u, int postId)
        int AddAnswer(User u, Post p, string answer)
        bool EditAnswer(User u, Post p, int answerId, string answer)
        bool RemoveAnswer(User u, Post p, int answerId)
        int AddComment(User u, Post p, int paragraphId, string comment)
        bool EditComment(User u, Post p, int paragraphId, int commentId, string comment)
        bool RemoveComment(User u, Post p, int paragraphId, int commentId)
        void NotifyAll()
    }

    class User {
        string name
        byte reputation 0->255 bigger is better
    }

    class Post {
        Question question
        Answer[] answers
    }

    class Commentable {
        <<abstract>>
        Map~int, Comment~ comments
        int AddComment(User u, string comment)
        bool EditComment(User u, int commentId, string comment)
        bool RemoveComment(User u, int commentId)
    }

    class Votable {
        <<abstract>>
        Map~int, Vote~ votes
        int AddVote(User u, VoteEnum v)
        bool EditVote(User u, int voteId, VoteEnum v)
        bool RemoveVote(User u, int voteId)
    }

    class Comment {
        int ID
        User u 
        string comment
    }

    class Paragraph {
        <<abstract>>
        int ID
        string text
    }

    class Question {
        string[] tags
    }

    class Vote {
        User u
        VoteEnum value
    }

    class VoteEnum {
        <<enumeration>>
        UP_VOTE
        DOWN_VOTE
    }

    StackOverflow --> Post
    StackOverflow --> PostsManager
    StackOverflow --> PostsSearcher
    StackOverflow --> UserEvaluator
    Votable --> Vote
    Commentable --> Comment
    Post --> Question
    Post --> Answer
    Answer --> Paragraph
    Question --> Paragraph
    Answer --> Commentable
    Question --> Commentable
    Answer --> Votable
    Question --> Votable
```
