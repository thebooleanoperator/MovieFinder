﻿namespace MovieFinder.DtoModels
{
    public class UserSearchHistoryDto
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
    }
}
