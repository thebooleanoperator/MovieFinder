using MovieFinder.Utils;

namespace MovieFinder.Auth
{
    public static class MovieValidator
    {
        public static bool MatchUser(int userId, Session session)
        {
            if (userId == session.UserId)
            {
                return true;
            }

            return false;
        }

    }
}
